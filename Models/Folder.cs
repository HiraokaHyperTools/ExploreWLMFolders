using ExploreWLMFolders.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExploreWLMFolders.Models
{
    public class Folder
    {
        [EsColumn("FLDCOL_ACCOUNTID")] public string FLDCOL_ACCOUNTID { get; set; }
        [EsColumn("FLDCOL_CLIENTHIGH")] public int FLDCOL_CLIENTHIGH { get; set; }
        [EsColumn("FLDCOL_CLIENTLOW")] public int FLDCOL_CLIENTLOW { get; set; }
        [EsColumn("FLDCOL_COLOR")] public int FLDCOL_COLOR { get; set; }
        [EsColumn("FLDCOL_DESCRIPTION")] public string FLDCOL_DESCRIPTION { get; set; }
        [EsColumn("FLDCOL_FILE")] public string FLDCOL_FILE { get; set; }
        [EsColumn("FLDCOL_FLAGS")] public int FLDCOL_FLAGS { get; set; }
        [EsColumn("FLDCOL_HIERARCHY")] public int FLDCOL_HIERARCHY { get; set; }
        [EsColumn("FLDCOL_ID")] public long FLDCOL_ID { get; set; }
        [EsColumn("FLDCOL_IGNORED")] public int FLDCOL_IGNORED { get; set; }
        [EsColumn("FLDCOL_LISTSTAMP")] public int FLDCOL_LISTSTAMP { get; set; }
        [EsColumn("FLDCOL_MANAGED")] public int FLDCOL_MANAGED { get; set; }
        [EsColumn("FLDCOL_MESSAGES")] public int FLDCOL_MESSAGES { get; set; }
        [EsColumn("FLDCOL_NAME")] public string FLDCOL_NAME { get; set; }
        [EsColumn("FLDCOL_NAMEHASH")] public int FLDCOL_NAMEHASH { get; set; }
        [EsColumn("FLDCOL_NAMEW", Unicode = true)] public string FLDCOL_NAMEW { get; set; }
        [EsColumn("FLDCOL_NOTDOWNLOADED")] public int FLDCOL_NOTDOWNLOADED { get; set; }
        [EsColumn("FLDCOL_PARENT")] public long FLDCOL_PARENT { get; set; }
        [EsColumn("FLDCOL_PATH", Unicode = true)] public string FLDCOL_PATH { get; set; }
        [EsColumn("FLDCOL_READ")] public byte[] FLDCOL_READ { get; set; }
        [EsColumn("FLDCOL_REQUESTED")] public byte[] FLDCOL_REQUESTED { get; set; }
        [EsColumn("FLDCOL_SERVERCOUNT")] public int FLDCOL_SERVERCOUNT { get; set; }
        [EsColumn("FLDCOL_SERVERHIGH")] public int FLDCOL_SERVERHIGH { get; set; }
        [EsColumn("FLDCOL_SERVERID")] public string FLDCOL_SERVERID { get; set; }
        [EsColumn("FLDCOL_SERVERLOW")] public int FLDCOL_SERVERLOW { get; set; }
        [EsColumn("FLDCOL_SORTCOLUMN")] public int FLDCOL_SORTCOLUMN { get; set; }
        [EsColumn("FLDCOL_SPECIAL")] public int FLDCOL_SPECIAL { get; set; }
        [EsColumn("FLDCOL_STATUSMSGDELTA")] public int FLDCOL_STATUSMSGDELTA { get; set; }
        [EsColumn("FLDCOL_STATUSUNREADDELTA")] public int FLDCOL_STATUSUNREADDELTA { get; set; }
        [EsColumn("FLDCOL_SUBSCRIBED")] public int FLDCOL_SUBSCRIBED { get; set; }
        [EsColumn("FLDCOL_THREADUNREAD")] public int FLDCOL_THREADUNREAD { get; set; }
        [EsColumn("FLDCOL_TYPE")] public int FLDCOL_TYPE { get; set; }
        [EsColumn("FLDCOL_UNREAD")] public int FLDCOL_UNREAD { get; set; }
        [EsColumn("FLDCOL_URLCOMPONENT")] public string FLDCOL_URLCOMPONENT { get; set; }
        [EsColumn("FLDCOL_VIEWUNREAD")] public int FLDCOL_VIEWUNREAD { get; set; }
        [EsColumn("FLDCOL_WATCHED")] public int FLDCOL_WATCHED { get; set; }
        [EsColumn("FLDCOL_WATCHEDHIGH")] public int FLDCOL_WATCHEDHIGH { get; set; }
        [EsColumn("FLDCOL_WATCHEDUNREAD")] public int FLDCOL_WATCHEDUNREAD { get; set; }
    }
}
