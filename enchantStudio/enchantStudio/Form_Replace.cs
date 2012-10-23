using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sgry.Azuki;
using Sgry.Azuki.WinForms;

namespace enchantStudio
{
    public partial class Form_Replace : Form
    {
        AzukiControl refaz;
        public Form_Replace()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 指定のアズキコントロールで置き換えをします。
        /// </summary>
        /// <param name="az"></param>
        public void ShowDialog(AzukiControl az)
        {
            refaz = az;
            this.ShowDialog();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //次を
        private void button1_Click(object sender, EventArgs e)
        {
            SearchResult sr;
            if (checkBox2.Checked)
            {
                sr = refaz.Document.FindNext(new System.Text.RegularExpressions.Regex(textBox1.Text), refaz.CaretIndex, refaz.CaretIndex);
            }
            else
            {
                sr = refaz.Document.FindNext(textBox1.Text, refaz.CaretIndex,checkBox1.Checked);
            }
            if (sr != null)
            {
                refaz.Document.Replace(textBox2.Text, sr.Begin, sr.End);
            }
        }

        //すべて
        private void button2_Click(object sender, EventArgs e)
        {
            SearchResult sr;
            int nowi = 0;
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(textBox1.Text);
            do
            {
                if (checkBox2.Checked)
                {
                    sr = refaz.Document.FindNext(reg, nowi, refaz.CaretIndex);
                }
                else
                {
                    sr = refaz.Document.FindNext(textBox1.Text, nowi, checkBox1.Checked);
                }
                if (sr != null)
                {
                    refaz.Document.Replace(textBox2.Text, sr.Begin, sr.End);
                    nowi = sr.End;
                }
            } while (sr != null);
        }
    }
}
