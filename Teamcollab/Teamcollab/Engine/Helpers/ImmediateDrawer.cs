using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Teamcollab.Engine.Helpers
{
  class ImmediateDrawer
  {
    #region Properites

    #endregion

    #region Members
    SpriteBatch spriteBatch;
    List<Tuple<Rectangle, Color>> drawRects;
    List<Tuple<string, Vector2>> drawStrings;
    #endregion

    static ImmediateDrawer singleton;
    static Texture2D pixel;
    static SpriteFont font;

    private ImmediateDrawer(Game game = null)
    {
      spriteBatch = new SpriteBatch(game.GraphicsDevice);
      drawRects = new List<Tuple<Rectangle, Color>>();
      drawStrings = new List<Tuple<string, Vector2>>();
      pixel = new Texture2D(game.GraphicsDevice, 1, 1);
      pixel.SetData<Color>(new[] { Color.White });
      font = game.Content.Load<SpriteFont>("Fonts\\ImmediateDrawerFont");
    }

    public static ImmediateDrawer GetInstance(Game game)
    {
      if (singleton == null)
      {
        singleton = new ImmediateDrawer(game);
      }

      return singleton;
    }

    public void Draw()
    {
      spriteBatch.Begin();

      foreach (Tuple<Rectangle, Color> rect in drawRects)
      {
        spriteBatch.Draw(pixel, rect.Item1, rect.Item2);
      }

      foreach(Tuple<string, Vector2> tuple in drawStrings)
      {
        spriteBatch.DrawString(font, tuple.Item1, tuple.Item2, Color.White);
      }

      spriteBatch.End();

      drawRects.Clear();
      drawStrings.Clear();
    }

    public void DrawRectangle(Rectangle rectangle, Color color)
    {
      drawRects.Add(new Tuple<Rectangle, Color>(rectangle, color));
    }

    public void DrawRectangle(int left, int right, int top, int btm, Color clr)
    {
      DrawRectangle(new Rectangle(left, top, right - left, btm - top), clr);
    }

    public void DrawString(string text, Vector2 position)
    {
      drawStrings.Add(new Tuple<string, Vector2>(text, position));
    }
  }
}
