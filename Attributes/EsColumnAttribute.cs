using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExploreWLMFolders.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EsColumnAttribute : Attribute
    {
        public EsColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }

        public string ColumnName { get; }
        public bool Unicode { get; set; }
    }
}
