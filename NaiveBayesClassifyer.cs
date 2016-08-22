using System;
namespace MachineLearning
{
    public class NaiveBayesClassifyer
    {
        public static int Classify(string occupation, string dominance, string height, int[][][] jointCounts, int[] dependentCounts, bool withSmoothing, int xClasses)
        {
            double partProbMale = PartialProbability("male", occupation, dominance, height, jointCounts, dependentCounts, withSmoothing, xClasses);
            double partProbFemale = PartialProbability("female", occupation, dominance, height, jointCounts, dependentCounts, withSmoothing, xClasses);
            double evidence = partProbMale + partProbFemale;
            double probMale = partProbMale / evidence;
            double probFemale = partProbFemale / evidence;

            //Console.WriteLine("Partial prob of male   = " + partProbMale.ToString("F6"));
            //Console.WriteLine("Partial prob of female = " + partProbFemale.ToString("F6"));

            Console.WriteLine("Probability of male   = " + probMale.ToString("F4"));
            Console.WriteLine("Probability of female = " + probFemale.ToString("F4"));
            if (probMale > probFemale) // could use a threshold
                return 0;
            else
                return 1;
        }
        public static double PartialProbability(string sex, string occupation, string dominance, string height, int[][][] jointCounts, int[] dependentCounts, bool withSmoothing, int xClasses)
        {
            int sexIndex = AttributeValueToIndex(3, sex);

            int occupationIndex = AttributeValueToIndex(0, occupation);
            int dominanceIndex = AttributeValueToIndex(1, dominance);
            int heightIndex = AttributeValueToIndex(2, height);

            int totalMale = dependentCounts[0];
            int totalFemale = dependentCounts[1];
            int totalCases = totalMale + totalFemale;

            int totalToUse = 0;
            if (sex == "male") totalToUse = totalMale;
            else if (sex == "female") totalToUse = totalFemale;

            double p0 = (totalToUse * 1.0) / (totalCases); // prob of either male or female
            double p1 = 0.0;
            double p2 = 0.0;
            double p3 = 0.0;

            if (withSmoothing == false)
            {
                p1 = (jointCounts[0][occupationIndex][sexIndex] * 1.0) / totalToUse;  // conditional prob of male (or female, depending on sex parameter) given the occupation
                p2 = (jointCounts[1][dominanceIndex][sexIndex] * 1.0) / totalToUse;   // conditional prob of the specified sex, given the specified domnance
                p3 = (jointCounts[2][heightIndex][sexIndex] * 1.0) / totalToUse;      // condition prob given specified height
            }
            else if (withSmoothing == true) // Laplacian smoothing to avoid 0-count joint probabilities
            {
                p1 = (jointCounts[0][occupationIndex][sexIndex] + 1) / ((totalToUse + xClasses) * 1.0);  // add 1 to count in numerator, add number x classes in denominator
                p2 = (jointCounts[1][dominanceIndex][sexIndex] + 1) / ((totalToUse + xClasses) * 1.0);   // conditional prob of the specified sex, given the specified domnance
                p3 = (jointCounts[2][heightIndex][sexIndex] + 1) / ((totalToUse + xClasses) * 1.0);
            }

            //return p0 * p1 * p2 * p3; // risky if any very small values
            return Math.Exp(Math.Log(p0) + Math.Log(p1) + Math.Log(p2) + Math.Log(p3));
        }

        public static int AnalyzeJointCounts(int[][][] jointCounts)
        {
            // check for any joint-counts that are 0 which could blow up Naive Bayes
            int zeroCounts = 0;

            for (int i = 0; i < jointCounts.Length; ++i) // attribute
                for (int j = 0; j < jointCounts[i].Length; ++j) // value
                    for (int k = 0; k < jointCounts[i][j].Length; ++k) // sex
                        if (jointCounts[i][j][k] == 0)
                            ++zeroCounts;
            return zeroCounts;
        }
       
        public static int AttributeValueToIndex(int attribute, string attributeValue)
        {
            // we could do this programmatically (maybe with a Dictionary) but a crude approach is more clear
            if (attribute == 0)
            {
                if (attributeValue == "administrative") return 0;
                else if (attributeValue == "construction") return 1;
                else if (attributeValue == "education") return 2;
                else if (attributeValue == "technology") return 3;
            }
            else if (attribute == 1)
            {
                if (attributeValue == "left") return 0;
                else if (attributeValue == "right") return 1;
            }
            else if (attribute == 2)
            {
                if (attributeValue == "short") return 0;
                else if (attributeValue == "medium") return 1;
                else if (attributeValue == "tall") return 2;
            }
            else if (attribute == 3)
            {
                if (attributeValue == "male") return 0;
                else if (attributeValue == "female") return 1;
            }
            return -1; // error
        }
    }//class   
}//ns