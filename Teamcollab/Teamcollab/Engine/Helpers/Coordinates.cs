namespace Teamcollab.Engine.Helpers
{
  /// <summary>
  /// Two integers x,y representing coordinates
  /// </summary>
  struct Coordinates
  {
    public int X;
    public int Y;

    public Coordinates(int x, int y)
    {
      X = x;
      Y = y;
    }

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
  }
}