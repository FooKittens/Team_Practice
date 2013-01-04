using System;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
namespace Midgard.Engine.Helpers
{
  /// <summary>
  /// Two integers x,y representing coordinates
  /// </summary>
  [Serializable]
  public struct Point2D
  {
    public static Point2D Zero { get { return zero; } }

    public int X { get { return x; } set { x = value; } }
    public int Y { get { return y; } set { y = value; } }

    #region Static Caching
    private static readonly Point2D zero = new Point2D(0, 0); 
    #endregion

    private int x;
    private int y;

    public Point2D(int x, int y)
    {
      this.x = x;
      this.y = y;
    }

    public static Point2D operator +(Point2D lhs, Point2D rhs)
    {
      lhs.x += rhs.x;
      lhs.y += rhs.y;
      return lhs;
    }

    public static Point2D operator -(Point2D lhs, Point2D rhs)
    {
      lhs.x -= rhs.x;
      lhs.y -= rhs.y;
      return lhs;
    }

    public static Point2D operator *(Point2D lhs, Point2D rhs)
    {
      lhs.x *= rhs.x;
      lhs.y *= rhs.y;
      return lhs;
    }

    public static Point2D operator /(Point2D lhs, Point2D rhs)
    {
      lhs.x /= rhs.x;
      lhs.y /= rhs.y;
      return lhs;
    }

    public static bool operator ==(Point2D lhs, Point2D rhs)
    {
      return Equals(lhs, rhs);
    }

    public static bool operator !=(Point2D lhs, Point2D rhs)
    {
      return !Equals(lhs, rhs);
    }

    public override bool Equals(object obj)
    {
      if (obj is Point2D)
      {
        Equals(this, (Point2D)obj);
      }

      return false;
    }

    private static bool Equals(Point2D lhs, Point2D rhs)
    {
      if (lhs.x == rhs.x && lhs.y == rhs.y)
      {
        return true;
      }

      return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("x: {0}, y: {1}", x, y);
    }

    public static implicit operator Microsoft.Xna.Framework.Vector2(Point2D c)
    {
      return new Vector2(c.x, c.y);
    }

    public static implicit operator Microsoft.Xna.Framework.Point(Point2D c)
    {
      return new Point(c.x, c.y);
    }
  }
}