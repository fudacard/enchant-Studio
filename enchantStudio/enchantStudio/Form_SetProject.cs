using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace enchantStudio
{
    public partial class Form_SetProject : Form
    {
        ESProject project;

        public Form_SetProject()
        {
            InitializeComponent();
        }

        public void ShowDialog(ESProject prj)
        {
            project = prj;
            ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Text = "プロジェクト作成中...";
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                project.DefaultFiles.Add("Data/enchant.js/AppendData/plugin/" + checkedListBox1.CheckedItems[i]);
            }

            for (int i = 0; i < checkedListBox2.CheckedItems.Count; i++)
            {
                project.DefaultFiles.Add("Data/enchant.js/AppendData/default resource/" + checkedListBox2.CheckedItems[i]);
            }
            /*
            for (int i = 0; i < project.DefaultFiles.Count; i++)
            {
                Console.WriteLine("追加するファイル:" + project.DefaultFiles[i]);
            }
            */
            SetProject();
            this.Close();
            this.Text = "プロジェクト設定";
        }

        private void SetProject()
        {
            string prjfp = project.ProjectPath + "\\" + project.ProjectName;
            Directory.CreateDirectory(prjfp);
            
            string copypath;

            foreach (string orgpath in project.DefaultFiles)
            {
                copypath = prjfp + "\\" + Path.GetFileName(orgpath);
                File.Copy(orgpath, copypath, true);
            }

            DirectoryInfo dir = new DirectoryInfo("Data/Template");
            foreach (FileInfo file in dir.GetFiles())
            {
                File.Copy(file.FullName, prjfp + "\\"+Path.GetFileName(file.Name),true);
            }

            File.Copy("Data/enchant.js/enchant.js", prjfp + "\\enchant.js",true);
            //File.Copy("Data/project.txt", prjfp + "\\" + project.ProjectName + ".esprj",true);
            //プロジェクト設定
            ESProjectXML px = new ESProjectXML();
            px.ProjectName = project.ProjectName;
            new Serializer().SaveObject(typeof(ESProjectXML), px, prjfp + "\\" + project.ProjectName + ".esprj");

            project.IsCreated = true;
        }

        private void Form_SetProject_Load(object sender, EventArgs e)
        {
            //ファイル名のみ表示のこと
            string[] pfiles = Directory.GetFiles("Data/enchant.js/AppendData/plugin", "*");
            for (int i = 0; i < pfiles.Length; i++)
            {
                pfiles[i] = Path.GetFileName(pfiles[i]);
            }
            checkedListBox1.Items.AddRange(pfiles);

            string[] rfiles = Directory.GetFiles("Data/enchant.js/AppendData/default resource", "*");
            for (int i = 0; i < rfiles.Length; i++)
            {
                rfiles[i] = Path.GetFileName(rfiles[i]);
            }
            checkedListBox2.Items.AddRange(rfiles);
        }
    }
}
