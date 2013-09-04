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
       /// <summary>
       /// Takes the header (dictionary) and Huffman encoded input and creates a single
       /// byte array that can be persisted.
       /// </summary>
       /// <param name="fileHeader">The header information (frequency dictionary) as a byte array</param>
       /// <param name="compressed">The bit array from the Huffman compression</param>
       /// <returns>A byte array that can be persisted</returns>
       public static byte[] GetFileByteArray(byte[] fileHeader, byte[] compressed)
       {
          var compressedLength = BitConverter.GetBytes(compressed.Length);

          var compressedByteArray = ConvertToByteArray(compressed);

          var fileData = new byte[fileHeader.Length 
             + compressedLength.Length 
             + compressedByteArray.Length];

          int len = 0;
          fileHeader.CopyTo(fileData, len);

          len += fileHeader.Length;
          compressedLength.CopyTo(fileData, len);

          len += compressedLength.Length;
          compressedByteArray.CopyTo(fileData, len);

          return fileData;
       }

       /// <summary>
       /// Convert an array of bits into a (smaller) array of bytes
       /// </summary>
       /// <param name="bitArray">A byte array comprised of bit (0 and 1) values</param>
       /// <returns>A byte array of smaller length than the input array</returns>
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

       /// <summary>
       /// Converts a byte into an array of bits (0 and 1 only).  Intended to be the converse
       /// of <see cref="Compress.Core.CompressUtil.ConvertToByteArray"/>.
       /// </summary>
       /// <param name="byteItem">A byte to convert into an array of bit values</param>
       /// <returns>A byte array containing only 0 and 1 as elements</returns>
       public static byte[] ConvertToBitArray(byte byteItem)
       {
          var b = new byte[8];

          for (var i = 0; i < 8; i++)
          {
             b[i] = (byteItem % 2 == 0) ? (byte)0 : (byte)1;
             byteItem = (byte)(byteItem >> 1);
          }
          return b;
       }
    }
}
