using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compress.Core;

namespace Compress
{
   class Program
   {
      private const string _compressFileExtension = ".compress";
      public static Encoding Encoding = Encoding.ASCII;

      static void Main(string[] args)
      {
         if (args.Length <= 1)
         {
            ShowHelp();
            return;
         }

         try
         {
            if (String.Equals(args[0].Substring(1, 1), "g", StringComparison.OrdinalIgnoreCase))
            {
               ShowDictionaryKeyAndValueStrings(args[1]);
               return;
            }

            if (String.Equals(args[0].Substring(1, 1), "c", StringComparison.OrdinalIgnoreCase))
               CompressFile(args[1]);

            if (String.Equals(args[0].Substring(1, 1), "d", StringComparison.OrdinalIgnoreCase))
               DecompressFile(args[1]);

         }
         catch (FileNotFoundException ex)
         {
            WriteFileError(args[2]);
         }
         catch (Exception ex)
         {
            WriteUnhandledException(ex);
         }
      }

      private static void ShowDictionaryKeyAndValueStrings(string filename)
      {
         var input = String.Join(Environment.NewLine, File.ReadAllText(filename, Encoding));
         var dict = CharacterFrequencyDictionary.CreateDictionary(input);
         Console.WriteLine("Keys:");
         Console.WriteLine(Convert.ToBase64String(dict.GetKeysAsByteArray()));
         Console.WriteLine("Values:");
         Console.WriteLine(Convert.ToBase64String(dict.GetValuesAsByteArray()));
         Console.WriteLine("Any key to continue.");
         Console.ReadKey();

         return;
      }

      private static void CompressFile(string inputFile)
      {
         var outputFile = inputFile + _compressFileExtension;

         var input = String.Join(System.Environment.NewLine, File.ReadAllText(inputFile, Encoding));

         var dict = CharacterFrequencyDictionary.CreateDictionary(input);

         var fileHeader = CompressUtil.GetHeaderByteArray(dict);
         var compressed = new HuffmanTree<char>(dict).Encode(input);
         var fileByteArray = CompressUtil.GetFileByteArray(fileHeader, compressed);

         using (var fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            fs.Write(fileByteArray, 0, fileByteArray.Length);
      }

      public static void DecompressFile(string fileName)
      {
         using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
         {
            var keyLengthArray = new byte[4];
            var valueLengthArray = new byte[4];

            fs.Read(keyLengthArray, 0, 4);
            fs.Read(valueLengthArray, 0, 4);

            var keyLength = BitConverter.ToInt32(keyLengthArray, 0);
            var valueLength = BitConverter.ToInt32(valueLengthArray, 0);

            var keyArray = new byte[keyLength];
            var valueArray = new byte[valueLength];

            fs.Read(keyArray, 0, keyLength);
            fs.Read(valueArray, 0, valueLength);

            var dict = CharacterFrequencyDictionary.CreateDictionaryFromByteArray(keyArray, valueArray);

            var huffmanTree = new HuffmanTree<char>(dict);

            var outputFile = fileName.Substring(0, (fileName.Length - _compressFileExtension.Length));

            var encodedStream = new List<byte>();
            int record;

            while ((record = fs.ReadByte()) != -1)
               encodedStream.AddRange(CompressUtil.ConvertToBitArray(record));

            using (var fsw = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (var sr = new StreamWriter(fsw))
               sr.Write(huffmanTree.Decode(encodedStream));
         }
      }

      public static void ShowHelp()
      {
         Console.Write(@"General usage:
CompressUtil.exe [-c | -d] -g INPUT_FILENAME OUTPUT_FILENAME

-c compresses the file specified by INPUT_FILENAME into OUTPUT_FILENAME
-d decompresses INPUT_FILENAME into OUTPUT_FILENAME
-g generate a dictionary based on the input file (compression only)

One of the two compression flags (-c or -d) and the input file name must be specified. When compressing
a file, the file is assumed to be a text file.  The resulting output file, as well as the input for the
decompress option, is a binary file.

If the generate flag (-g) is specified without the compress flag (-c), the program will output two
binary lines, base 64 encoded: the first will be the keys for the dictionary the second will be the frequency.  These
are intended for internal application use and maintenance of internal (default) dictionaries.
");
      }

      private static void WriteFileError(string s)
      {
         Console.WriteLine("File " + s + " was not found.  Please confirm the file name and retry.");
      }

      private static void WriteUnhandledException(Exception exception)
      {
         Console.WriteLine("An unhandled exception occured. Details: " + exception.Message);
         Console.WriteLine(exception.StackTrace);
      }

   }
}
