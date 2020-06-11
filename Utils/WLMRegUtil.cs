using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExploreWLMFolders.Utils
{
    static class WLMRegUtil
    {
        internal static string GetMailMSMessageStore()
        {
            var storeRoot = Environment.ExpandEnvironmentVariables(
                "" + Registry.GetValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows Live Mail",
                    "Store Root",
                    null
                )
            );
            if (storeRoot.Length >= 1 && Directory.Exists(storeRoot))
            {
                var mailMSMessageStore = Path.Combine(storeRoot, "Mail.MSMessageStore");
                if (File.Exists(mailMSMessageStore))
                {
                    return mailMSMessageStore;
                }
            }
            return null;
        }
    }
}
