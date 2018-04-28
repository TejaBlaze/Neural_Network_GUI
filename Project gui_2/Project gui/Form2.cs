//Training Data

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
using System.IO;

namespace Project_gui
{
    public partial class Form2 : Form
    {
        public string file;
        public Form5 f5;
        public Perceptron perc { get; set; }
        public Form2()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(file);

            LoadData ld = new LoadData();
            ld.Load(@file);
            MessageBox.Show("Loaded data");

            f5 = new Form5(ld);
            f5.Show();
            this.Hide();
            f5.FormClosing += F5_Closing;
        }

        private void F5_Closing(object sender, FormClosingEventArgs e)
        {
            perc = f5.perc;
            this.Close();
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void button3_Click_1(object sender, EventArgs e)
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
