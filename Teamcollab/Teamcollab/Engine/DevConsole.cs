using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Teamcollab.Engine.Helpers;
using Microsoft.Xna.Framework.Input;

namespace Teamcollab.Engine
{
  static class DevConsole
  {
    #region Properties
    public static bool Visible { get; set; }
    public static bool IsCheckingForKeys { get; private set; }
    #endregion

    #region Member
    static bool initialized;
    static SpriteFont devFont;
    static SpriteBatch spriteBatch;
    static Texture2D pixel;
    static List<string> textRows;
    static string currentWrittenLine;
    static bool previouslyVisible;
    static Color color;
    static TimeSpan caretTimer;
    static bool showCaret;
    static int inputLineY;
    #endregion

    #region Constants
    const int ConsoleMaxLines = 15;
    static readonly TimeSpan CaretTickTime = TimeSpan.FromMilliseconds(125);
    static readonly Color DefaultTextColor = new Color(255, 205, 139);
    static readonly Color DefaultBackground = 
      new Color(0.125f, 0.125f, 0.19f, 0.95f);
    static readonly Color DefaultInputBarColor =
      new Color(0.1f, 0.1f, 0.125f, 0.9f);
    #endregion

    public static void Initialize(Game game)
    {
      DevConsole.devFont = game.Content.Load<SpriteFont>(
        "Fonts\\ImmediateDrawerFont"
      );
      spriteBatch = new SpriteBatch(game.GraphicsDevice);
      pixel = new Texture2D(game.GraphicsDevice, 1, 1);
      pixel.SetData<Color>(new[] { Color.White } );
      textRows = new List<string>();
      currentWrittenLine = "";
      previouslyVisible = false;
      color = DefaultTextColor;
      IsCheckingForKeys = false;
      inputLineY = (ConsoleMaxLines - 1) * devFont.LineSpacing;

      // Set when all members are initialized.
      initialized = true;
    }

    public static void Update(GameTime gameTime)
    {
      if (previouslyVisible)
      {
        IsCheckingForKeys = true;

        currentWrittenLine += GetInputString();

        #region Slash, Enter, Back, Escape
        if ((InputManager.KeyDown(Keys.LeftShift) ||
          InputManager.KeyDown(Keys.RightShift)) &&
          InputManager.KeyRelease(Keys.D7))
        {
          currentWrittenLine += '/';
        }

        if (InputManager.KeyRelease(Keys.Enter))
        {
          if (currentWrittenLine != "")
          {
            if (currentWrittenLine.StartsWith("/"))
            {
              currentWrittenLine = currentWrittenLine.Remove(0, 1);
              RecieveCommand(currentWrittenLine);
            }
            else
            {
              WriteLine(currentWrittenLine);
            }

            currentWrittenLine = "";
          }
        }

        if (InputManager.KeyRelease(Keys.Back))
        {
          if (currentWrittenLine.Length > 0)
          {
            currentWrittenLine = currentWrittenLine.Remove(
              currentWrittenLine.Length - 1
            );
          }
        }

        if (InputManager.KeyRelease(Keys.Escape))
        {
          Visible = false;
        }
        #endregion
      }

      previouslyVisible = Visible;
      IsCheckingForKeys = false;

      // Update caret vars.
      caretTimer += gameTime.ElapsedGameTime;
      if (caretTimer > CaretTickTime)
      {
        caretTimer = TimeSpan.Zero;
        showCaret = !showCaret;
      }
    }

    public static void Draw()
    {
      if (Visible == false)
      {
        return;
      }

      spriteBatch.Begin();

      // TODO(Peter): Remove
      // Draw an alpha blended rectangle behind the text.
      spriteBatch.Draw(
        pixel,
        new Rectangle(0, 0, Settings.ScreenWidth,
          ConsoleMaxLines * devFont.LineSpacing
        ),
        DefaultBackground
      );

      Vector2 textOffset = Vector2.Zero;
      int startRow = textRows.Count < ConsoleMaxLines ?
        0 : textRows.Count - ConsoleMaxLines + 1 // + 1 for write line
      ;
      for(int i = startRow; i < textRows.Count; ++i)
      {
        spriteBatch.DrawString(devFont, textRows[i], textOffset, color);
        textOffset.Y += devFont.LineSpacing;
      }

      DrawInputBar();

      spriteBatch.End();
    }

    public static void WriteLine(string formatString, params object[] args)
    {
      textRows.Add(string.Format(formatString, args));
    }

    private static void RecieveCommand(string command)
    {
      if (command.StartsWith("help"))
      {
        #region /help
        WriteLine("Available commands:");
        WriteLine("/help - display this");
        WriteLine("/color [COLOR] - change the color of the console text");
        #endregion
      }
      else if (command.StartsWith("color"))
      {
        #region /color
        if (command.EndsWith("blue"))
          color = Color.Blue;
        else if (command.EndsWith("red"))
          color = Color.Red;
        else if (command.EndsWith("green"))
          color = Color.Green;
        else if (command.EndsWith("yellow"))
          color = Color.Yellow;
        else if (command.EndsWith("orange"))
          color = Color.Orange;
        else if (command.EndsWith("purple"))
          color = Color.Purple;
        else if (command.EndsWith("white"))
          color = Color.White;
        else if (command.EndsWith("black"))
          color = Color.Black;
        else if (command.EndsWith("default"))
          color = DefaultTextColor;
        else
          WriteLine("Not a valid color. Correct usage is /color [COLOR]");
        #endregion
      }
      else
        WriteLine("{0} is not a valid command", command);
    }

    private static string GetInputString()
    {
      string input = "";
      if (InputManager.KeyNewDown(Keys.A))
        input = "a";
      else if (InputManager.KeyNewDown(Keys.B))
        input = "b";
      else if (InputManager.KeyNewDown(Keys.C))
        input = "c";
      else if (InputManager.KeyNewDown(Keys.D))
        input = "d";
      else if (InputManager.KeyNewDown(Keys.E))
        input = "e";
      else if (InputManager.KeyNewDown(Keys.F))
        input = "f";
      else if (InputManager.KeyNewDown(Keys.G))
        input = "g";
      else if (InputManager.KeyNewDown(Keys.H))
        input = "h";
      else if (InputManager.KeyNewDown(Keys.I))
        input = "i";
      else if (InputManager.KeyNewDown(Keys.J))
        input = "j";
      else if (InputManager.KeyNewDown(Keys.K))
        input = "k";
      else if (InputManager.KeyNewDown(Keys.L))
        input = "l";
      else if (InputManager.KeyNewDown(Keys.M))
        input = "m";
      else if (InputManager.KeyNewDown(Keys.N))
        input = "n";
      else if (InputManager.KeyNewDown(Keys.O))
        input = "o";
      else if (InputManager.KeyNewDown(Keys.P))
        input = "p";
      else if (InputManager.KeyNewDown(Keys.Q))
        input = "q";
      else if (InputManager.KeyNewDown(Keys.R))
        input = "r";
      else if (InputManager.KeyNewDown(Keys.S))
        input = "s";
      else if (InputManager.KeyNewDown(Keys.T))
        input = "t";
      else if (InputManager.KeyNewDown(Keys.U))
        input = "u";
      else if (InputManager.KeyNewDown(Keys.V))
        input = "v";
      else if (InputManager.KeyNewDown(Keys.W))
        input = "w";
      else if (InputManager.KeyNewDown(Keys.X))
        input = "x";
      else if (InputManager.KeyNewDown(Keys.Y))
        input = "y";
      else if (InputManager.KeyNewDown(Keys.Z))
        input = "z";
      else if (InputManager.KeyNewDown(Keys.Space))
        input = " ";
      else if (InputManager.KeyNewDown(Keys.OemEnlW))
        input = "'";
      if (InputManager.KeyDown(Keys.LeftShift) ||
        InputManager.KeyDown(Keys.RightShift))
      {
        input = input.ToUpper();
      }

      return input;
    }

    private static void DrawInputBar()
    {     
      spriteBatch.Draw(
        pixel,
        new Rectangle(0, inputLineY, Settings.ScreenWidth, devFont.LineSpacing),
        DefaultInputBarColor
      );

      // Separator line
      spriteBatch.Draw(pixel, new Rectangle(0, inputLineY, Settings.ScreenWidth, 2), Color.Red);

      spriteBatch.DrawString(
        devFont,
        currentWrittenLine,
        new Vector2(0, inputLineY),
        color
      );

      if (showCaret)
      {
        Vector2 caretPos = new Vector2(
          devFont.MeasureString(currentWrittenLine).X, inputLineY
        );

        spriteBatch.DrawString(devFont, "|", caretPos, color);
      }
    }
  }
}
