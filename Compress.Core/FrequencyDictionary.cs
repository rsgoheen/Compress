using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress.Core
{
   public class CharacterFrequencyDictionary : Dictionary<char, int>
   {
      /// <summary>
      /// Create a character frequency dictionary given a string
      /// </summary>
      /// <param name="input"></param>
      /// <returns>A dictionary with the key being characters in the input string and the values
      /// being the number of times the character appears in the input string</returns>
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

      /// <summary>
      /// create a character frequency dictionary given byte arrays representing the keys
      /// and the values.  Intended to be used with the output of <see cref="GetKeysAsByteArray"/> and
      /// <see cref="GetValuesAsByteArray"/>.
      /// </summary>
      /// <param name="keyArray">The dictionary keys as a byte array from <see cref="GetKeysAsByteArray"/></param>
      /// <param name="valueArray">The dictonary values as a byte array from <see cref="GetKeysAsByteArray"/></param>
      /// <returns></returns>
      public static CharacterFrequencyDictionary CreateDictionary(byte[] keyArray, byte[] valueArray)
      {
         var keys = Encoding.ASCII.GetString(keyArray).ToCharArray();
         var values = Encoding.ASCII.GetString(valueArray).Split(',').Select(Int32.Parse).ToArray();

         if (keys.Length != values.Length)
            throw new ArgumentException(
               String.Format("Key array length ({0}) does not match value array length ({1}) ", keys.Length, values.Length));

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

      public static byte[] GetHeaderByteArray(CharacterFrequencyDictionary dict)
      {
         var dictKeys = dict.GetKeysAsByteArray();
         var dictValues = dict.GetValuesAsByteArray();

         var keyLength = BitConverter.GetBytes(dictKeys.Length);
         var dictLength = BitConverter.GetBytes(dictValues.Length);

         var headerLength = dictKeys.Length + dictValues.Length + keyLength.Length + dictLength.Length;

         var fileHeader = new byte[headerLength];

         int len = 0;

         keyLength.CopyTo(fileHeader, len);
         len += keyLength.Length;

         dictLength.CopyTo(fileHeader, len);
         len += dictLength.Length;

         dictKeys.CopyTo(fileHeader, len);
         len += dictKeys.Length;

         dictValues.CopyTo(fileHeader, len);
         return fileHeader;
      }
   }
}
