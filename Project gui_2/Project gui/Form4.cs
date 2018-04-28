//Manual change of parameters

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
    public partial class Form4 : Form
    {
        public LoadData ld;
        public Perceptron perc { get; set; }
        public Form4(LoadData l1)
        {
            ld = l1;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int epoch = int.Parse(textBox1.Text);
            double alpha = float.Parse(textBox2.Text);
            Console.WriteLine(epoch);
            Console.WriteLine(alpha);
            this.Hide();
            perc = new Perceptron();
            perc.Train(ld.Data, ld.Targets, 1, epoch, alpha);

            MessageBox.Show("Done");
            this.Close();
        }

    }
}
