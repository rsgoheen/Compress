using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Compress.Core;

namespace Compress.Core.Test
{
   [TestClass]
   public class HuffingtonTreeTest
   {
      [TestMethod]
      public void TestEncodeDecode()
      {
         var input = "abcdef 123 a test string baba abba 234";
         var dict = CharacterFrequencyDictionary.CreateDictionary(input);

         var encodeTree = new HuffmanTree<char>(dict);

         var z = encodeTree.Encode(input);

         var decodeTree = new HuffmanTree<char>(dict);
         var decode = decodeTree.Decode(z);

         Assert.AreEqual(input, decode);
      }

      [TestMethod]
      public void TestEncodeDecodeWithCompression()
      {
         var input =
            "He paused for a moment, many recollections overpowering him. Then he went on telling her the history of his life, unfolding to her the story of his hopes and ambitions, describing to her the very home where he was born, and the dark-eyed sister whom he had loved, and with whom he had played over the daisied fields, and through the carpeted woods, and all among the richly tinted bracken. One day he was told she was dead, and that he must never speak her name; but he spoke it all the day and all the night, Beryl, nothing but Beryl, and he looked for her in the fields and in the woods and among the bracken. It seemed as if he had unlocked the casket of his heart, closed for so many years, and as if all the memories of the past and all the secrets of his life were rushing out, glad to be free once more, and grateful for the open air of sympathy.";

         var dict = CharacterFrequencyDictionary.CreateDictionary(input);

         var encodeTree = new HuffmanTree<char>(dict);

         var encode = encodeTree.Encode(input);

         var encodeAsByte = CompressUtil.ConvertToByteArray(encode);

         var secondDict = CharacterFrequencyDictionary.CreateDictionary(
            dict.GetKeysAsByteArray(), dict.GetValuesAsByteArray());

         var encodeAsByteArray = new List<byte>();
         foreach (var b in encodeAsByte)
            encodeAsByteArray.AddRange(CompressUtil.ConvertToBitArray(b));

         if (encode.Length < encodeAsByteArray.ToArray().Length)
            encodeAsByteArray.RemoveRange(encode.Length, encodeAsByteArray.ToArray().Length - encode.Length);

         CollectionAssert.AreEqual(dict, secondDict);
         CollectionAssert.AreEqual(encode, encodeAsByteArray.ToArray());

         var decodeTree = new HuffmanTree<char>(secondDict);
         var decode = decodeTree.Decode(encodeAsByteArray);

         Assert.AreEqual(input, decode);
      }
   }
}
