using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ExploreWLMFolders.Models
{
    public class FolderInfo
    {
        public string Path { get; internal set; }
        [Category("Important")]
        public int MessageCount { get; internal set; }
        public long Id { get; internal set; }
        public string AccountId { get; internal set; }
        public int Flags { get; internal set; }
        public int Type { get; internal set; }
    }
}
