using System;
using Microsoft.Xna.Framework;
namespace Midgard.Engine.Helpers
{
  /// <summary>
  /// Two integers x,y representing coordinates
  /// </summary>
  [Serializable]
  public struct Coordinates
  {
    public static Coordinates Zero { get { return zero; } }

    public int X { get { return x; } set { x = value; } }
    public int Y { get { return y; } set { y = value; } }

    #region Static Caching
    private static readonly Coordinates zero = new Coordinates(0, 0); 
    #endregion

    public int x;
    public int y;

    public Coordinates(int x, int y)
    {
      this.x = x;
      this.y = y;
    }

    public static Coordinates operator +(Coordinates lhs, Coordinates rhs)
    {
      lhs.x += rhs.x;
      lhs.y += rhs.y;
      return lhs;
    }

    public static Coordinates operator -(Coordinates lhs, Coordinates rhs)
    {
      lhs.x -= rhs.x;
      lhs.y -= rhs.y;
      return lhs;
    }

    public static Coordinates operator *(Coordinates lhs, Coordinates rhs)
    {
      lhs.x *= rhs.x;
      lhs.y *= rhs.y;
      return lhs;
    }

    public static Coordinates operator /(Coordinates lhs, Coordinates rhs)
    {
      lhs.x /= rhs.x;
      lhs.y /= rhs.y;
      return lhs;
    }

    public static bool operator ==(Coordinates lhs, Coordinates rhs)
    {
      return Equals(lhs, rhs);
    }

    public static bool operator !=(Coordinates lhs, Coordinates rhs)
    {
      return !Equals(lhs, rhs);
    }

    public override bool Equals(object obj)
    {
      if (obj is Coordinates)
      {
        Equals(this, (Coordinates)obj);
      }

      return false;
    }

    private static bool Equals(Coordinates lhs, Coordinates rhs)
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

    public static implicit operator Vector2(Coordinates c)
    {
      return new Vector2(c.x, c.y);
    }

    public static implicit operator Point(Coordinates c)
    {
      return new Point(c.x, c.y);
    }
  }
}