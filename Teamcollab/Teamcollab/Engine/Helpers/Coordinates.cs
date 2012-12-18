using Microsoft.Xna.Framework;
namespace Teamcollab.Engine.Helpers
{
  /// <summary>
  /// Two integers x,y representing coordinates
  /// </summary>
  public struct Coordinates
  {

    #region Static Caching
    private static readonly Coordinates zero = new Coordinates(0, 0); 
    #endregion

    public int X;
    public int Y;

    public Coordinates(int x, int y)
    {
      X = x;
      Y = y;
    }

    public static Coordinates Zero { get { return zero; } }

    public static Coordinates operator +(Coordinates lhs, Coordinates rhs)
    {
      lhs.X += rhs.X;
      lhs.Y += rhs.Y;
      return lhs;
    }

    public static Coordinates operator -(Coordinates lhs, Coordinates rhs)
    {
      lhs.X -= rhs.X;
      lhs.Y -= rhs.Y;
      return lhs;
    }

    public static Coordinates operator *(Coordinates lhs, Coordinates rhs)
    {
      lhs.X *= rhs.X;
      lhs.Y *= rhs.Y;
      return lhs;
    }

    public static Coordinates operator /(Coordinates lhs, Coordinates rhs)
    {
      lhs.X /= rhs.X;
      lhs.Y /= rhs.Y;
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
      if (lhs.X == rhs.X && lhs.Y == rhs.Y)
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
      return string.Format("X: {0}, Y: {1}", X, Y);
    }

    public static implicit operator Vector2(Coordinates c)
    {
      return new Vector2(c.X, c.Y);
    }

    public static implicit operator Point(Coordinates c)
    {
      return new Point(c.X, c.Y);
    }
  }
}