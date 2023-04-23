using System;
using System.IO;

namespace CLASSIFICATION_USING_K_NEAREST_NEIGHBORS_ALGORITHM
{
    class Program
    {
        static void Main(string[] args)
        {
            double[,] datasetArray = GetData();

            BanknoteClassification(datasetArray);
            MeasuringSuccess(datasetArray);
            ShowDataset(datasetArray);
        }

        static double[,] GetData() //File reader
        {
            string[] file = File.ReadAllLines("data_banknote_authentication.txt");
            double[,] datasetArray = new double[file.GetLength(0), 5];

            for (int i = 0; i < file.GetLength(0); i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    datasetArray[i, j] = Convert.ToDouble(file[i].Split(",")[j].Replace(".", ","));
                }
            }
            return datasetArray;
        }

        static double[,] KNNalgorithm(double[,] dataset, int k, double variance, double skewness, double kurtosis, double entropy, bool print, string mode = "single value")
        { //kNN algorithm
            double distance;

            double[,] ratingArray = new double[k, 2];
            for (int i = 0; i < ratingArray.GetLength(0); i++) ratingArray[i, 0] = double.MaxValue;

            for (int i = 0; i < dataset.GetLength(0); i++)
            {
                distance = Math.Sqrt(Math.Pow(dataset[i, 0] - variance, 2) + Math.Pow(dataset[i, 1] - skewness, 2) + Math.Pow(dataset[i, 2] - kurtosis, 2) + Math.Pow(dataset[i, 3] - entropy, 2));
                for (int s = 0; s < ratingArray.GetLength(0); s++)
                {
                    if (distance < ratingArray[s, 0])
                    {
                        for (int p = ratingArray.GetLength(0) - 1; p > s; p--)
                        {
                            ratingArray[p, 0] = ratingArray[p - 1, 0];
                            ratingArray[p, 1] = ratingArray[p - 1, 1];
                        }
                        ratingArray[s, 0] = distance;
                        ratingArray[s, 1] = i;
                        break;
                    }
                }
            }

            double truthValue = 0;

            for (int i = 0; i < ratingArray.GetLength(0); i++)
            {
                truthValue += dataset[Convert.ToInt32(ratingArray[i, 1]), 4];
            }

            truthValue /= k; //  data's average calculation
            if (print)
            {
                if (truthValue > (0.5)) Console.WriteLine("\n--> Likely real.\n");

                else if (truthValue < (0.5)) Console.WriteLine("\n--> Likely fake.\n");

                else
                {
                    if (dataset[Convert.ToInt32(ratingArray[0, 1]), 4] == 0)
                    {
                        Console.WriteLine("\n--> Likely fake, equal condition.\n");
                    }
                    else
                    {
                        Console.WriteLine("\n--> Likely real, equal condition.\n");
                    }
                }
            }

            if (mode.Equals("Array")) //return array
            {
                return ratingArray;
            }

            else //return type value
            {
                if (truthValue > (0.5)) return new double[1, 1] { { 1 } };
                else if (truthValue < (0.5)) return new double[1, 1] { { 0 } };
                else
                {
                    if (dataset[Convert.ToInt32(ratingArray[0, 1]), 4] == 0) return new double[1, 1] { { 0 } };
                    else return new double[1, 1] { { 1 } };
                }
            }
        }

        static void BanknoteClassification(double[,] dataset) //Inputs and table
        {
            //Inputs
            Console.WriteLine("---   Banknote Classification   ---\n");

            Console.Write("Please Enter 'k' Value: ");
            int k = Convert.ToInt32(Console.ReadLine());

            Console.Write("Please Enter Variance: ");           
            double variance = Convert.ToDouble(Console.ReadLine());

            Console.Write("Please Enter Skewness: ");
            double skewness = Convert.ToDouble(Console.ReadLine());

            Console.Write("Please Enter Kurtosis: ");
            double kurtosis = Convert.ToDouble(Console.ReadLine());

            Console.Write("Please Enter Entropy: ");
            double entropy = Convert.ToDouble(Console.ReadLine());

            double[,] closestData = KNNalgorithm(dataset, k, variance, skewness, kurtosis, entropy, true,"Array");

            //Table prints
            Console.Write("Subject Index | ");
            Console.Write("Variance Value | ");
            Console.Write("Skewness Value   | ");
            Console.Write("Kurtosis Value | ");
            Console.Write("Entropy Value  | ");
            Console.WriteLine("        Type");
            Console.WriteLine("--------------|----------------|------------------|----------------|----------------|----------------------");

            for (int i = 0; i < k; i++)
            {
                //checking if the values positive or negative with if else blocks for table
                //and printing table
                if (closestData[i, 1]<1000 && closestData[i, 1]>99) Console.Write("      " + closestData[i, 1] + "     |     "); //Subject Index
                else if (closestData[i, 1]<100 && closestData[i, 1]>9) Console.Write("      " + closestData[i, 1] + "      |     "); 
                else if (closestData[i, 1] < 10 && closestData[i, 1] > -1) Console.Write("      " + closestData[i, 1] + "       |     "); 
                else Console.Write("      " + closestData[i, 1] + "    |     "); 
                //Variance
                if (dataset[Convert.ToInt32(closestData[i, 1]), 0]<0) Console.Write(dataset[Convert.ToInt32(closestData[i, 1]), 0].ToString("F") + "      |      ");
                else Console.Write(dataset[Convert.ToInt32(closestData[i, 1]), 0].ToString("F") + "       |      ");

                //Skewness
                if (dataset[Convert.ToInt32(closestData[i, 1]), 1]<0) Console.Write(dataset[Convert.ToInt32(closestData[i, 1]), 1].ToString("F") + "       |     ");
                else Console.Write(dataset[Convert.ToInt32(closestData[i, 1]), 1].ToString("F") + "        |     ");

                //Kurtosis              
                if (dataset[Convert.ToInt32(closestData[i, 1]), 2]<0) Console.Write(dataset[Convert.ToInt32(closestData[i, 1]), 2].ToString("F") + "      |      ");
                else Console.Write(dataset[Convert.ToInt32(closestData[i, 1]), 2].ToString("F") + "       |      ");

                //Entropy               
                if (dataset[Convert.ToInt32(closestData[i, 1]), 3]<0) Console.Write(dataset[Convert.ToInt32(closestData[i, 1]), 3].ToString("F") + "     |         ");
                else Console.Write(dataset[Convert.ToInt32(closestData[i, 1]), 3].ToString("F") + "      |         ");

                if (Convert.ToBoolean(dataset[Convert.ToInt32(closestData[i, 1]), 4])) Console.WriteLine("Real"); //type control
                else Console.WriteLine("Fake");
            }
            Console.WriteLine("\n\n*************************************************************************************************************\n\n");
        }

        static void MeasuringSuccess(double[,] dataset) //part c: success measuring
        {
            Console.WriteLine("---   Measuring Success   ---\n");

            Console.Write("Please Enter 'k' Value: ");
            int k = Convert.ToInt32(Console.ReadLine());

            double[,] editedDataset = new double[dataset.GetLength(0) - 200, dataset.GetLength(1)];
            double[,] selectedData = new double[200, dataset.GetLength(1)];

            int count = dataset.GetLength(0) - 200 - 1;
            int selectcount = 0;

            for (int i = dataset.GetLength(0) - 1; i >= 0; i--) //comparison
            {
                if (selectcount < 100 && dataset[i, 4] == 1) 
                {
                    for (int s = 0; s < 5; s++) selectedData[selectcount, s] = dataset[i, s];

                    selectcount++;
                }
                else if (selectcount < 200 && dataset[i, 4] == 0)
                {
                    for (int s = 0; s < 5; s++) selectedData[selectcount, s] = dataset[i, s];

                    selectcount++;
                }
                else
                {
                    for (int s = 0; s < 5; s++) editedDataset[count, s] = dataset[i, s];

                    count--;
                }
            }
            double[,] closestData;

            for (int j = 0; j < selectedData.GetLength(0); j++)
            {
                if (selectedData[j, 4] == 1) 
                {
                    Console.Write("\n--> Actually real. ");
                }
                else
                {
                    Console.Write("\n--> Actually fake. ");
                }
                closestData = KNNalgorithm(editedDataset, k, selectedData[j, 0], selectedData[j, 1], selectedData[j, 2], selectedData[j, 3], true,"Array");

                //Table prints
                Console.Write("Subject Index | ");
                Console.Write("Variance Value | ");
                Console.Write("Skewness Value   | ");
                Console.Write("Kurtosis Value | ");
                Console.Write("Entropy Value  | ");
                Console.WriteLine("        Type");
                Console.WriteLine("--------------|----------------|------------------|----------------|----------------|----------------------");

                for (int i = 0; i < k; i++)
                {
                    //checking if the values positive or negative with if else blocks for table
                    //and printing table
                    if (closestData[i, 1] < 1000 && closestData[i, 1] > 99) Console.Write("      " + closestData[i, 1] + "     |     "); //Subject Index
                    else if (closestData[i, 1] < 100 && closestData[i, 1] > 9) Console.Write("      " + closestData[i, 1] + "      |     ");
                    else if (closestData[i, 1] < 10 && closestData[i, 1] > -1) Console.Write("      " + closestData[i, 1] + "       |     ");
                    else Console.Write("      " + closestData[i, 1] + "    |     ");
                    //Variance
                    if (editedDataset[Convert.ToInt32(closestData[i, 1]), 0] < 0) Console.Write(editedDataset[Convert.ToInt32(closestData[i, 1]), 0].ToString("F") + "      |      ");
                    else Console.Write(editedDataset[Convert.ToInt32(closestData[i, 1]), 0].ToString("F") + "       |      ");

                    //Skewness
                    if (editedDataset[Convert.ToInt32(closestData[i, 1]), 1] < 0) Console.Write(editedDataset[Convert.ToInt32(closestData[i, 1]), 1].ToString("F") + "       |     ");
                    else Console.Write(editedDataset[Convert.ToInt32(closestData[i, 1]), 1].ToString("F") + "        |     ");

                    //Kurtosis              
                    if (editedDataset[Convert.ToInt32(closestData[i, 1]), 2] < 0) Console.Write(editedDataset[Convert.ToInt32(closestData[i, 1]), 2].ToString("F") + "      |      ");
                    else Console.Write(editedDataset[Convert.ToInt32(closestData[i, 1]), 2].ToString("F") + "       |      ");

                    //Entropy               
                    if (editedDataset[Convert.ToInt32(closestData[i, 1]), 3] < 0) Console.Write(editedDataset[Convert.ToInt32(closestData[i, 1]), 3].ToString("F") + "     |         ");
                    else Console.Write(editedDataset[Convert.ToInt32(closestData[i, 1]), 3].ToString("F") + "      |         ");

                    if (Convert.ToBoolean(editedDataset[Convert.ToInt32(closestData[i, 1]), 4])) Console.WriteLine("Real"); //type control
                    else Console.WriteLine("Fake");
                }
            }
            
            
            int successCount = 0;
            for (int i = 0; i < selectedData.GetLength(0); i++)
            {
                if (KNNalgorithm(editedDataset, k, selectedData[i, 0], selectedData[i, 1], selectedData[i, 2], selectedData[i, 3], false)[0, 0] == selectedData[i, 4]) successCount++;
            }
            Console.WriteLine("\n---> Success Rate %" + (successCount / 200.0) * 100); //success rate calculation
            Console.WriteLine("\n\n*************************************************************************************************************\n\n");
        }

        static void ShowDataset(double[,] dataset) //part d: listing dataset
        {
            Console.WriteLine("Press a Key To Show The Dataset\n\n");
            Console.ReadKey();

            Console.Write("\nSubject Index | ");
            Console.Write("Variance Value | ");
            Console.Write("Skewness Value   | ");
            Console.Write("Kurtosis Value | ");
            Console.Write("Entropy Value  | ");
            Console.WriteLine("        Type");
            Console.WriteLine("--------------|----------------|------------------|----------------|----------------|----------------------");

            for (int i = 0; i < dataset.GetLength(0); i++)
            {
                if (i < 1000 && i > 99) Console.Write("      " + i + "     |     "); //Subject Index
                else if (i < 100 && i > 9) Console.Write("      " + i + "      |     ");
                else if (i < 10 && i > -1) Console.Write("      " + i + "       |     ");
                else Console.Write("      " + i + "    |     ");
                
                //checking how many digits dataset have

                //Variance
                if (dataset[i, 0] <= -10) Console.Write(dataset[i, 0].ToString("F") + "     |      ");
                else if (dataset[i, 0] < 0 && dataset[i, 0] >-10) Console.Write(dataset[i, 0].ToString("F") + "      |      ");
                
                else if (dataset[i, 0] >=0 && dataset[i, 0]<10) Console.Write(dataset[i, 0].ToString("F") + "       |      ");
                else Console.Write(dataset[i, 0].ToString("F") + "      |      ");

                //Skewness
                if (dataset[i, 1] <= -10) Console.Write(dataset[i, 1].ToString("F") + "      |     ");
                else if (dataset[i, 1] < 0 && dataset[i, 1] > -10) Console.Write(dataset[i, 1].ToString("F") + "       |     ");
                
                else if (dataset[i, 1] >= 0 && dataset[i, 1] < 10) Console.Write(dataset[i, 1].ToString("F") + "        |     ");
                else Console.Write(dataset[i, 1].ToString("F") + "       |     ");

                //Kurtosis
                if (dataset[i, 2] <= -10) Console.Write(dataset[i, 2].ToString("F") + "     |      ");
                else if (dataset[i, 2] < 0 && dataset[i, 2] > -10) Console.Write(dataset[i, 2].ToString("F") + "      |      ");
                
                else if (dataset[i, 2] >= 0 && dataset[i, 2] < 10) Console.Write(dataset[i, 2].ToString("F") + "       |      ");
                else Console.Write(dataset[i, 2].ToString("F") + "      |      ");

                //Entropy
                if (dataset[i, 3] < 0) Console.Write(dataset[i, 3].ToString("F") + "     |         ");
                else Console.Write(dataset[i, 3].ToString("F") + "      |         ");

                if (Convert.ToBoolean(dataset[i, 4])) Console.WriteLine("Real"); //type control
                else Console.WriteLine("Fake");
            }
            Console.WriteLine("\n\n*************************************************************************************************************\n\n");
        }
    }
}
