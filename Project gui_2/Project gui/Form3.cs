//Testing Data

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AI_Project_Recent;

namespace Project_gui
{
    public partial class Form3 : Form
    {
        public string file;
        public Perceptron perc { get; set; }
        public Form3(Perceptron p1)
        {
            perc = p1;
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(file);

            LoadData ld = new LoadData();
            ld.Load(@file);
            MessageBox.Show("Loaded data");

            Form7 f7 = new Form7(perc, ld);
            f7.Show();
            this.Hide();
            f7.FormClosing += F7_Closing;
        }

        private void F7_Closing(object sender, FormClosingEventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                file = ofd.FileName;
                label2.Visible = true;
                button1.Visible = true;
            }
        }
    }
}
