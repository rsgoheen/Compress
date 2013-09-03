using System;
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
               var input = String.Join(System.Environment.NewLine, File.ReadAllText(args[1], Encoding));
               var dict = CharacterFrequencyDictionary.CreateDictionary(input);
               Console.WriteLine("Keys:");
               Console.WriteLine(Convert.ToBase64String(dict.GetKeysAsByteArray()));
               Console.WriteLine("Values:");
               Console.WriteLine(Convert.ToBase64String(dict.GetValuesAsByteArray()));
            }
            return;

         }
         catch (FileNotFoundException ex)
         {
            WriteFileError(args[2]);
         }
         catch (Exception ex)
         {
            WriteUnhandledException(ex);
         }



         if (String.Equals(args[0].Substring(1,1), "c", StringComparison.OrdinalIgnoreCase))
         {
            try
            {

            }
            catch (FileNotFoundException ex)
            {
               WriteFileError(args[2]);
            }
            catch (Exception ex)
            {
               WriteUnhandledException(ex);
            }

            return;
         }


         if (String.Equals(args[0].Substring(1, 1), "d", StringComparison.OrdinalIgnoreCase))
         {
            try
            {

            }
            catch (FileNotFoundException ex)
            {
               WriteFileError(args[2]);
            }
            catch (Exception ex)
            {
               WriteUnhandledException(ex);
            }

            return;
         }

      }

      public static void ShowHelp()
      {
         Console.Write(@"General usage:
Compress.exe [-c | -d] -g INPUT_FILENAME OUTPUT_FILENAME

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
