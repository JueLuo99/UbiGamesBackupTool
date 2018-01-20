using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UbiGamesBackupTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonBackup_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void buttonBackupTo_Click(object sender, EventArgs e)
        {

        }
    }
}
