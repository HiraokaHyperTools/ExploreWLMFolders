using ExploreWLMFolders.Attributes;
using Microsoft.Isam.Esent.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExploreWLMFolders.Utils
{
    class ES : IDisposable
    {
        String baseDir = String.Empty;
        String instId = "ExploreWLMFolders";

        public JET_INSTANCE instance = JET_INSTANCE.Nil;
        public JET_SESID sesid = JET_SESID.Nil;
        public JET_DBID dbid = JET_DBID.Nil;

        public bool isReadOnly;

        public event EventHandler Disposed;

        public void Open(String fpdb)
        {
            baseDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(baseDir);

            String tmp1 = Path.Combine(baseDir, "1");
            Directory.CreateDirectory(tmp1);
            String tmp2 = Path.Combine(baseDir, "2");
            Directory.CreateDirectory(tmp2);
            String tmp3 = Path.Combine(baseDir, "3");
            Directory.CreateDirectory(tmp3);

            Open2(fpdb, tmp1, tmp2, tmp3, true, 8192);
        }

        public void Open2(String fpdb, String tmp1, String tmp2, String tmp3, bool isReadOnly, int? DatabasePageSize)
        {
            Dispose();

            this.isReadOnly = isReadOnly;

            if (DatabasePageSize.HasValue) SystemParameters.DatabasePageSize = DatabasePageSize.Value;
            Api.JetCreateInstance(out instance, instId);
            Api.JetSetSystemParameter(instance, JET_SESID.Nil, JET_param.LogFilePath, 0, tmp1 + "\\");
            Api.JetSetSystemParameter(instance, JET_SESID.Nil, JET_param.TempPath, 0, tmp2 + "\\");
            Api.JetSetSystemParameter(instance, JET_SESID.Nil, JET_param.SystemPath, 0, tmp3 + "\\");
            Api.JetInit(ref instance);
            Api.JetBeginSession(instance, out sesid, null, null);
            EUt.Check(Api.JetAttachDatabase(sesid, fpdb, isReadOnly ? AttachDatabaseGrbit.ReadOnly : AttachDatabaseGrbit.None), "JetAttachDatabase");
            EUt.Check(Api.JetOpenDatabase(sesid, fpdb, null, out dbid, isReadOnly ? OpenDatabaseGrbit.ReadOnly : OpenDatabaseGrbit.None), "JetOpenDatabase");
        }

        class EUt
        {
            internal static void Check(JET_wrn wrn, String message)
            {
                if (wrn == JET_wrn.Success) return;
                throw new ApplicationException(message + " " + wrn);
            }
        }

        #region IDisposable メンバ

        public void Dispose()
        {
            if (dbid != JET_DBID.Nil)
            {
                Api.JetCloseDatabase(sesid, dbid, CloseDatabaseGrbit.None);
                dbid = JET_DBID.Nil;
            }
            if (sesid != JET_SESID.Nil)
            {
                Api.JetEndSession(sesid, EndSessionGrbit.None);
                sesid = JET_SESID.Nil;
            }
            if (instance != JET_INSTANCE.Nil)
            {
                Api.JetTerm(instance);
                instance = JET_INSTANCE.Nil;
            }
            if (Disposed != null)
            {
                Disposed(this, new EventArgs());
            }
        }

        #endregion

        public IEnumerable<T> ReadEntireTable<T>(string tableName)
        {
            using (Table table = new Table(sesid, dbid, tableName, OpenTableGrbit.ReadOnly))
            {
                var type = typeof(T);
                var props = type.GetProperties();
                var columns = Api.GetTableColumns(sesid, table.JetTableid)
                    .ToArray()
                    .ToDictionary(it => it.Name, it => it);
                if (Api.TryMoveFirst(sesid, table.JetTableid))
                {
                    while (true)
                    {
                        T instance = (T)type.InvokeMember(
                            null,
                            BindingFlags.CreateInstance,
                            null,
                            null,
                            new object[0]
                        );

                        foreach (var prop in props)
                        {
                            var att = prop.GetCustomAttributes(false)
                                .OfType<EsColumnAttribute>()
                                .FirstOrDefault();

                            if (att != null)
                            {
                                if (columns.TryGetValue(att.ColumnName, out ColumnInfo columnInfo))
                                {
                                    var data = Api.RetrieveColumn(sesid, table.JetTableid, columnInfo.Columnid);
                                    object value = null;

                                    if (prop.PropertyType == typeof(string))
                                    {
                                        if (data != null)
                                        {
                                            value = (att.Unicode ? Encoding.Unicode : Encoding.Default)
                                                .GetString(data)
                                                .Trim('\0');
                                        }
                                    }
                                    else if (prop.PropertyType == typeof(int))
                                    {
                                        if (data == null)
                                        {
                                            // null
                                        }
                                        else if (data.Length == 1)
                                        {
                                            value = data[0];
                                        }
                                        else if (data.Length == 2)
                                        {
                                            value = BitConverter.ToUInt16(data, 0);
                                        }
                                        else if (data.Length >= 4)
                                        {
                                            value = BitConverter.ToInt32(data, 0);
                                        }
                                    }
                                    else if (prop.PropertyType == typeof(long))
                                    {
                                        if (data == null)
                                        {
                                            // null
                                        }
                                        else if (data.Length >= 8)
                                        {
                                            value = BitConverter.ToInt64(data, 0);
                                        }
                                    }
                                    else if (prop.PropertyType == typeof(byte[]))
                                    {
                                        if (data == null)
                                        {
                                            // null
                                        }
                                        else if (data.Length >= 8)
                                        {
                                            value = data;
                                        }
                                    }
                                    else
                                    {
                                        throw new NotSupportedException(
                                            columnInfo.Coltyp + " to " + prop.PropertyType.FullName
                                        );
                                    }

                                    if (value != null)
                                    {
                                        prop.SetValue(instance, value, new object[0]);
                                    }
                                }
                            }
                        }

                        yield return instance;

                        if (!Api.TryMoveNext(sesid, table.JetTableid))
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
