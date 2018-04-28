//Saving to file

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
    public partial class Form8 : Form
    {
        public string file;
        public int flag = 0;
        public Perceptron perc { get; set; }
        public LoadData ld { get; set; }
        public List<double> predicted { get; set; }
        public Form8(List<double> pr, LoadData l1)
        {
            predicted = pr;
            ld = l1;
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(file);
            flag = 1;
            using (var w = new StreamWriter(file))
            {
                for (int i=0; i<ld.Data.Count; i++)
                {
                    string curr_row = "";
                    foreach (var ele in ld.Data[i]) curr_row += ele.ToString() + ",";
                    curr_row += predicted[i].ToString();
                    var line = curr_row; // string.Format("{0},{1}", first, second);
                    w.WriteLine(line);
                    w.Flush();
                }
            }
            MessageBox.Show("File saved to "+ file);
            //this.Close();
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                file = sfd.FileName;
                button1.Visible = true;
                label1.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (flag == 1)
                this.Close();
            else
            {
                MessageBox.Show("File not saved yet \nPlease press Save and then exit");
            }
        }
    }
}
