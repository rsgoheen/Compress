using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compress.Core
{
   class HuffmanNode<T>
   {
      public int Weight { get; set; }
      public T Value { get; set; }
      public HuffmanNode<T> LeftNode { get; set; }
      public HuffmanNode<T> RightNode { get; set; } 

      public bool IsLeaf
      {
         get { return LeftNode == null && RightNode == null; }
      }
   }
}
