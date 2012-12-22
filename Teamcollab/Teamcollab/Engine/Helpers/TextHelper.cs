using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    #region Regular Expressions
    static readonly string[] Vector2Matches = { "[0-9]*, [0-9]*", "([0-9]*, [0-9]*)",
                                                "[0-9]*,[0-9]*", "([0-9]*,[0-9]*)" };


    #endregion


    public static Vector2 ParseVector2(string text)
    {
      foreach (string regex in Vector2Matches)
      {
        Match match = Regex.Match(text, regex);
        if (match.Success)
        {
          string res = match.Value;
          DevConsole.WriteLine(res);
        }
      }
      return Vector2.Zero;
    }


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
