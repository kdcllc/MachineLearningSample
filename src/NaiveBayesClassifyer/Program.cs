using System;

namespace MachineLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("\nBegin Naive Bayes Classification demo\n");
                Console.WriteLine("Demo will classify sex based on occpation, dominance, height\n");

                string[] attributes = new string[] { "occupation", "dominance", "height", "sex" };

                string[][] attributeValues = new string[attributes.Length][];  // could scan values from raw data
                attributeValues[0] = new string[] { "administrative", "construction", "education", "technology" };
                attributeValues[1] = new string[] { "left", "right" };
                attributeValues[2] = new string[] { "short", "medium", "tall" };
                attributeValues[3] = new string[] { "male", "female" };

                double[][] numericAttributeBorders = new double[1][];     // there may be several numeric variables
                numericAttributeBorders[0] = new double[] { 64.0, 71.0 }; // height range: [57.0 to 78.0]

                Console.WriteLine("Generating 40 lines of occupation, dominance, height, sex data\n");
                string[] data = GenerateSample.MakeData(40);

                Console.WriteLine("First 4 lines of training data are:\n");

                for (int i = 0; i < 4; ++i)
                    Console.WriteLine(data[i]);
                Console.WriteLine("\n");

                Console.WriteLine("Converting numeric height data to categorical data on 64.0 71.0\n");

                string[] binnedData = DataAnalysis.BinData(data, attributeValues, numericAttributeBorders);  // convert numeric heights to categories

                Console.WriteLine("First 4 lines of binned training data are:\n");
                for (int i = 0; i < 4; ++i)
                    Console.WriteLine(binnedData[i]);
                Console.WriteLine("\n");

                Console.WriteLine("Scanning binned data to compute joint and dependent counts\n");
                int[][][] jointCounts = DataAnalysis.MakeJointCounts(binnedData, attributes, attributeValues);
                int[] dependentCounts = DataAnalysis.MakeDependentCounts(jointCounts, 2);
                Console.WriteLine("Total male = " + dependentCounts[0]);
                Console.WriteLine("Total female = " + dependentCounts[1]);
                Console.WriteLine("");

                DataAnalysis.ShowJointCounts(jointCounts, attributeValues);

                // classify the sex of a person whose occupation is education, is right-handed, and is tall
                string occupation = "education";
                string dominance = "right";
                string height = "tall";

                bool withLaplacian = true;  // prevent joint counts with 0

                Console.WriteLine("Using Naive Bayes " + (withLaplacian ? "with" : "without") + " Laplacian smoothing to classify when:");
                Console.WriteLine(" occupation = " + occupation);
                Console.WriteLine(" dominance = " + dominance);
                Console.WriteLine(" height = " + height);
                Console.WriteLine("");

                int c = NaiveBayesClassifyer.Classify(occupation, dominance, height, jointCounts, dependentCounts, withLaplacian, 3);
                if (c == 0)
                    Console.WriteLine("\nData case is most likely male");
                else if (c == 1)
                    Console.WriteLine("\nData case is most likely female");

                Console.WriteLine("\nEnd demo\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }//class
} //ns