using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public T Find(TraverseComparison comparison)
    {
      int searches;
      return Find(comparison, out searches);
    }

    public T Find(TraverseComparison comparison, out int searches)
    {
      searches = 0;
      Node<T> node = Traverse(start, comparison, ref searches);

      return node.Data;
    }

    public void Insert(T data, TraverseComparison comparison)
    {
      Node<T> node = new Node<T>();
      node.Data = data;
      RecurseInsert(node, start, comparison);
    }

    private static void RecurseInsert(Node<T> data, Node<T> node,
      TraverseComparison comparison)
    {
      data.Back = node;
      int dir = comparison(node.Data);

      if (dir == 0)
      {
        throw new ArgumentException("Missed insertion point in RecurseInsert");
      }

      if (dir < 0)
      {
        if (node.Left == null)
        {
          Debug.WriteLine(string.Format(
              "Setting node({0}) left of node({1})",
              data.Data.ToString(), node.Data.ToString()
            )
          );
          node.Left = data;
        }
        else
        {
          //node.Left.Back = node;
          RecurseInsert(data, node.Left, comparison);
        }
      }
      else if (dir > 0)
      {
        if (node.Right == null)
        {
          Debug.WriteLine(string.Format(
              "Setting node({0}) right of node({1})",
              data.Data.ToString(), node.Data.ToString()
            )
          );
          node.Right = data;
        }
        else
        {
          //node.Right.Back = node;
          RecurseInsert(data, node.Right, comparison);
        }
      }
    }

    //public void Insert(T data, TraverseComparison comparison)
    //{
    //  Node<T> previous = start;
    //  Node<T> node = Traverse(previous, comparison);

    //  node.Back = previous;

    //  if (node == previous)
    //  {
    //    Node<T> addition = new Node<T>();
    //    addition.Back = node;
    //    addition.Data = data;

    //    int dir = comparison(node.Data.Value);

    //    if (dir == -1)
    //    {
    //      node.Left = addition;
    //    }
    //    else if (dir == 1)
    //    {
    //      node.Right = addition;
    //    }
    //  }
    //  else
    //  {

    //  }
    //}

    private static Node<T> Traverse(Node<T> me, TraverseComparison comparison, ref int searches)
    {
      if (me.Data == null)
      {
        return me;
      }

      int dir = comparison(me.Data);

      searches++;

      if (dir == 0)
      {
        return me;
      }
      else if (dir < 0)
      {
        return Traverse(me.Left, comparison, ref searches);
      }
      else
      {
        return Traverse(me.Right, comparison, ref searches);
      }
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
