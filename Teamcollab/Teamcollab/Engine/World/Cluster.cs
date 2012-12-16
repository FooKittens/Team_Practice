using Teamcollab.Engine.Helpers;

namespace Teamcollab.Engine.World
{

  public enum ClusterType : byte
  {
    Undefined = 0,

    Evergreen,
  }

  /// <summary>
  /// Handles a collection of Tiles
  /// </summary>
  struct Cluster
  {
    public ClusterType Type;
    public Tile[] Tiles;
    public Coordinates Coordinates;
    public bool Active;

    public Cluster(ClusterType type, Coordinates coordinates)
    {
      Type = type;
      Tiles = new Tile[Constants.ClusterWidth * Constants.ClusterHeight];
      Active = false;
      Coordinates = coordinates;
    }

    public Tile this[int index]
    {
      get
      {
        return Tiles[index];
      }
      set
      {
        Tiles[index] = value;
      }
    }

    public Tile this[Coordinates coord]
    {
      get
      {
        return Tiles[coord.X * coord.Y + coord.X];
      }
      set
      {
        Tiles[coord.X * coord.Y + coord.X] = value;
      }
    }
  }
}