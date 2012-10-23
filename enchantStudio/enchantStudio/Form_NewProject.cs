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
    public partial class Form_NewProject : Form
    {
        Form_SetProject setp;
        ESProject proj;
        ESConfig conf;

        public Form_NewProject()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set(ryとESPr(ryとEScon(ryを指定しませーな。
        /// </summary>
        /// <param name="setprj"></param>
        /// <param name="prj"></param>
        public void ShowDialog(Form_SetProject setprj,ESProject prj,ESConfig cnf)
        {
            setp = setprj;
            proj = prj;
            conf = cnf;
            textBox1.Text = conf.ProjectPath;
            ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            proj.ProjectPath = textBox1.Text;
            proj.ProjectName = textBox2.Text;
            conf.ProjectPath = textBox1.Text;
            this.Close();
            setp.ShowDialog(proj);
        }
    }
}
