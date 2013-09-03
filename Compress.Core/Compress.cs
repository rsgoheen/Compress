using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress.Core
{
    public class Compress
    {

       public static void CompressFile(string inputFile, string outputFile)
       {
          var input = File.ReadAllText(inputFile);

          var bytes = System.Text.Encoding.ASCII.GetBytes(outputFile);
          var str = System.Text.Encoding.ASCII.GetString(bytes);
       }

       public static Dictionary<char, int> CreateDictionary (string input)
       {
          var dict = new Dictionary<char, int>();

          foreach (var c in input.ToCharArray())
          {
             if (! dict.ContainsKey(c))
                dict.Add(c,0);

             dict[c]++;
          }

          return dict;
       }
    }
}
