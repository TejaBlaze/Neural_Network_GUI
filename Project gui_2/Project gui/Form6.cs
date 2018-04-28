//Optimize parameters

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
    public partial class Form6 : Form
    {
        public Perceptron perc { get; set; }
        public LoadData ld;
        public Form6(LoadData l1)
        {
            ld = l1;
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            int No_of_folds = int.Parse(textBox1.Text);
            double eta_start = double.Parse(textBox2.Text);
            double eta_stop = double.Parse(textBox3.Text);
            double eta_step = double.Parse(textBox4.Text);
            int epoch_start = int.Parse(textBox5.Text);
            int epoch_stop = int.Parse(textBox6.Text);
            int epoch_step = int.Parse(textBox7.Text);

            Console.WriteLine(No_of_folds);
            Console.WriteLine(eta_start);
            Console.WriteLine(eta_stop);
            Console.WriteLine(eta_step);
            Console.WriteLine(epoch_start);
            Console.WriteLine(epoch_stop);
            Console.WriteLine(epoch_step);

            this.Hide();
            Console.WriteLine("Optimising...");
            OptimizeParameters optparam = new OptimizeParameters();
            optparam.Set(eta_start, eta_stop, eta_step, epoch_start, epoch_stop, epoch_step);
            optparam.CrossValidate(ld.Data, ld.Targets, No_of_folds);
            Console.WriteLine("Optimal parameters\nEta : {0}\tEpochs: {1}", optparam.eta, optparam.num_epochs);


            perc = new Perceptron();
            perc.Train(ld.Data, ld.Targets, 1, optparam.num_epochs, optparam.eta);

            MessageBox.Show("Done");
            this.Close();
        }
    }
}
