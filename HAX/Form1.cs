using System;
using System.Windows.Forms;

namespace HAX
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Main.label1 = label1;
            Main.Start();
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            Main.showBones = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Main.showBoxes = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Main.showHP = checkBox3.Checked;
        }
    }
}
