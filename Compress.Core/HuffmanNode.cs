using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress.Core
{
   public class HuffmanNode<T> : IEquatable<HuffmanNode<T>>, IComparer<HuffmanNode<T>>, IComparable<HuffmanNode<T>>
   {
      public int Weight { get; set; }
      public T Value { get; set; }
      public HuffmanNode<T> LeftNode { get; set; }
      public HuffmanNode<T> RightNode { get; set; }
      public HuffmanNode<T> Parent { get; set; } 

      public bool IsLeaf
      {
         get { return LeftNode == null && RightNode == null; }
      }

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
