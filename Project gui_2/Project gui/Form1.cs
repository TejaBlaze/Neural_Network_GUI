//Home form

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AI_Project_Recent;

namespace Project_gui
{
    public partial class Form1 : Form
    {
        public Form2 f2;
        public Perceptron perc { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (perc == null)
            {
                MessageBox.Show("Train classifier first!");
            }
            else
            {
                Form3 f3 = new Form3(perc);
                f3.Show();
                this.Hide();
                f3.FormClosing += F3_Closing;
            }
        }

        private void F3_Closing(object sender, FormClosingEventArgs e)
        {
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f2 = new Form2();
            f2.Show();
            this.Hide();
            f2.FormClosing += F2_Closing;
        }

        private void F2_Closing(object sender, FormClosingEventArgs e)
        {
            this.Show();
            perc = f2.perc;
        }

        private void F4_Closing(object sender, FormClosingEventArgs e)
        {
            this.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure to quit?", "Confirm", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                this.Close();
        }
        
    }
}
