using System;
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
         var dict = Compress.CreateDictionary(input);

         var encodeTree = new HuffmanTree<char>(dict);

         var z = encodeTree.Encode(input);

         var decodeTree = new HuffmanTree<char>(dict);
         var decode = decodeTree.Decode(z);

         Assert.AreEqual(input,decode);
      }
   }
}
