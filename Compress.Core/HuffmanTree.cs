using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress.Core
{
   class HuffmanTree<T>
   {
      public HuffmanNode<T> Root { get; set; } 

      public HuffmanTree(Dictionary<T, int> frequencyDictionary)
      {
         var sortedNodes = new Queue<HuffmanNode<T>>( frequencyDictionary.Keys.Count );

         foreach (var item in frequencyDictionary.OrderBy( x => x.Value))
            sortedNodes.Enqueue(new HuffmanNode<T>() {Value = item.Key, Weight = item.Value});

         while (sortedNodes.Count() > 1)
         {
            // Note: should use a priority queue implementation, but close enough for now.
            if (sortedNodes.Count >= 2)
            {
               var left = sortedNodes.Dequeue();
               var right = sortedNodes.Dequeue();

               var parent = new HuffmanNode<T>()
                  {
                     Value = default(T),
                     Weight = left.Weight + right.Weight,
                     LeftNode = left,
                     RightNode = right
                  };

               sortedNodes.Enqueue(parent);
            }
         }

         Root = sortedNodes.Dequeue();
         
         throw new NotImplementedException();
      }

      public BitArray Encode(string input)
      {
         var list = new List<bool>();

         BitArray bitlist = new BitArray(list.ToArray());

         throw new NotImplementedException();
      }

      public string Decode(BitArray input)
      {
         throw new NotImplementedException();
      }


   }
}
