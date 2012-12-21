using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Teamcollab.Engine
{
  static class DevConsole
  {
    #region Properties
    public static bool Visible { get; set; }
    #endregion

    #region Member
    static bool initialized;
    static SpriteFont devFont;
    static SpriteBatch spriteBatch;
    static Texture2D pixel;
    static List<string> textRows;
    #endregion

    public static void Initialize(Game game)
    {
      DevConsole.devFont = game.Content.Load<SpriteFont>("Fonts\\ImmediateDrawerFont");
      spriteBatch = new SpriteBatch(game.GraphicsDevice);
      pixel = new Texture2D(game.GraphicsDevice, 1, 1);
      pixel.SetData<Color>(new[] { Color.White });
      textRows = new List<string>();

      initialized = true;
    }

    public static void Update(GameTime gameTime)
    {

    }

    public static void Draw()
    {
      if (!Visible)
      {
        return;
      }

      const int consoleMaxLines = 15;

      spriteBatch.Begin();

      // TODO(Peter): Remove
      // Draw an alpha blended rectangle behind the text.
      spriteBatch.Draw(
        pixel,
        new Rectangle(0, 0, Settings.ScreenWidth, consoleMaxLines * devFont.LineSpacing),
        Color.Black * 0.45f
      );

      Vector2 textOffset = Vector2.Zero;
      int startRow = textRows.Count <  consoleMaxLines ? 0 : textRows.Count - consoleMaxLines;
      for(int i = startRow; i < textRows.Count; ++i)
      {
        spriteBatch.DrawString(devFont, textRows[i], textOffset, Color.White);
        textOffset.Y += devFont.LineSpacing;
      }
      spriteBatch.End();
    }

    public static void WriteLine(string formatString, params object[] args)
    {
      textRows.Add(string.Format(formatString, args));
    }

    private static void RecieveCommand(string command)
    {

    }
  }
}
