using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Engine.WorldManagement;
using System;

namespace Teamcollab.Engine.Helpers
{
  public class IsoBatch : SpriteBatch
  {
    static Matrix ScreenScale =
      Matrix.CreateScale(Constants.TileWidth, Constants.TileHeight, 1f);

    public IsoBatch(GraphicsDevice device)
      : base(device)
    { }

    #region Vector2 Drawing
    public void Draw(Texture2D tex, Vector2 pos)
    {
      Vector2 origin = new Vector2(tex.Width / 2, tex.Height / 2);
      Draw(tex, pos, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
    }

    public new void Draw(Texture2D tex, Vector2 pos, Color color)
    {
      Vector2 origin = new Vector2(tex.Width / 2, tex.Height / 2);
      Draw(tex, pos, null, color, 0, origin, 1, SpriteEffects.None, 0);
    }

    public new void Draw(Texture2D tex, Vector2 pos, Rectangle? source,
      Color color)
    {
      Vector2 origin = new Vector2(tex.Width / 2, tex.Height / 2);
      Draw(tex, pos, source, color, 0, origin, 1, SpriteEffects.None, 0);
    }

    public new void Draw(Texture2D tex, Vector2 pos, Rectangle? source,
      Color color, float rot, Vector2 origin, Vector2 scale,
      SpriteEffects effects, float layer)
    {
      pos = WorldManager.TransformIsometric(pos);
      pos = WorldManager.GetTileScreenPosition(pos);
      base.Draw(tex, pos, source, color, rot, origin, scale, effects, layer);
    }

    public new void Draw(Texture2D tex, Vector2 pos,
      Rectangle? source, Color color, float rot, Vector2 origin,
      float scale, SpriteEffects effects, float layer)
    {
      pos = WorldManager.TransformIsometric(pos);
      pos = WorldManager.TransformWorldToScreen(pos);
      base.Draw(tex, pos, source, color, rot, origin, scale, effects, layer);
    }
    #endregion

    #region Destination Rectangle Drawing
    public new void Draw(Texture2D tex, Rectangle dest, Color color)
    {
      throw new NotImplementedException();
    }

    public new void Draw(Texture2D tex, Rectangle dest, Rectangle? source,
      Color color)
    {
      throw new NotImplementedException();
    }

    public new void Draw(Texture2D tex, Rectangle dest, Rectangle? source,
      Color color, float rot, Vector2 origin, SpriteEffects effects,
      float layer)
    {
      throw new NotImplementedException();
    }
    #endregion
  }
}
