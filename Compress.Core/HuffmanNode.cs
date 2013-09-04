using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress.Core
{
   /// <summary>
   /// Encapsulates a node in a tree.  A node can either be a leaf with a value, or an empty node with
   /// child nodes.
   /// </summary>
   public class HuffmanNode<T> : IEquatable<HuffmanNode<T>>, IComparer<HuffmanNode<T>>, IComparable<HuffmanNode<T>>
   {
      public int Weight { get; set; }
      public T Value { get; set; }
      public HuffmanNode<T> LeftNode { get; set; }
      public HuffmanNode<T> RightNode { get; set; }
      public HuffmanNode<T> Parent { get; set; } 

      /// <summary>
      /// True if this is a leaf node containing a value (has no child nodes)
      /// </summary>
      public bool IsLeaf
      {
         get { return LeftNode == null && RightNode == null; }
      }

      /// <summary>
      /// True if this is the top node of a tree (has no parents)
      /// </summary>
      public bool IsRoot
      {
         get { return Parent == null; }
      }

      public string Name
      {
         get
         {
            if (IsLeaf) return Value.ToString();

            return (LeftNode == null ? "" : LeftNode.Name)
                   + (RightNode == null ? "" : RightNode.Name);
         }

      }

      public bool Equals(HuffmanNode<T> other)
      {
         if (other == null) return false;

         return Name == other.Name;
      }

      public int Compare(HuffmanNode<T> x, HuffmanNode<T> y)
      {
         var weightComp = x.Weight.CompareTo(y.Weight);

         return weightComp == 0 
            ? String.Compare(x.Name, y.Name, StringComparison.Ordinal) 
            : weightComp;
      }

      public int CompareTo(HuffmanNode<T> other)
      {
         return Compare(this, other);
      }
   }
}
