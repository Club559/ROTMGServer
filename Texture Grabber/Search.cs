using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Texture_Grabber
{
    public partial class Search : Form
    {
        public Form1 main;
        public Search(Form1 main)
        {
            InitializeComponent();

            this.main = main;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Searching...";
            button1.Enabled = false;
            if (main.FindItems(textBox1.Text))
            {
                Hide();
                main.Focus();
            }
            else
            {
                MessageBox.Show("No matches found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            button1.Text = "Search";
            button1.Enabled = true;
        }

        private void Search_Shown(object sender, EventArgs e)
        {
            if (!main.HasItems())
                button1.Enabled = false;
        }

        private void Search_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            main.Focus();
        }
    }
}
