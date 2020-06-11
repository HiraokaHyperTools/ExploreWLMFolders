using ExploreWLMFolders.Models;
using ExploreWLMFolders.Utils;
using kenjiuno.AutoHourglass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExploreWLMFolders
{
    public partial class Form1 : Form
    {
        private readonly ES es = new ES();
        private Folder[] folders;

        public Form1()
        {
            InitializeComponent();
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            ofdEdb.FileName = WLMRegUtil.GetMailMSMessageStore();
            if (ofdEdb.ShowDialog(this) == DialogResult.OK)
            {
                using (new AH())
                {
                    es.Open(ofdEdb.FileName);
                    folders = es.ReadEntireTable<Folder>("Folders").ToArray();
                }

                folderTree.Nodes.Clear();
                Build(folderTree.Nodes, 0, folders, Path.GetDirectoryName(ofdEdb.FileName));
            }
        }

        private static void Build(TreeNodeCollection nodes, long folderId, Folder[] folders, string rootDir)
        {
            foreach (var subF in folders.Where(it => it.FLDCOL_PARENT == folderId))
            {
                var info = new FolderInfo
                {
                    Path = (subF.FLDCOL_PATH != null)
                        ? Path.GetFullPath(Path.Combine(rootDir, subF.FLDCOL_PATH))
                        : null,
                    MessageCount = subF.FLDCOL_MESSAGES,
                    AccountId = subF.FLDCOL_ACCOUNTID,
                    Id = subF.FLDCOL_ID,
                    Flags = subF.FLDCOL_FLAGS,
                    Type = subF.FLDCOL_TYPE,
                };
                var countKey = (info.MessageCount != 0) ? "1" : "0";
                var imageKey = (string.IsNullOrEmpty(info.Path) || !Directory.Exists(info.Path)) ? "X"
                    : (3 != (subF.FLDCOL_TYPE)) ? $"H{countKey}"
                    : countKey
                    ;
                var subNode = nodes.Add(subF.FLDCOL_NAMEW, subF.FLDCOL_NAMEW, imageKey, imageKey);
                subNode.Tag = info;

                Build(subNode.Nodes, subF.FLDCOL_ID, folders, rootDir);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            es.Dispose();
        }

        private void folderTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = folderTree.SelectedNode;
            if (node != null)
            {
                folderInfoGrid.SelectedObject = node.Tag;
            }
        }

        private void copyPathBtn_Click(object sender, EventArgs e)
        {
            var info = folderInfoGrid.SelectedObject as FolderInfo;
            if (info != null)
            {
                Clipboard.SetText(info.Path ?? "");
                MessageBox.Show("Copied.");
            }
        }
    }
}
