using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress.Core
{
   public class CharacterFrequencyDictionary : Dictionary<char, int>
   {
      public static CharacterFrequencyDictionary CreateDictionary(string input)
      {
         var dict = new CharacterFrequencyDictionary();

         foreach (var c in input.ToCharArray())
         {
            if (!dict.ContainsKey(c))
               dict.Add(c, 0);

            dict[c]++;
         }

         return dict;
      }

      public static CharacterFrequencyDictionary CreateDictionaryFromByteArray(byte[] keyArray, byte[] valueArray)
      {
         var keys = Encoding.ASCII.GetString(keyArray).ToCharArray();
         var values = Encoding.ASCII.GetString(valueArray).Split(',').Select(int.Parse).ToArray();

         if (keys.Length != values.Length)
            throw new ArgumentException(
               string.Format("Key array length ({0}) does not match value array length ({1}) ", keys.Length, values.Length));

         var dict = new CharacterFrequencyDictionary();

         for (var i = 0; i < keys.Length; i++)
            dict.Add(keys[i], values[i]);

         return dict;
      }

      public byte[] GetKeysAsByteArray()
      {
         return Encoding.ASCII.GetBytes(new string(Keys.ToArray()));
      }

      public byte[] GetValuesAsByteArray()
      {
         return Encoding.ASCII.GetBytes(String.Join(",", Values.Select(x => x.ToString()).ToArray()));
      }
   }
}
