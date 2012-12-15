using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Teamcollab.Engine
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
    /// Returns true if any new key began being pressed this frame
    /// </summary>
    public static bool KeyAnyNewDown()
    {
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
    public static bool Key_NewDown(Keys key)
    {
      if (keys.IsKeyDown(key) && prevKeys.IsKeyUp(key))
      {
        return true;
      }
      return false;
    }

    /// <summary>
    /// Returns true if the given key is pressed
    /// </summary>
    public static bool Key_Down(Keys key)
    {
      if (keys.IsKeyDown(key))
      {
        return true;
      }
      return false;
    }

    /// <summary>
    /// Returns true if the given key began being released this frame
    /// </summary>
    public static bool Key_Release(Keys key)
    {
      if (keys.IsKeyUp(key) && prevKeys.IsKeyDown(key))
      {
        return true;
      }
      return false;
    }
    #endregion

    #region Mouse
    /// <summary>
    /// Returns true if the left mouse button is pressed
    /// </summary>
    public static bool Mouse_LeftDown()
    {
      return mouse.LeftButton == ButtonState.Pressed;
    }

    /// <summary>
    /// Returns true if the left mouse button began being pressed this frame
    /// </summary>
    public static bool Mouse_LeftNewDown()
    {
      return mouse.LeftButton == ButtonState.Pressed &&
             prevMouse.LeftButton == ButtonState.Released;
    }

    /// <summary>
    /// Returns true if the left mouse button began being released this frame
    /// </summary>
    public static bool Mouse_LeftNewUp()
    {
      return mouse.LeftButton == ButtonState.Released &&
             prevMouse.LeftButton == ButtonState.Pressed;
    }

    /// <summary>
    /// Returns true if the right mouse button is pressed
    /// </summary>
    public static bool Mouse_RightDown()
    {
      return mouse.RightButton == ButtonState.Pressed;
    }

    /// <summary>
    /// Returns true if the right mouse button began being pressed this frame
    /// </summary>
    public static bool Mouse_RightNewDown()
    {
      return mouse.RightButton == ButtonState.Pressed &&
             prevMouse.RightButton == ButtonState.Released;
    }

    /// <summary>
    /// Returns true if the right mouse button began being released this frame
    /// </summary>
    public static bool Mouse_RightNewUp()
    {
      return mouse.RightButton == ButtonState.Released &&
             prevMouse.RightButton == ButtonState.Pressed;
    }

    /// <summary>
    /// Returns the mouse position in screen coordinates
    /// </summary>
    public static Vector2 Mouse_Position()
    {
      return new Vector2(mouse.X, mouse.Y);
    }

    /// <summary>
    /// Returns the previous mouse position in screen coordinates
    /// </summary>
    public static Vector2 Mouse_PreviousPosition()
    {
      return new Vector2(prevMouse.X, prevMouse.Y);
    }
    #endregion

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
  }
}
