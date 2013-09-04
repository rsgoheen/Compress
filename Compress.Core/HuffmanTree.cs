using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress.Core
{
   public class HuffmanTree<T>
   {
      public HuffmanNode<T> Root { get; private set; }
      public Dictionary<T, int> FrequencyDictionary { get; private set; }

      private readonly Dictionary<T, HuffmanNode<T>> _nodeDictionary = new Dictionary<T, HuffmanNode<T>>();

      public HuffmanTree(IDictionary<T, int> frequencyDictionary)
      {
         FrequencyDictionary = new Dictionary<T, int>(frequencyDictionary);

         var sortedNodes = new SortedSet<HuffmanNode<T>>();

         foreach (var item in frequencyDictionary.OrderBy(x => x.Value))
         {
            var node = new HuffmanNode<T>() { Value = item.Key, Weight = item.Value };
            sortedNodes.Add(node);
            _nodeDictionary.Add(node.Value, node);
         }

         while (sortedNodes.Count() > 1)
         {
            var left = sortedNodes.Min;
            sortedNodes.Remove(left);
            var right = sortedNodes.Min;
            sortedNodes.Remove(right);

            var parent = new HuffmanNode<T>()
            {
               Value = default(T),
               Weight = left.Weight + right.Weight,
               LeftNode = left,
               RightNode = right
            };

            left.Parent = parent;
            right.Parent = parent;

            sortedNodes.Add(parent);
         }

         Root = sortedNodes.FirstOrDefault();
      }

      public byte[] Encode(IEnumerable<T> input)
      {
         var list = new List<byte>();

         foreach (var item in input)
            list.AddRange(EncodeElement(item));

         return list.ToArray();
      }

      private IEnumerable<byte> EncodeElement(T item)
      {
         var list = new List<byte>();
         var node = _nodeDictionary[item];

         while (!node.IsRoot)
         {
            list.Add(node.Parent.LeftNode == node ? (byte)1 : (byte)0);
            node = node.Parent;
         }

         list.Reverse();
         return list;
      }

      public string Decode(IEnumerable<byte> input)
      {
         var sb = new StringBuilder();
         var node = Root;

         foreach (var b in input)
         {
            node = (b == 1) ? node.LeftNode : node.RightNode;

            if (!node.IsLeaf) continue;

            sb.Append(node.Value);
            node = Root;
         }

         return sb.ToString();
      }
   }
}
