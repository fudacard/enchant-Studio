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
    public partial class Form_Ref : Form
    {
        public Form_Ref()
        {
            InitializeComponent();
        }

        private void トップページToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("http://enchantjs.com/ja/reference.html");
        }

        private void 戻るToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void 進むToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void 更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

        private void Form_Ref_Load(object sender, EventArgs e)
        {

        }
    }
}
