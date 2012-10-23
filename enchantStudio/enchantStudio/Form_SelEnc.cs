using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace enchantStudio
{
    public partial class Form_SelEnc : Form
    {
        Encoding enc;

        public Form_SelEnc()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Encodingを変更されられるShowDialogのオーバーロードです。
        /// </summary>
        /// <param name="selenc">対象のEncoding</param>
        public void ShowDialog(Encoding selenc)
        {
            enc = selenc;
            ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                enc = Encoding.GetEncoding(int.Parse(textBox1.Text));
            }
            catch (Exception)
            {
                this.Close();
                return;
            }
            this.Close();
        }
    }
}
