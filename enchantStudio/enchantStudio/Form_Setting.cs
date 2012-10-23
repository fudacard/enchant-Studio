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
    public partial class Form_Setting : Form
    {
        public Form_Setting()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ESConfigを渡してモーダル表示
        /// </summary>
        /// <param name="confclass"></param>
        public void ShowDialog(ESConfig confclass) {
            propertyGrid1.SelectedObject = confclass;
            this.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
