using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Teamcollab.Engine.Helpers
{
  static class TextHelper
  {
    #region Properties
    public static bool AnnounceUnknownKeys { get; set; }
    #endregion

    // For avoiding swedish number formats, since they're confusing.
    static readonly NumberFormatInfo NumberFormat =
      new CultureInfo("en-US", false).NumberFormat;

    #region Regular Expressions
    // Regexes for Vector2's - Please kill me.
    static readonly string Vector2Match = @"((?<X>[0-9]*[.]?[0-9]*)\s*,\s*(?<Y>[0-9]*[.]?[0-9]*))";

    static readonly string[] FloatMatches = { "[0-9*].[0-9*]", "[0-9*]", ".[0-9*]" };
    //static readonly string IntMatch = "[0-9*]"; //TODO(Martin): Use this at all?
    #endregion

    #region Parsers
    /// <summary>
    /// Parses a text for a valid Vector2.
    /// Example: (1, 2) or (1.25, 2) or (1.1 , 1.1)
    /// </summary>
    /// <param name="text">String containg a valid vector2.</param>
    public static Vector2[] ParseVector2(string text)
    {
      MatchCollection matches = Regex.Matches(text, Vector2Match); 
       
      var vectors = new List<Vector2>();
      foreach(Match match in matches)
      {
        float x, y;

        bool succeeded = Single.TryParse(
          match.Groups["X"].Value,
          NumberStyles.Any,
          NumberFormat,
          out x
        );

        // & results together since both have to succeed.
        succeeded &= Single.TryParse(
          match.Groups["Y"].Value,
          NumberStyles.Any,
          NumberFormat,
          out y
        );

        if(succeeded)
        {
          Vector2 v = new Vector2(x, y);
          vectors.Add(v);
        }
      }

      return vectors.ToArray();
    }

    /// <summary>
    /// Parses a text and returns all integers found in the text.
    /// Numbers are separated with " " or ",".
    /// </summary>
    public static int[] ParseInts(string text)
    {
      var ints = new List<int>();
      string[] parts = text.Split(' ', ',');
      foreach (string part in parts)
      {
        int i;
        if (Int32.TryParse(part, NumberStyles.Any, NumberFormat, out i))
        {
          ints.Add(i);
        }
      }
      return ints.ToArray();
    }

    /// <summary>
    /// Parses a text and returns all single point precision numbers.
    /// Numbers are separated with " " or ",".
    /// </summary>
    public static float[] ParseFloats(string text)
    {
      string[] splits = text.Split(' ', ',');
      
      var floats = new List<float>();

      foreach (string part in splits)
      {
        float f;
        if (Single.TryParse(part, NumberStyles.Any, NumberFormat, out f))
        {
          floats.Add(f);
        }
      }

      return floats.ToArray();
    }
    #endregion

    public static string KeyToChar(Keys key)
    {
      string inputChar = "";
      switch (key)
      {
        case Keys.A:
          inputChar = "A";
          break;
        case Keys.Add:
          inputChar = "+";
          break;
        case Keys.B:
          inputChar = "B";
          break;
        case Keys.C:
          inputChar = "C";
          break;
        case Keys.D:
          inputChar = "D";
          break;
        case Keys.Decimal:
          inputChar = ".";
          break;
        case Keys.Divide:
          inputChar = "/";
          break;
        case Keys.E:
          inputChar = "E";
          break;
        case Keys.F:
          inputChar = "F";
          break;
        case Keys.G:
          inputChar = "G";
          break;
        case Keys.H:
          inputChar = "H";
          break;
        case Keys.I:
          inputChar = "I";
          break;
        case Keys.J:
          inputChar = "J";
          break;
        case Keys.K:
          inputChar = "K";
          break;
        case Keys.L:
          inputChar = "L";
          break;
        case Keys.M:
          inputChar = "M";
          break;
        case Keys.Multiply:
          inputChar = "*";
          break;
        case Keys.N:
          inputChar = "N";
          break;
        case Keys.O:
          inputChar = "O";
          break;
        case Keys.OemComma:
          inputChar = ",";
          break;
        case Keys.OemMinus:
          inputChar = "-";
          break;
        case Keys.OemPeriod:
          inputChar = ".";
          break;
        case Keys.OemPlus:
          inputChar = "+";
          break;
        case Keys.P:
          inputChar = "P";
          break;
        case Keys.Q:
          inputChar = "Q";
          break;
        case Keys.R:
          inputChar = "R";
          break;
        case Keys.S:
          inputChar = "S";
          break;
        case Keys.Space:
          inputChar = " ";
          break;
        case Keys.Subtract:
          inputChar = "-";
          break;
        case Keys.T:
          inputChar = "T";
          break;
        case Keys.U:
          inputChar = "U";
          break;
        case Keys.V:
          inputChar = "V";
          break;
        case Keys.W:
          inputChar = "W";
          break;
        case Keys.X:
          inputChar = "X";
          break;
        case Keys.Y:
          inputChar = "Y";
          break;
        case Keys.Z:
          inputChar = "Z";
          break;

        // Number keys
        case Keys.D1:
          if (ShiftDown())
          {
            inputChar = "!";
            break;
          }
          inputChar = "1";
          break;
        case Keys.D2:
          if (ShiftDown())
          {
            inputChar = "\"";
            break;
          }
          inputChar = "2";
          break;
        case Keys.D3:
          if (ShiftDown())
          {
            inputChar = "#";
            break;
          }
          inputChar = "3";
          break;
        case Keys.D4:
          if (ShiftDown())
          {
            inputChar = "¤";
            break;
          }
          inputChar = "4";
          break;
        case Keys.D5:
          if (ShiftDown())
          {
            inputChar = "%";
            break;
          }
          inputChar = "5";
          break;
        case Keys.D6:
          if (ShiftDown())
          {
            inputChar = "&";
            break;
          }
          inputChar = "6";
          break;
        case Keys.D7:
          if (ShiftDown())
          {
            inputChar = "/";
            break;
          }
          inputChar = "7";
          break;
        case Keys.D8:
          if (ShiftDown())
          {
            inputChar = "(";
            break;
          }
          inputChar = "8";
          break;
        case Keys.D9:
          if (ShiftDown())
          {
            inputChar = ")";
            break;
          }
          inputChar = "9";
          break;
        case Keys.D0:
          if (ShiftDown())
          {
            inputChar = "=";
            break;
          }
          inputChar = "0";
          break;
        default:
          if (AnnounceUnknownKeys)
          {
            DevConsole.WriteLine("Unknown Key with code [{0}] found.", key);
          }
          break;
      }

      return ShiftDown() ? inputChar : inputChar.ToLower();
    }

    private static bool ShiftDown()
    {
      return InputManager.KeyDown(Keys.LeftShift) ||
             InputManager.KeyDown(Keys.RightShift);
    }
  }
}
