using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Sgry.Azuki;
using Sgry.Azuki.Highlighter;
using Sgry.Azuki.WinForms;
using System.Text;

/*
 *こっちのファイルは
 *イベントハンドラ以外のものを記述
 * 名前こそAzukiだが
 * そんなのカンケイネェ
 */

namespace enchantStudio
{
    public partial class Form1
    {
        /*
         azuki用のいろいろな関数
         
         */


        /// <summary>
        /// 指定のファイルを開いたAzukiタブを生成します。
        /// </summary>
        /// <param name="filename">ファイル名</param>
        private void CreateAzukiTabWithFile(string filename)
        {
            TabPage addtab = new TabPage();
            AzukiControl addazuki = new AzukiControl();
            KeywordHighlighter kh;
            ColorScheme cs;
            Color bc;

            addtab.Padding = new System.Windows.Forms.Padding(3);
            addtab.Text = Path.GetFileName(filename);
            //ハッシュをキーにすることで区別
            FullFileName[addtab.GetHashCode()] = filename;
            //Console.WriteLine(String.Format("id{0}に{1}を設定",addtab.GetHashCode(),filename));
            string ext = Path.GetExtension(filename).ToLower().Substring(1);
            if (Highlighters.ContainsKey(ext))
            {
                kh = Highlighters[ext];
            }
            else
            {
                kh = Highlighters["$DEFAULT$"];
            }

            if (ColorSchemes.ContainsKey(ext))
            {
                cs = ColorSchemes[ext];
            }
            else
            {
                cs = ColorSchemes["$DEFAULT$"];
            }

            if (BackColors.ContainsKey(ext))
            {
                bc = BackColors[ext];
            }
            else
            {
                bc = BackColors["$DEFAULT$"];
            }


            setAzukiExt(addazuki,kh,cs,bc);

            //addazuki.KeyDown += new KeyEventHandler(addazuki_KeyDown);
            addazuki.OverwriteModeChanged += new EventHandler(addazuki_OverwriteModeChanged);
            addazuki.Text = File.ReadAllText(filename, Encoding.Default);
            addazuki.Document.IsDirty = false;

            addtab.Controls.Add(addazuki);
            tabControl1.TabPages.Add(addtab);
        }

        void addazuki_OverwriteModeChanged(object sender, EventArgs e)
        {
        }

        ///テキスト編集字のハンドラ

        /// <summary>
        /// あずきコントロールにいろいろ設定します。
        /// 背景色を設定できます。
        /// </summary>
        /// <param name="az">AzukiControl</param>
        /// <param name="kh">設定するKeywordHighlighter</param>
        /// <param name="cs">設定するColorScheme</param>
        /// <param name="cl">設定する背景色</param>
        private void setAzukiExt(AzukiControl az, KeywordHighlighter kh, ColorScheme cs,Color cl)
        {
            az.Dock = DockStyle.Fill;
            az.Location = new Point(3, 3);
            az.Highlighter = kh;
            az.ColorScheme = cs;

            az.BackColor = cl;
            az.Font = EditorFont;
            az.AutoIndentHook = AutoIndentHooks.CHook;
            az.ContextMenuStrip = contextMenuStrip1;
        }



        /// <summary>
        /// 一般的な切り取りの動作をさせます。
        /// </summary>
        /// <param name="az">対象のAzukiControl</param>
        private void AzukiCut(AzukiControl az)
        {
            int b, end;
            string selectstr;

            az.Document.GetSelection(out b, out end);
            selectstr = az.Text.Substring(b, end - b);
            az.Document.Replace("", b, end);

            if ((selectstr != null)&&(selectstr!="")) Clipboard.SetText(selectstr);
        }

        /// <summary>
        /// 一般的なコピーの動作をさせます。
        /// </summary>
        /// <param name="az">対象のAzukiControl</param>
        private void AzukiCopy(AzukiControl az)
        {
            int b, end;
            string selectstr;

            az.Document.GetSelection(out b, out end);
            selectstr = az.Text.Substring(b, end - b);

            if ((selectstr!=null)&&(selectstr!="")) Clipboard.SetText(selectstr);
        }

        /// <summary>
        /// クリップボードの内容をAzukiにペーストします。
        /// </summary>
        /// <param name="az">対象のAzukiControl</param>
        private void AzukiPaste(AzukiControl az)
        {
            string text = Clipboard.GetText();
            az.Document.Replace(text, az.CaretIndex, az.CaretIndex);
        }

        /// <summary>
        /// 指定した文字列をAzukiにペーストします。
        /// </summary>
        /// <param name="az">対象のAzukiControl</param>
        private void AzukiPaste(AzukiControl az, string text)
        {
            az.Document.Replace(text, az.CaretIndex, az.CaretIndex);
        }

        /// <summary>
        /// 一般的なすべてを選択の動作をさせます。
        /// </summary>
        /// <param name="az"></param>
        private void AzukiSelectAll(AzukiControl az)
        {
            az.Document.SetSelection(0, az.Document.Length);
        }

        private void AzukiUndo(AzukiControl az)
        {
            az.Document.Undo();
        }

        private void AzukiRedo(AzukiControl az)
        {
            az.Document.Redo();
        }

        /// <summary>
        /// ファイル選択ダイアログを開いて、
        /// ファイル名を返します。
        /// 無選択時はnullです。
        /// </summary>
        /// <returns>選択されたファイル名</returns>
        private string GetOpenFileName()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            return null;
        }

        /// <summary>
        /// マウスで触れているタブのIndexを返します。
        /// </summary>
        /// <param name="tc">調べたいTabControl</param>
        /// <param name="e">渡されたe</param>
        /// <returns>インデックス(-1は無し)
        /// </returns>
        private int GetOveredTabIndex(TabControl tc, Point location)
        {
            Rectangle r = new Rectangle();
            for (int i = 0; i < tc.TabCount; i++)
            {
                r = tc.GetTabRect(i);
                if (r.Contains(location))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// ファイルを保存します。
        /// </summary>
        public void SaveFile(string filename,string text)
        {
            StreamWriter sw = new StreamWriter(filename,false,SelectEncoding);

            sw.Write(text);
            sw.Close();

        }

        /// <summary>
        /// ファイルを名前保存します。
        /// </summary>
        public void SaveFileAs(string text,TabPage page)
        {
            string fname;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fname = saveFileDialog1.FileName;
            }
            else
            {
                return;
            }

            StreamWriter sw = new StreamWriter(fname, false, SelectEncoding);
            sw.Write(text);
            sw.Close();
            FullFileName[page.GetHashCode()] = fname;
            isEdited[page.GetHashCode()] = false;
            page.Text = Path.GetFileName(fname);
        }

        /// <summary>
        /// ファイルを前保存します。
        /// </summary>
        public void SaveFileAll(TabControl tabc)
        {
            AzukiControl nowaz;
            TabPage nowpage;
            for (int i = 0; i < tabc.TabCount; i++)
            {
                nowpage = tabc.TabPages[i];
                nowaz = (AzukiControl)nowpage.Controls[0];

                if (FullFileName.ContainsKey(nowpage.GetHashCode())) {
                    SaveFile(FullFileName[nowpage.GetHashCode()],nowaz.Text);
                    ((AzukiControl)(tabControl1.SelectedTab.Controls[0])).Document.IsDirty = false;
                    isEdited[tabc.SelectedTab.GetHashCode()] = false;
                }

            }
        }

        /// <summary>
        /// スニペットツリーを構築します。
        /// </summary>
        public void SetSnippetsTree()
        {
            treeView2.Nodes.Clear();
            DirectoryInfo dirs = new DirectoryInfo("Data/Snippet");
            foreach (DirectoryInfo dir in dirs.GetDirectories())
            {
                TreeNode node_fl = new TreeNode(dir.Name);
                treeView2.Nodes.Add(node_fl);
                node_fl.Nodes.Add("---");
                S_IsDirectory[node_fl.GetHashCode()] = true;
            }

            foreach (FileInfo file in dirs.GetFiles("*.txt"))
            {
                TreeNode node_fl = new TreeNode(Path.GetFileNameWithoutExtension(file.Name));
                treeView2.Nodes.Add(node_fl);
                S_IsDirectory[node_fl.GetHashCode()] = false;
            }
        }

        /// <summary>
        /// フォルダツリーを構築します。
        /// 
        /// </summary>
        /// <param name="path">構築するツリーのパス</param>
        /// <param name="treev">魅せるツリービュー</param>
        public void ViewFolderTree(string path, TreeView treev)
        {
            treev.Nodes.Clear();
            IsDirectory.Clear();
            DirectoryInfo dirs = new DirectoryInfo(path);
            foreach (DirectoryInfo dir in dirs.GetDirectories())
            {
                TreeNode node_fl = new TreeNode(dir.Name, 4, 4);
                treev.Nodes.Add(node_fl);
                node_fl.Nodes.Add("---");
                IsDirectory[node_fl.GetHashCode()] =true;
                //Console.WriteLine(String.Format("NodeHash( {0} ) is true", node_fl.GetHashCode()));
            }

            foreach (FileInfo file in dirs.GetFiles())
            {
                TreeNode node_fl = new TreeNode(file.Name, GetFileIconIndex(file.Name), GetFileIconIndex(file.Name));
                treev.Nodes.Add(node_fl);
                IsDirectory[node_fl.GetHashCode()] = false;
                //Console.WriteLine(String.Format("NodeHash( {0} ) is false", node_fl.GetHashCode()));
            }

        }

        /// <summary>
        /// 拡張子から、ファイルタイプにあったアイコン番号を
        /// 取得します。
        /// </summary>
        /// <param name="ext">.つき拡張子</param>
        /// <returns>アイコン番号</returns>
        public int GetFileIconIndex(string ext)
        {
            switch (Path.GetExtension(ext).ToLower())
            {
                case ".js":
                case ".css":
                    return 0;

                case ".txt":
                case ".rtf":
                case ".log":
                    return 1;

                case ".png":
                case ".jpg":
                case ".gif":
                case ".jpeg":
                case ".bmp":
                case ".dib":
                    return 2;

                case ".exe":
                case ".bat":
                case ".com":
                    return 3;

                case ".html":
                case ".htm":
                case ".xml":
                    return 6;

                case ".cgi":
                case ".php":
                case ".rb":
                case ".py":
                case ".lua":
                    return 7;

                case ".zip":
                case ".lzh":
                case ".tar":
                case ".gz":
                case ".7z":
                case ".cab":
                    return 8;

                case ".mp3":
                case ".wav":
                case ".m4a":
                case ".ogg":
                case ".wma":
                    return 9;



                default:
                    return 1;

            }
        }

        /// <summary>
        /// 拡張子に見合った最適なコンテキスト(ryを
        /// 表示します。
        /// </summary>
        /// <param name="ext"></param>
        void ShowBestContextMenu(string ext,TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
            switch (GetFileIconIndex(ext))
            {
                case 2:
                    rcnode = e.Node;
                    contextMenuStrip5.Show(treeView1, e.Location);
                    break;
                default:
                    rcnode = e.Node;
                    contextMenuStrip3.Show(treeView1, e.Location);
                    break;

            }
        }
    }
}
