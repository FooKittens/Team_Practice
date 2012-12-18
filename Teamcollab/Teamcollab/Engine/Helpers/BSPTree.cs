using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teamcollab.Engine.Helpers
{
  public class BSPTree<T>
  {
    public delegate bool NodeComparison(T data);
    public delegate int TraverseComparison(T data);

    #region Properties

    #endregion

    #region Members
    Node<T> start;
    //NodeComparison comparison;
    #endregion

    public BSPTree(T origin)
    {
      start = new Node<T>();
      start.Back = null;
      start.Left = null;
      start.Right = null;
      start.Data = origin;
    }

    public T Find(NodeComparison comparison)
    {

      if (comparison(start.Data))
      {
        return start.Data;
      }

      return start.Data;
    }

    private static Node<T> Traverse(Node<T> me, T t)
    {
      return null;
    }

    private class Node<T>
    {
      public Node<T> Back;
      public Node<T> Left;
      public Node<T> Right;

      public T Data;
    }

  }
}
