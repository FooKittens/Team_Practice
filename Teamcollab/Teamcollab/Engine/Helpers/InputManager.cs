using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Teamcollab.Engine.Helpers
{
  /// <summary>
  /// Handles input.
  /// Note that Update() needs to be called
  /// every frame to keep up with input changes.
  /// </summary>
  public static class InputManager
  {
    #region Members
    private static KeyboardState keys;
    private static KeyboardState prevKeys;
    private static MouseState mouse;
    private static MouseState prevMouse;
    #endregion

    /// <summary>
    /// Needs to called every frame to keep the class updated
    /// </summary>
    public static void Update()
    {
      prevKeys = keys;
      prevMouse = mouse;
      keys = Keyboard.GetState();
      mouse = Mouse.GetState();
    }

    #region Keyboard
    /// <summary>
    /// Returns all currently pressed keys.
    /// </summary>
    /// <returns></returns>
    public static Keys[] GetCurrentKeys()
    {
      return keys.GetPressedKeys();
    }

    /// <summary>
    /// Returns true if any new key began being pressed this frame
    /// </summary>
    public static bool KeyAnyNewDown()
    {
      if (DevConsole.Visible && !DevConsole.IsCheckingForKeys)
      {
        return false;
      }

      Keys[] newPressed = keys.GetPressedKeys();
      Keys[] prevPressed = prevKeys.GetPressedKeys();

      if (newPressed.Length > prevPressed.Length)
      {
        return true;
      }

      /* Handles the edge case where one key was released at the same time
       * as another was pressed. */
      foreach (Keys nk in newPressed)
      {
        if (FoundKey(nk, prevPressed))
        {
          continue;
        }

        return true;
      }

      return false;
    }

    /// <summary>
    /// Returns true if the given key began being pressed this frame
    /// </summary>
    public static bool KeyNewDown(Keys key)
    {
      if (DevConsole.Visible && !DevConsole.IsCheckingForKeys)
      {
        return false;
      }

      if (keys.IsKeyDown(key) && prevKeys.IsKeyUp(key))
      {
        return true;
      }
      return false;
    }

    /// <summary>
    /// Returns true if the given key is pressed
    /// </summary>
    public static bool KeyDown(Keys key)
    {
      if (DevConsole.Visible && !DevConsole.IsCheckingForKeys)
      {
        return false;
      }

      if (keys.IsKeyDown(key))
      {
        return true;
      }
      return false;
    }

    /// <summary>
    /// Returns true if the given key began being released this frame
    /// </summary>
    public static bool KeyRelease(Keys key)
    {
      if (DevConsole.Visible && !DevConsole.IsCheckingForKeys)
      {
        return false;
      }

      if (keys.IsKeyUp(key) && prevKeys.IsKeyDown(key))
      {
        return true;
      }
      return false;
    }

    #region Helpers
    /// <summary>
    /// Checks the input collection for an occurence of the parameter key.
    /// </summary>
    /// <param name="key">The key to search for.</param>
    /// <param name="collection">The collection to look in.</param>
    /// <returns>Can't see this :(</returns>
    private static bool FoundKey(Keys key, Keys[] collection)
    {
      foreach (Keys k in collection)
      {
        if (key == k)
        {
          return true;
        }
      }

      return false;
    }
    #endregion
    #endregion

    #region Mouse
    /// <summary>
    /// Returns true if the left mouse button is pressed
    /// </summary>
    public static bool MouseLeftDown()
    {
      return mouse.LeftButton == ButtonState.Pressed;
    }

    /// <summary>
    /// Returns the cumulative mouse wheel value since the game was started
    /// </summary>
    public static int MouseWheelTotal()
    {
      return mouse.ScrollWheelValue;
    }

    /// <summary>
    /// Returns the wheel change since previous frame
    /// </summary>
    public static int MouseWheelChange()
    {
      return mouse.ScrollWheelValue - prevMouse.ScrollWheelValue;
    }

    /// <summary>
    /// Returns true if the left mouse button began being pressed this frame
    /// </summary>
    public static bool MouseLeftNewDown()
    {
      return mouse.LeftButton == ButtonState.Pressed &&
             prevMouse.LeftButton == ButtonState.Released;
    }

    /// <summary>
    /// Returns true if the left mouse button began being released this frame
    /// </summary>
    public static bool MouseLeftNewUp()
    {
      return mouse.LeftButton == ButtonState.Released &&
             prevMouse.LeftButton == ButtonState.Pressed;
    }

    /// <summary>
    /// Returns true if the right mouse button is pressed
    /// </summary>
    public static bool MouseRightDown()
    {
      return mouse.RightButton == ButtonState.Pressed;
    }

    /// <summary>
    /// Returns true if the right mouse button began being pressed this frame
    /// </summary>
    public static bool MouseRightNewDown()
    {
      return mouse.RightButton == ButtonState.Pressed &&
             prevMouse.RightButton == ButtonState.Released;
    }

    /// <summary>
    /// Returns true if the right mouse button began being released this frame
    /// </summary>
    public static bool MouseRightNewUp()
    {
      return mouse.RightButton == ButtonState.Released &&
             prevMouse.RightButton == ButtonState.Pressed;
    }

    /// <summary>
    /// Returns the mouse position in screen coordinates
    /// </summary>
    public static Vector2 MousePosition()
    {
      return new Vector2(mouse.X, mouse.Y);
    }

    /// <summary>
    /// Returns the previous mouse position in screen coordinates
    /// </summary>
    public static Vector2 MousePreviousPosition()
    {
      return new Vector2(prevMouse.X, prevMouse.Y);
    }
    #endregion
  }
}
