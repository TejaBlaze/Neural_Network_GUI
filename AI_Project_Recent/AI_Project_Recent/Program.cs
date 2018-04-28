using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace AI_Project_Recent
{
    public static class DataPreprocess
    {
        public static void Normalize(ref List<List<double>> Train_Data)
        {
                List<double> mins = new List<double>(new double[Train_Data[0].Count]);
                List<double> maxs = new List<double>(new double[Train_Data[0].Count]);
                for (int j = 0; j < Train_Data[0].Count; j++)
                {
                    for (int i = 0; i < Train_Data.Count; i++)
                    {
                        if (mins[j] > Train_Data[i][j]) mins[j] = Train_Data[i][j];
                        if (maxs[j] < Train_Data[i][j]) maxs[j] = Train_Data[i][j];
                    }
                }
                for (int j = 0; j < Train_Data[0].Count; j++)
                {
                    for (int i = 0; i < Train_Data.Count; i++)
                    {
                        Train_Data[i][j] = (Train_Data[i][j] - mins[j]) / (maxs[j] - mins[j]);
                    }
                }

            }
        }

        public class LoadData
        {
            public List<double> Targets;
            public List<List<double>> Data;

            public void Load(string fn)
                {
                Targets = new List<double>();
                Data = new List<List<double>>();
                    
                int N = 0;
                    using (var reader = new System.IO.StreamReader(fn))//@ added
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            List<double> curr_row = new List<double>();
                            curr_row.Add(double.Parse(values[0]));
                            for (int i = 2; i < values.Length; i++)
                                curr_row.Add(double.Parse(values[i]));
                            Data.Add(curr_row);
                            if (values[1].Equals("B")) Targets.Add(0);
                            else Targets.Add(1);
                            N += 1;
                        }
                        /*for (int i = 0; i < N; i++)
                        {
                            Console.WriteLine("\n{0} -> {1}", String.Join(" ", Data[i]), Targets[i]);
                        }*/
                    }
                    DataPreprocess.Normalize(ref Data);

                }
        }


        public class Solution
        {
 //       [STAThread] //n
        public static void Main()
            {

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            LoadData ld = new LoadData();
            Perceptron p = new Perceptron();
            List<double> Train_Targets;
            List<List<double>> Train_Data;
            List<double> Test_Targets;
            List<List<double>> Test_Data;
            while (true) { 
                Console.WriteLine("Choose\n1)Train classifier\n2)Test classifier\n3)Exit");
                int choice = int.Parse(Console.ReadLine());
                int N = 0;
                if (choice == 1)
                {
                    ld.Load(@"N:\Engineering\Project\C sharp\C# Project final\wdbc_train.csv");
                    Train_Targets = ld.Targets;
                    Train_Data = ld.Data;
                    Console.Write("Choose\n1)Manually enter parameters\n2)Optimize parameters\n");
                    int cho = int.Parse(Console.ReadLine());
                    if (cho == 1)
                    { 
                        Console.Write("Training parameters:\nLearning rate: ");
                        double eta = double.Parse(Console.ReadLine());
                        Console.Write("Number of epochs: ");
                        int num_epochs = int.Parse(Console.ReadLine());

                        Console.WriteLine("Training on data");
                        Console.Write("Print cost vs iteration?(0/1): ");
                        int diag_flag = int.Parse(Console.ReadLine());
                        p.Train(Train_Data, Train_Targets, diag_flag, num_epochs, eta);
                    }

                    else if (cho == 2)
                    {
                        Console.Write("Enter number of folds(k): ");
                        int k = int.Parse(Console.ReadLine());
                        OptimizeParameters optparam = new OptimizeParameters();
                        optparam.SetRange();
                        optparam.CrossValidate(Train_Data, Train_Targets, k);
                        double eta = optparam.eta;
                        int num_epochs = optparam.num_epochs;

                        Console.WriteLine("Training on data");
                        Console.Write("Print cost vs iteration?(0/1): ");
                        int diag_flag = int.Parse(Console.ReadLine());
                        p.Train(Train_Data, Train_Targets, diag_flag, num_epochs, eta);

                    }
                }
                else if (choice == 2)
                {
                    ld.Load(@"N:\Engineering\Project\C sharp\C# Project final\wdbc_test.csv");
                    Test_Targets = ld.Targets;
                    Test_Data = ld.Data;
                  

                    List<double> predicted = p.Test(Test_Data);

                    /*for (int i = 0; i < Test_Data.Count; i++)
                    {
                        foreach (var x in Test_Data[i])
                        {
                            Console.Write(x.ToString() + ' ');
                        }
                        Console.WriteLine("-> {0}", predicted[i].ToString());
                    }*/
                    List<double> evaluated_res = p.Evaluate(predicted, Test_Targets);
                    Console.WriteLine("Accuracy: {0}%", evaluated_res[0] * 100.0);
                    Console.WriteLine("Specificity: {0}%", evaluated_res[1] * 100.0);
                    Console.WriteLine("Sensitivity: {0}%", evaluated_res[2] * 100.0);
                }
               
                else break;
            }
            //Console.ReadKey();
            }
        }


    //Perceptron class
    public class Perceptron
    {
        //Weights and bias
        public double bias;
        public List<double> weights;

        //Initialize weights and bias
        public void Init_weights(List<List<double>> X)
        {
            Random r = new Random();
            weights = new List<double>();
            for (int i = 0; i < X[0].Count; i++)
            {
                weights.Add(r.NextDouble());
            }
            bias = r.NextDouble();
        }


        public void Train(List<List<double>> X, List<double> d, int diag_flag=0, int epoch = 1000, double eta = 0.01)
        {
            //Initialize the weights
            Init_weights(X);
            double cost,y, error;
            for (int iteration = 0; iteration < epoch; iteration++)
            {
                if (diag_flag == 1) Console.Write("Iteration: {0}", iteration);
                cost = 0.0;
                for (int i = 0; i < X.Count; i++)
                {
                    y = sigmoid(multiply(X[i]));
                    error = d[i] - y;
                    cost += error * error;
                    WeightUpdate(error, X[i], eta);
                }
                if (diag_flag==1) Console.WriteLine("\tCost: {0}", cost);
            }
        }

        public List<double> Test(List<List<double>> X)
        {
            List<double> predicted = new List<double>();
            for (int i = 0; i < X.Count; i++)
            {
                predicted.Add(Predict(X[i]));
            }
            return predicted;
        }

        public double Predict(List<double> x)
        {
            return Math.Round(sigmoid(multiply(x)));
        }

        public List<double> Evaluate(List<double> predicted, List<double> desired)
        {
            double accuracy = 0.0, sensitivity = 0.0, specificity = 0.0;
            double TP=0.0, TN=0.0, FN=0.0, FP=0.0;
            for (int i = 0; i < predicted.Count; i++)
            {
                if (predicted[i] == desired[i]) accuracy++;
                if (predicted[i] >= 0.5 && desired[i] >= 0.5) TP++;
                if (predicted[i] < 0.5 && desired[i] >= 0.5) FN++;
                if (predicted[i] < 0.5 && desired[i] < 0.5) TN++;
                if (predicted[i] >= 0.5 && desired[i] < 0.5) FP++;
            }
            //Console.WriteLine(TP + "\t" + FN + "\t" + TN + "\t" + FP);
            accuracy /= predicted.Count;
            sensitivity = TP / (TP+FN);
            specificity = TN / (TN+FP);
            List<double> Evaluated_res = new List<double>();
            Evaluated_res.Add(accuracy);
            Evaluated_res.Add(specificity);
            Evaluated_res.Add(sensitivity);
            return Evaluated_res;
        }

        public double multiply(List<double> x)
        {
            double y = 0;
            for (int i = 0; i < x.Count; i++)
            {
                y += (x[i] * weights[i]);
            }
            y = y + bias;
            return y;
        }

        public double sigmoid(double x)
        {
            double y = 0;
            y = (1.0 / (1.0 + (Math.Exp(-x))));
            return y;
        }

        public double Differential(double v)
        {
            double y = sigmoid(v);
            return y * (1 - y);
        }

        public void WeightUpdate(double error, List<double> x, double eta=0.01)
        {
            for (int i = 0; i < weights.Count; i++)
            {
                weights[i] = weights[i] + (eta * error * Differential(multiply(x)) * x[i]);
            }
            bias = bias + (eta * error * Differential(multiply(x)));
        }


    }

    public class OptimizeParameters : Perceptron
    {
        public double eta;
        public int num_epochs;
        public List<double> eta_range;
        public List<int> epoch_range;
        public OptimizeParameters(double e = 0.1, int ne = 1000) { eta = e; num_epochs = ne; }

        public void Set(double le, double ue, double se, int ln, int un, int sn)
        {
            double leta=le, ueta=ue, step_eta=se;
            int lnum_epochs=ln, unum_epochs=un, step_epochs=sn;
            eta_range = new List<double>();
            for (double i = leta; i <= ueta; i += step_eta) eta_range.Add(i);
            epoch_range = new List<int>();
            for (int i = lnum_epochs; i <= unum_epochs; i += step_epochs) epoch_range.Add(i);
        }

        public void SetRange()
        {
            double leta, ueta, step_eta;
            int lnum_epochs, unum_epochs, step_epochs;
            Console.Write("Enter range of learning rate[lower_limit, upper_limit] and step:\n");
            leta = double.Parse(Console.ReadLine());
            ueta = double.Parse(Console.ReadLine());
            step_eta = double.Parse(Console.ReadLine());
            Console.Write("Enter range of number of epochs[lower_limit, upper_limit] and step:\n");
            lnum_epochs = int.Parse(Console.ReadLine());
            unum_epochs = int.Parse(Console.ReadLine());
            step_epochs = int.Parse(Console.ReadLine());
            eta_range = new List<double>();
            for (double i = leta; i <= ueta; i += step_eta) eta_range.Add(i);
            epoch_range = new List<int>();
            for (int i = lnum_epochs; i <= unum_epochs; i += step_epochs) epoch_range.Add(i);
        }

        public void CrossValidate(List<List<double>> Tr_Data, List<double> Tr_Targets, int k= 5)
        {
            int lim = (int)(Tr_Data.Count / k), max_epoch=100;
            double max_eta=0.1;
            List<double> avg_accs = new List<double>();
            for (int i = 0; i < eta_range.Count * epoch_range.Count; i++) avg_accs.Add(0.0);
            for (int i = 0; i < k; i++)
            {
                List<List<double>> Curr_tr_data = new List<List<double>>();
                List<List<double>> Curr_te_data = new List<List<double>>();
                List<double> Curr_tr_targets = new List<double>();
                List<double> Curr_te_targets = new List<double>();

                for (int j = 0; j < Tr_Data.Count; j++)
                {
                    if (j >= i * lim && j < (i + 1) * lim)
                    {
                        Curr_te_data.Add(Tr_Data[j]);
                        Curr_te_targets.Add(Tr_Targets[j]);
                    }
                    else
                    {
                        Curr_tr_data.Add(Tr_Data[j]);
                        Curr_tr_targets.Add(Tr_Targets[j]);
                    }
                }
                double avg_acc = 0.0, curr_acc;
                //double avg_sensitivity = 0.0, curr_sensitivity;
                //double avg_specificity = 0.0, curr_specificity;
                int ctr = 0;
                foreach (var curr_eta in eta_range)
                {
                    foreach (var curr_epoch in epoch_range)
                    {
                        Train(Curr_tr_data, Curr_tr_targets, 0, curr_epoch, curr_eta);
                        List<double> preds = Test(Curr_te_data);
                        List<double> Evaluated_res = Evaluate(preds, Curr_te_targets);
                        curr_acc = Evaluated_res[0];
                        avg_accs[ctr] = curr_acc;
                        //Console.WriteLine("Iter: {0}\tEta: {1}\tEpochs: {2}\tAccuracy: {3}%",ctr, curr_eta, curr_epoch, curr_acc*100.0);
                        ctr++;
                    }
                }

            }
            double max_acc = 0.0;
            int eta_id=0, epoch_id=0;
            for (int i = 0; i < eta_range.Count; i++)
            {
                for (int j = 0; j < epoch_range.Count; j++)
                {
                    avg_accs[i*j] /= k;
                    Console.WriteLine("Iter: {0}\tEta: {1}\tEpochs: {2}\tAvg_acc: {3}", i*j, eta_range[i], epoch_range[j], avg_accs[i*j]);
                    if (max_acc < avg_accs[i*j])
                    {
                        max_acc = avg_accs[i*j];
                        eta_id = i;
                        epoch_id = j;
                    }
                }
            }
            eta = eta_range[eta_id];
            num_epochs = epoch_range[epoch_id];
        }
    }

}
