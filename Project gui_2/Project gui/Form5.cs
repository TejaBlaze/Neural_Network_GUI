//Changing parameters

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
    public partial class Form5 : Form
    {
        public LoadData ld;
        public Form4 f4;
        public Form6 f6;
        public Perceptron perc { get; set; }
        public Form5(LoadData l1)
        {
            ld = l1;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f4 = new Form4(ld);
            this.Hide();
            f4.Show();
            f4.FormClosing += F4_Closing;
        }

        private void F4_Closing(object sender, FormClosingEventArgs e)
        {
            perc = f4.perc;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f6 = new Form6(ld);
            this.Hide();
            f6.Show();
            f6.FormClosing += F6_Closing;
        }

        private void F6_Closing(object sender, FormClosingEventArgs e)
        {
            perc = f6.perc;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
