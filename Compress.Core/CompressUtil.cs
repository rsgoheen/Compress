using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress.Core
{
    public class CompressUtil
    {

       public static void CompressFile(string inputFile, string outputFile)
       {
          var input = File.ReadAllText(inputFile);

          var bytes = Encoding.ASCII.GetBytes(outputFile);
          var str = Encoding.ASCII.GetString(bytes);
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

       public static byte[] GetFileByteArray(byte[] fileHeader, byte[] compressed)
       {
          var compressedByteArray = ConvertToByteArray(compressed);

          var fileData = new byte[fileHeader.Length + compressedByteArray.Length];

          fileHeader.CopyTo(fileData, 0);
          compressedByteArray.CopyTo(fileData, fileHeader.Length);

          return fileData;
       }

       public static byte[] ConvertToByteArray(byte[] bitArray)
       {
          var bytes = (bitArray.Length + 7)/8;
          var arr2 = new byte[bytes];

          int bitIndex = 0, byteIndex = 0;
          foreach (var b in bitArray)
          {
             if (b == 1)
                arr2[byteIndex] |= (byte) (1 << bitIndex);

             bitIndex++;

             if (bitIndex != 8) continue;

             bitIndex = 0;
             byteIndex++;
          }
          return arr2;
       }

       public static byte[] ConvertToBitArray(int record)
       {
          var b = new byte[8];
          var x = (byte) record;

          for (var i = 0; i < 8; i++)
          {
             b[i] = (x%2 == 0) ? (byte) 0 : (byte) 1;
             x = (byte) (x >> 1);
          }
          return b;
       }
    }
}
