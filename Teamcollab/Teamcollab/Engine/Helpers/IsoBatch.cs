using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Teamcollab.GUI;

namespace Teamcollab.Engine.Helpers
{
  public class IsoBatch : SpriteBatch
  {
    static Matrix Isometric =
        Matrix.CreateRotationZ(MathHelper.PiOver4) *
        Matrix.CreateScale((float)Math.Sqrt(2f) / 2.0f, (float)Math.Sqrt(2f) / 4.0f, 1);

    static Matrix ScreenScale =
      Matrix.CreateScale(Constants.TileWidth, Constants.TileHeight, 1f);

    static Matrix IsoScreen = Isometric * ScreenScale;

    public IsoBatch(GraphicsDevice device)
      : base(device)
    { }


    public new void Draw(Texture2D text, Vector2 position,
      Rectangle? source, Color color, float rot, Vector2 origin,
      float scale, SpriteEffects effects, float layer)
    {
      position = Vector2.Transform(position, IsoScreen);
      base.Draw(text, position, source, color, rot, origin, scale, effects, layer);
    }

  }
}
