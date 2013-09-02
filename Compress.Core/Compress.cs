using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress.Core
{
    public class Compress
    {

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
