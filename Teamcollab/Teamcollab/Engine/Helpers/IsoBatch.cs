using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Engine.WorldManagement;

namespace Teamcollab.Engine.Helpers
{
  public class IsoBatch : SpriteBatch
  {
    static Matrix ScreenScale =
      Matrix.CreateScale(Constants.TileWidth, Constants.TileHeight, 1f);

    public IsoBatch(GraphicsDevice device)
      : base(device)
    { }

    public new void Draw(Texture2D tex, Vector2 pos,
      Rectangle? source, Color color, float rot, Vector2 origin,
      float scale, SpriteEffects effects, float layer)
    {
      pos = WorldManager.TransformIsometric(pos);
      pos = WorldManager.GetTileScreenPosition(pos);
      base.Draw(tex, pos, source, color, rot, origin, scale, effects, layer);
    }
  }
}
