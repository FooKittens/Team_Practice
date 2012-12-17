using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teamcollab.Engine.Helpers
{
  public class BSPTree<T>
  {
    #region Properties

    #endregion

    #region Members

    #endregion

    public BSPTree()
    {

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
