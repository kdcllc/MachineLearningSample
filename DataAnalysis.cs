using System;

namespace MachineLearning
{
    public class DataAnalysis
    {
        public static string[] BinData(string[] data, string[][] attributeValues, double[][] numericAttributeBorders)
        {
            // convert numeric height to "short", "medium", or "tall". assumes data is occupation,dominance,height,sex
            string[] result = new string[data.Length];
            string[] tokens;
            double heightAsDouble;
            string heightAsBinnedString;

            for (int i = 0; i < data.Length; ++i)
            {
                tokens = data[i].Split(',');
                heightAsDouble = double.Parse(tokens[2]);
                if (heightAsDouble <= numericAttributeBorders[0][0]) // short
                    heightAsBinnedString = attributeValues[2][0];
                else if (heightAsDouble >= numericAttributeBorders[0][1]) // tall
                    heightAsBinnedString = attributeValues[2][2];
                else
                    heightAsBinnedString = attributeValues[2][1]; // medium

                string s = tokens[0] + "," + tokens[1] + "," + heightAsBinnedString + "," + tokens[3];
                result[i] = s;
            }
            return result;
        }


        public static void ShowJointCounts(int[][][] jointCounts, string[][] attributeValues)
        {
            for (int k = 0; k < 2; ++k)
            {
                for (int i = 0; i < jointCounts.Length; ++i)
                    for (int j = 0; j < jointCounts[i].Length; ++j)
                        Console.WriteLine(attributeValues[i][j].PadRight(15) + "& " + attributeValues[3][k].PadRight(6) + " = " + jointCounts[i][j][k]);
                Console.WriteLine(""); // separate sexes
            }
        }
        public static int[][][] MakeJointCounts(string[] binnedData, string[] attributes, string[][] attributeValues)
        {
            // assumes binned data is occupation, dominance, height, sex
            // result[][][] -> [attribute][att value][sex]
            // ex: result[0][3][1] is the count of (occupation) (technology) (female), i.e., the count of technology AND female

            //dominance count
            var dominanceCount = attributeValues[1].Length;
            //height count
            var heightCount = attributeValues[2].Length;

            int[][][] jointCounts = new int[attributes.Length - 1][][]; // note the -1 (no sex)

            jointCounts[0] = new int[attributeValues[0].Length][]; // 4 occupations
            jointCounts[1] = new int[dominanceCount][]; // 2 dominances
            jointCounts[2] = new int[heightCount][]; // 3 heights

            //all of the features contains two classes
            var classCount = attributeValues[3].Length;

            jointCounts[0][0] = new int[classCount]; // 2 sexes for administrative
            jointCounts[0][1] = new int[classCount]; // construction
            jointCounts[0][2] = new int[classCount]; // education
            jointCounts[0][3] = new int[classCount]; // tedchnology

            jointCounts[1][0] = new int[dominanceCount]; // left
            jointCounts[1][1] = new int[dominanceCount]; // right

            jointCounts[2][0] = new int[heightCount]; // short
            jointCounts[2][1] = new int[heightCount]; // medium
            jointCounts[2][2] = new int[heightCount]; // tall

            for (int i = 0; i < binnedData.Length; ++i)
            {
                string[] tokens = binnedData[i].Split(',');

                int occupationIndex = NaiveBayesClassifyer.AttributeValueToIndex(0, tokens[0]);
                int dominanceIndex = NaiveBayesClassifyer.AttributeValueToIndex(1, tokens[1]);
                int heightIndex = NaiveBayesClassifyer.AttributeValueToIndex(2, tokens[2]);
                int sexIndex = NaiveBayesClassifyer.AttributeValueToIndex(3, tokens[3]);

                ++jointCounts[0][occupationIndex][sexIndex];  // occupation and sex count
                ++jointCounts[1][dominanceIndex][sexIndex];
                ++jointCounts[2][heightIndex][sexIndex];
            }

            return jointCounts;
        }
        public static int[] MakeDependentCounts(int[][][] jointCounts, int numDependents)
        {
            int[] result = new int[numDependents];
            for (int k = 0; k < numDependents; ++k)  // male then female
                for (int j = 0; j < jointCounts[0].Length; ++j) // scanning attribute 0 = occupation. could use any attribute
                    result[k] += jointCounts[0][j][k];

            return result;
        }

    }//class
}//ns