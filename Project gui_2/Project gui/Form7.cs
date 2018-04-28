//Testing Writing to file and Evaluating

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
    public partial class Form7 : Form
    {
        public Perceptron perc { get; set; }
        public List<double> predicted { get; set; }
        public LoadData ld { get; set; }
        public Form7(Perceptron p1, LoadData l1)
        {
            perc = p1;
            ld = l1;
            predicted = perc.Test(ld.Data);
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Do something
            List<double> Evaluated_res = perc.Evaluate(predicted, ld.Targets);
            double acc = Evaluated_res[0] * 100.0;
            double specificity = Evaluated_res[1] * 100.0;
            double sensitivity = Evaluated_res[2] * 100.0;
            MessageBox.Show("Performance stats:\nAccuracy: "+acc.ToString()+"% " +
                "\nSpecificity: "+specificity.ToString()+ "% " +
                "\nSensitivity: " + sensitivity.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8(predicted, ld);
            f8.Show();
            this.Hide();
            f8.FormClosing += F8_Closing;
        }

        private void F8_Closing(object sender, FormClosingEventArgs e)
        {
            this.Show();
        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }
    }
}
