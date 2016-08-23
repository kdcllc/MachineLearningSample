using System;
namespace MachineLearning
{
  public class GenerateSample
  {
    static Random ran = new Random(25); // 25 gives a nice demo

   public static string[] MakeData(int numRows) // make dummy data
    {
      string[] result = new string[numRows];
      for (int i = 0; i < numRows; ++i)
      {
        string sex = MakeSex();
        string occupation = MakeOccupation(sex);
        string dominance = MakeDominance(sex);
        string height = MakeHeight(sex);
        string s = occupation + "," + dominance + "," + height + "," + sex;
        result[i] = s;
      }
      return result;
    }

    static string MakeSex()
    {
      int r = ran.Next(0, 19);
      if (r >= 0 && r <= 11) return "male"; // 60%
      else if (r >= 12 && r <= 19) return "female"; // 40%
      return "error";
    }

    static string MakeDominance(string sex)
    {
      double p = ran.NextDouble();
      if (sex == "male")
      {
        if (p < 0.33) return "left"; else return "right";
      }
      else if (sex == "female")
      {
        if (p < 0.20) return "left"; else return "right";
      }
      return "error";
    }

    static string MakeOccupation(string sex)
    {
      int r = ran.Next(0, 20);
      if (sex == "male")
      {
        if (r == 0) return "administrative"; // 5%
        else if (r >= 1 && r <= 6) return "construction"; // 30%
        else if (r >= 7 && r <= 9) return "education"; // 15%
        else if (r >= 10 && r <= 19) return "technology"; // 50%
      }
      else if (sex == "female")
      {
        if (r >= 0 & r <= 9) return "administrative"; // 50%
        else if (r == 10) return "construction"; // 5%
        else if (r >= 11 & r <= 15) return "education";  // 25%
        else if (r >= 16 && r <= 19) return "technology"; // 20%
      }
      return "error";
    }

    static string MakeHeight(string sex)
    {
      int bucket = 0;  // height bucket: 0 = short, 1 = medium, 2 = tall
      double p = ran.NextDouble();
      if (p < 0.1587) bucket = 0;
      else if (p > 0.8413) bucket = 2;
      else bucket = 1; // p = (2 * 0.3413) = 0.6826

      double hi = 0.0;
      double lo = 0.0;

      if (sex == "male")
      {
        if (bucket == 0) { lo = 60.0; hi = 66.0; }
        else if (bucket == 1) { lo = 66.0; hi = 72.0; }
        else if (bucket == 2) { lo = 72.0; hi = 78.0; }
      }
      else if (sex == "female")
      {
        if (bucket == 0) { lo = 57.0; hi = 63.0; }
        else if (bucket == 1) { lo = 63.0; hi = 69.0; }
        else if (bucket == 2) { lo = 69.0; hi = 75.0; }
      }

      double resultAsDouble = (hi - lo) * ran.NextDouble() + lo;
      return resultAsDouble.ToString("F1");
    }


    
  } // class Program
} // ns
