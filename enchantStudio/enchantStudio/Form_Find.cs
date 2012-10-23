using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Sgry.Azuki.WinForms;
using Sgry.Azuki.Highlighter;
using Sgry.Azuki;

namespace enchantStudio
{
    public partial class Form_Find : Form
    {
        TabControl ntc;
        AzukiControl nowaz;
        SearchResult re;
        int next;       //次の検索開始
        int prevbegin;  //前回の検索結果範囲の開始位置

        public Form_Find()
        {
            InitializeComponent();
            next = 0;
            prevbegin = 0;
        }

        /// <summary>
        /// AzukiControlのタブコントロールを渡して、
        /// 選択されているTabを検索するウィンドウを出します。
        /// </summary>
        /// <param name="tc">対象のTabControl</param>
        public void ShowWithTabControl(TabControl tc)
        {
            if (tc.TabCount <= 0) return;
            ntc = tc;
            this.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nowaz = (AzukiControl)ntc.SelectedTab.Controls[0];
            if (checkBox2.Checked)
            {
                Regex reg = new Regex(textBox1.Text);
                nowaz.Document.FindNext(reg, next);
            }
            else
            {
                re = nowaz.Document.FindNext(textBox1.Text, next, checkBox1.Checked);
            }
            
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
                //MessageBox.Show("終端まで検索しました", "検索結果", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            nowaz = (AzukiControl)ntc.SelectedTab.Controls[0];
            if (checkBox2.Checked)
            {
                Regex reg = new Regex(textBox1.Text);
                nowaz.Document.FindPrev(reg, prevbegin);
            }
            else
            {
                re = nowaz.Document.FindPrev(textBox1.Text, prevbegin, checkBox1.Checked);
            }
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
                //MessageBox.Show("終端まで検索しました", "検索結果", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
