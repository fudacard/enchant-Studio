/*
 * enchant Studio
 * v1.1.1
 * ｋｂ１０うｙ
 * 
 * v1.0.2からの更新点
 * ・フォルダツリーの右クリックメニューを追加した。
 * 
 * ・それにともなって、画像を右クリした時に、
 * 　チェックツールでチェックするようにした。
 * 
 */

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Sgry.Azuki;
using Sgry.Azuki.Highlighter;
using Sgry.Azuki.WinForms;
using System.Text;


namespace enchantStudio
{
    public partial class Form1 : Form
    {

        ESConfig config;
        ESProject nowProject;
        ESProjectXML nowxml;


        int seltab;
        /// <summary>
        /// タブで開いているファイルのフルパスを格納します。
        /// キーはTabPageのHashCodeを指定してください。
        /// </summary>
        Dictionary<int, string> FullFileName = new Dictionary<int, string>();

        /// <summary>
        /// タブで開いているファイルが編集されているかを格納します。
        /// </summary>
        Dictionary<int, bool> isEdited = new Dictionary<int, bool>();

        /// <summary>
        /// 拡張子に対応したKeywordHighlighterを格納します。
        /// "$DEFAULT$"でデフォルトのものを取得します。
        /// </summary>
        Dictionary<string, KeywordHighlighter> Highlighters = new Dictionary<string, KeywordHighlighter>();

        /// <summary>
        /// 拡張子に対応したKeywordHighlighterを格納します。
        /// "$DEFAULT$"でデフォルトのものを取得します。
        /// </summary>
        Dictionary<string, ColorScheme> ColorSchemes = new Dictionary<string, ColorScheme>();

        /// <summary>
        /// 背景色を格納します。
        /// "$DEFAULT$"でデフォルトのものを取得します。
        /// </summary>
        Dictionary<string, Color> BackColors = new Dictionary<string, Color>();

        /// <summary>
        /// 格納したNodeがディレクトリ用のものかをチェックします。
        /// キーにはNodeのHashを入れてください。
        /// </summary>
        Dictionary<int, bool> IsDirectory = new Dictionary<int, bool>();

        /// <summary>
        /// IsDirectoryのスニペット版です。
        /// </summary>
        Dictionary<int, bool> S_IsDirectory = new Dictionary<int, bool>();

        //Form
        Form_Setting SettingWindow;
        Form_Ref ReferenceWindow;
        Form_SelEnc SelEncWindow;
        Form_Find FindWindow;
        Form_NewProject NewPrjWindow;
        Form_About AboutWindow;
        Form_SetProject PrjSettingWindow;
        Form_Replace ReplaceWindow;
        
        Encoding SelectEncoding;
        Font EditorFont;
        
        /// <summary>
        /// 右クリしたノード
        /// </summary>
        TreeNode rcnode;

        string viewpath = "C:\\";
        string nowfp = "";

        int next;       //次の検索開始
        int prevbegin;  //前回の検索結果範囲の開始位置

        Serializer seri;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            config = new ESConfig();
            nowProject = new ESProject();
            nowxml = new ESProjectXML();


            SelectEncoding = Encoding.GetEncoding(932);
            seltab = -1;

            SettingWindow = new Form_Setting();
            ReferenceWindow = new Form_Ref();
            FindWindow = new Form_Find();
            SelEncWindow = new Form_SelEnc();
            NewPrjWindow = new Form_NewProject();
            AboutWindow = new Form_About();
            PrjSettingWindow = new Form_SetProject();
            ReplaceWindow = new Form_Replace();

            //開始時に開かない
            //viewpath = Path.GetDirectoryName(Application.ExecutablePath);
            //nowfp = viewpath;
            //ViewFolderTree(viewpath, treeView1);
            //toolStripTextBox2.Text = viewpath;
            SetSnippetsTree();
            setKeywordColor();
            seri = new Serializer();


            config=LoadConfig();


            EditorFont = new Font(config.FontName, config.FontSize);
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

            /*
            XmlSerializer serializer = new XmlSerializer(typeof(ESConfig));
            XmlReader reader;
            if (!File.Exists("Data.cofig.xml"))
            {
                FileStream stream = new FileStream("Data/config.xml", FileMode.Create);
                serializer.Serialize(stream, config);
                stream.Close();
            }
            reader = XmlReader.Create("Data/config.xml");
            if (serializer.CanDeserialize(reader))
            {
                serializer.Deserialize(reader);
            }
            */


            //setAzukiExt(azukiControl1);

        }

        void Application_ApplicationExit(object sender, EventArgs e)
        {
            
            SaveConfig(config);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


        /*
         * ここから下はイベントハンドラ
         * 
         */

        private void ファイルToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TabPage addtab = new TabPage();
            AzukiControl addazuki = new AzukiControl();

            addtab.Padding = new System.Windows.Forms.Padding(3);
            addtab.Text = "Untitled";


            setAzukiExt(addazuki, Highlighters["$DEFAULT$"], ColorSchemes["$DEFAULT$"], BackColors["$DEFAULT$"]);
            addtab.Controls.Add(addazuki);
            tabControl1.Controls.Add(addtab);

        }

        private void 設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingWindow.ShowDialog(config);
        }

        private void 切り取りToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiCut((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void コピーToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiCopy((AzukiControl)tabControl1.SelectedTab.Controls[0]);

        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            //FormClosingで引き返せるなら二度手間
            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                if (((AzukiControl)tabControl1.TabPages[i].Controls[0]).Document.IsDirty)
                {
                    switch (MessageBox.Show(
                        "保存されていないファイルがあります。保存しますか？\n(Untitledは無視されます)",
                        "未保存のファイル",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk))
                    {
                        case DialogResult.Yes:
                            SaveFileAll(tabControl1);
                            break;
                        case DialogResult.No:
                            break;
                        default:
                            return;
                    }
                    break;
                }
            }
            */
            Application.Exit();
        }

        private void すべてを選択ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiSelectAll((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void 貼り付けToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiPaste((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void 切り取りToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiCut((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void コピーToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiCopy((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void 貼り付けToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiPaste((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiSelectAll((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void リファレンスウィンドウToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReferenceWindow.Show();
        }

        private void ファイルToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string fname;
            if ((fname = GetOpenFileName()) != null)
            {
                CreateAzukiTabWithFile(fname);
            }
        }

        private void tabControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (GetOveredTabIndex(tabControl1, e.Location) != -1)
            {
                contextMenuStrip2.Show(tabControl1, e.Location);
                seltab = GetOveredTabIndex(tabControl1, e.Location);
            }
        }

        private void タブを閉じるToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (seltab != -1)
            {
                if (((AzukiControl)tabControl1.SelectedTab.Controls[0]).Document.IsDirty == true)
                {
                    switch (
                    MessageBox.Show(
                        String.Format("{0}は変更されています。保存しますか？", tabControl1.SelectedTab.Text),
                        "変更されたファイル",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Asterisk))
                    {

                        case DialogResult.Cancel:
                            return;

                        case DialogResult.Yes:
                            if (FullFileName.ContainsKey(tabControl1.SelectedTab.GetHashCode()))
                            {
                                SaveFile(FullFileName[tabControl1.SelectedTab.GetHashCode()], ((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Text);
                            }
                            else
                            {
                                SaveFileAs(((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Text, tabControl1.SelectedTab);
                            }
                            break;
                        default:
                            break;
                    }
                }
                ((AzukiControl)tabControl1.TabPages[seltab].Controls[0]).Dispose();
                //tabControl1.TabPages[seltab].Dispose();
                tabControl1.TabPages.RemoveAt(seltab);
                seltab = -1;
            }
        }

        private void shiftJISToolStripMenuItem_CheckChanged(object sender, EventArgs e)
        {
            SelectEncoding = Encoding.GetEncoding(932);
            shiftJISToolStripMenuItem.Checked = true;
            uTF8ToolStripMenuItem.Checked = false;
            uTF16ToolStripMenuItem.Checked = false;
            ユーザー指定ToolStripMenuItem.Checked = false;
        }

        private void uTF8ToolStripMenuItem_CheckChanged(object sender, EventArgs e)
        {
            SelectEncoding = Encoding.UTF8;
            shiftJISToolStripMenuItem.Checked = false;
            uTF8ToolStripMenuItem.Checked = true;
            uTF16ToolStripMenuItem.Checked = false;
            ユーザー指定ToolStripMenuItem.Checked = false;
        }

        private void uTF16ToolStripMenuItem_CheckChanged(object sender, EventArgs e)
        {
            SelectEncoding = Encoding.Unicode;
            shiftJISToolStripMenuItem.Checked = false;
            uTF8ToolStripMenuItem.Checked = false;
            uTF16ToolStripMenuItem.Checked = true;
            ユーザー指定ToolStripMenuItem.Checked = false;
        }

        private void ユーザー指定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shiftJISToolStripMenuItem.Checked = false;
            uTF16ToolStripMenuItem.Checked = false;
            uTF8ToolStripMenuItem.Checked = false;
            ユーザー指定ToolStripMenuItem.Checked = true;

            SelEncWindow.ShowDialog(SelectEncoding);
        }

        private void 検索ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindWindow.ShowWithTabControl(tabControl1);
        }

        private void プロジェクトToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            NewPrjWindow.ShowDialog(PrjSettingWindow, nowProject, config);
            if (nowProject.IsCreated)
            {
                toolStripTextBox2.Text = nowProject.ProjectPath + "\\" + nowProject.ProjectName;
                nowfp = nowProject.ProjectPath + "\\" + nowProject.ProjectName;
                ViewFolderTree(nowProject.ProjectPath + "\\" + nowProject.ProjectName, treeView1);
                this.Text = nowProject.ProjectName + " - enchant Studio";

                ESProjectXML exm = (ESProjectXML)seri.LoadObject(typeof(ESProjectXML),nowfp+"\\"+nowProject.ProjectName+".esprj");
                nowProject.Execute = exm.ExecuteHTML;

            }
        }

        private void enchantStudioについてToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutWindow.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ファイルToolStripMenuItem2_Click(sender, e);
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            ファイルToolStripMenuItem1_Click(sender, e);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount <= 0) return;
            if (FullFileName.ContainsKey(tabControl1.SelectedTab.GetHashCode()))
            {
                SaveFile(FullFileName[tabControl1.SelectedTab.GetHashCode()], ((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Text);
                ((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Document.IsDirty = false;
                isEdited[tabControl1.SelectedTab.GetHashCode()] = false;
            }
            else
            {
                SaveFileAs(((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Text, tabControl1.SelectedTab);
                ((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Document.IsDirty = false;
            }
        }

        private void 名前を付けて保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1)
            {
                SaveFileAs(((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Text, tabControl1.SelectedTab);
                ((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Document.IsDirty = false;
            }
        }

        private void すべて保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1)
            {
                SaveFileAll(tabControl1);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount <= 0) return;
            if (FullFileName.ContainsKey(tabControl1.SelectedTab.GetHashCode()))
            {
                SaveFile(FullFileName[tabControl1.SelectedTab.GetHashCode()], ((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Text);
                ((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Document.IsDirty = false;
                isEdited[tabControl1.SelectedTab.GetHashCode()] = false;
            }
            else
            {
                SaveFileAs(((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Text, tabControl1.SelectedTab);
                ((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Document.IsDirty = false;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1)
            {
                SaveFileAs(((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Text, tabControl1.SelectedTab);
                ((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Document.IsDirty = false;
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1)
            {
                SaveFileAll(tabControl1);
            }
        }


        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            ファイルToolStripMenuItem2_Click(sender, e);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiCut((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiCopy((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiPaste((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            FindWindow.ShowWithTabControl(tabControl1);
        }

        private void 元に戻すToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiUndo((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void やり直しToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiRedo((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            プロジェクトToolStripMenuItem1_Click(sender, e);
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiUndo((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount >= 1) AzukiRedo((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void 内容を保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount <= 0) return;
            if (FullFileName.ContainsKey(tabControl1.SelectedTab.GetHashCode()))
            {
                SaveFile(FullFileName[tabControl1.SelectedTab.GetHashCode()], ((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Text);
                ((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Document.IsDirty = false;
                isEdited[tabControl1.SelectedTab.GetHashCode()] = false;
            }
            else
            {
                SaveFileAs(((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Text, tabControl1.SelectedTab);
                ((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Document.IsDirty = false;
            }
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(toolStripTextBox2.Text))
            {
                ViewFolderTree(viewpath, treeView1);
                viewpath = toolStripTextBox2.Text;

            }
            else
            {
                MessageBox.Show("指定されたフォルダは存在しません。", "警告");
            }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //Console.WriteLine(String.Format("Expanding Node is NodeHash({0})", e.Node.GetHashCode()));
            TreeNode par = e.Node;

            par.Nodes.Clear();
            par.ImageIndex = 5;
            par.SelectedImageIndex = 5;
            DirectoryInfo dirs = new DirectoryInfo(viewpath + "\\" + par.FullPath);
            foreach (DirectoryInfo dir in dirs.GetDirectories())
            {
                TreeNode node_fl = new TreeNode(dir.Name, 4, 4);
                par.Nodes.Add(node_fl);
                node_fl.Nodes.Add("---");
                IsDirectory[node_fl.GetHashCode()] = true;
            }
            foreach (FileInfo file in dirs.GetFiles())
            {
                TreeNode node_fl = new TreeNode(file.Name, GetFileIconIndex(file.Name), GetFileIconIndex(file.Name));
                par.Nodes.Add(node_fl);
                IsDirectory[node_fl.GetHashCode()] = false ;
            }
        }

        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.ImageIndex = 4;
            e.Node.SelectedImageIndex = 4;
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            ViewFolderTree(viewpath, treeView1);
        }

        private void プレビューウィンドウToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nowProject.IsCreated)
            {
                System.Diagnostics.Process.Start(nowfp+"\\"+nowProject.Execute);
            }
        }

        private void プロジェクトToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string path = Path.GetDirectoryName(openFileDialog2.FileName);
                nowfp = path;
                ViewFolderTree(path, treeView1);

                ESProjectXML exm = (ESProjectXML)seri.LoadObject(typeof(ESProjectXML), openFileDialog2.FileName);
                nowProject.ProjectName = exm.ProjectName;
                nowProject.Execute = exm.ExecuteHTML;
                this.Text = exm.ProjectName + " - enchant Studio";
                
                nowProject.IsCreated = true;
            }

        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //Console.WriteLine(String.Format("Opening Node is NodeHash({0})", e.Node.GetHashCode()));
            if (e.Node.Nodes.Count != 0) return;
            if (IsDirectory[e.Node.GetHashCode()]) return;
            switch (Path.GetExtension(e.Node.FullPath).ToLower())
            {
                case ".txt":
                case ".js":
                case ".htm":
                case ".html":
                    CreateAzukiTabWithFile(nowfp + "\\" + e.Node.FullPath);
                    break;
                default:
                    System.Diagnostics.Process.Start(nowfp + "\\" + e.Node.FullPath);
                    break;
            }
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            if (nowProject.IsCreated)
            {
                System.Diagnostics.Process.Start(nowfp + "\\" + nowProject.Execute);
            }
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (tabControl1.TabCount < 1) return;
            if (e.KeyData != Keys.Enter) return;
            AzukiControl nowaz= (AzukiControl)tabControl1.SelectedTab.Controls[0];
            SearchResult re = nowaz.Document.FindNext(toolStripTextBox1.Text, next);
            if (re != null)
            {
                nowaz.Document.SetSelection(re.Begin, re.End);
                next = re.End;
                prevbegin = re.Begin;
            }
            else
            {
                nowaz.Document.SetSelection(nowaz.CaretIndex, nowaz.CaretIndex);
                next = nowaz.CaretIndex;
                MessageBox.Show("終端まで検索しました", "検索結果", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            リファレンスウィンドウToolStripMenuItem_Click(sender, e);
        }

        private void treeView2_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode par = e.Node;

            par.Nodes.Clear();
            DirectoryInfo dirs = new DirectoryInfo("Data/Snippet/" + par.FullPath);
            foreach (DirectoryInfo dir in dirs.GetDirectories())
            {
                TreeNode node_fl = new TreeNode(dir.Name);
                par.Nodes.Add(node_fl);
                node_fl.Nodes.Add("---");
                S_IsDirectory[node_fl.GetHashCode()] = true;
            }
            foreach (FileInfo file in dirs.GetFiles("*.txt"))
            {
                TreeNode node_fl = new TreeNode(Path.GetFileNameWithoutExtension(file.Name));
                par.Nodes.Add(node_fl);
                S_IsDirectory[node_fl.GetHashCode()] = false;
            }
        }

        private void treeView2_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count != 0) return;
            if (S_IsDirectory[e.Node.GetHashCode()]) return;
            if (tabControl1.TabCount < 1) return;

            //なんて言うか、もっと効率的にしろっつーの
            //2012/10/21現在、(AzukiControl)tabControl1.SelectedTab.Controls[0]は21回使われている
            //選ばれているタブのAzukiControlですぜ？
            ((AzukiControl)tabControl1.SelectedTab.Controls[0]).Document.Replace(
                File.ReadAllText("Data/Snippet/" + e.Node.FullPath + ".txt", Encoding.GetEncoding(932)),
                ((AzukiControl)tabControl1.SelectedTab.Controls[0]).CaretIndex,
                ((AzukiControl)tabControl1.SelectedTab.Controls[0]).CaretIndex
                );
        }

        //ファイルの未保存チェックのみ
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                if (((AzukiControl)tabControl1.TabPages[i].Controls[0]).Document.IsDirty && FullFileName.ContainsKey(tabControl1.SelectedTab.GetHashCode()))
                {
                    switch (MessageBox.Show(
                        "保存されていないファイルがあります。保存しますか？\n(Untitledは無視されます)",
                        "未保存のファイル",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk))
                    {
                        case DialogResult.Yes:
                            SaveFileAll(tabControl1);
                            break;
                        case DialogResult.No:
                            break;
                        default:
                            e.Cancel = true;
                            return;
                    }
                    break;
                }
            }
        }

        private void プロジェクトToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            プロジェクトToolStripMenuItem2_Click(sender, e);
        }

        private void 置き換えToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount < 1) return;
            ReplaceWindow.ShowDialog((AzukiControl)tabControl1.SelectedTab.Controls[0]);
        }

        private void 開くToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (rcnode.Nodes.Count != 0) return;
            if (IsDirectory[rcnode.GetHashCode()]) return;
            switch (Path.GetExtension(rcnode.FullPath).ToLower())
            {
                case ".txt":
                case ".js":
                case ".htm":
                case ".html":
                    CreateAzukiTabWithFile(viewpath + "\\" + rcnode.FullPath);
                    break;
                default:
                    System.Diagnostics.Process.Start(viewpath + "\\" + rcnode.FullPath);
                    break;
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (e.Node.Nodes.Count != 0)
            {
                //Console.WriteLine("Node RightClicked");
                treeView1.SelectedNode = e.Node;
                rcnode = e.Node;
                contextMenuStrip4.Show(treeView1, e.Location);
            }
            else
            {
                //ファイルだった時
                ShowBestContextMenu(Path.GetExtension(e.Node.FullPath),e);
            }
        }

        private void 開く閉じるToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rcnode.IsExpanded)
            {
                rcnode.Collapse();
            }
            else
            {
                rcnode.Expand();
            }
        }

        private void 削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("削除してよろしいですか？", "削除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                File.Delete(viewpath + "\\" + rcnode.FullPath);
                ViewFolderTree(viewpath, treeView1);
            }
        }

        private void 名前の変更ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 開くToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            開くToolStripMenuItem1_Click(sender, e);
        }

        private void 名前の変更ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            名前の変更ToolStripMenuItem_Click(sender, e);
        }

        private void 削除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            削除ToolStripMenuItem_Click(sender, e);
        }

        private void スプライト画像チェックツールToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Data\\isview.exe", viewpath + rcnode.FullPath);
        }


    }
}
