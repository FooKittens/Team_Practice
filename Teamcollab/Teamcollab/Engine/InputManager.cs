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

    /// <summary>Returns true if any new key began being pressed this frame</summary>
    public static bool Key_AnyNewKey()
    {
      if (keys.GetPressedKeys().Length > 0 &&
        prevKeys.GetPressedKeys().Length == 0)
        return true;
      return false;
    }

    /// <summary>Returns true if the given key began being pressed this frame</summary>
    public static bool Key_NewDown(Keys key)
    {
      if (keys.IsKeyDown(key) && prevKeys.IsKeyUp(key))
        return true;
      return false;
    }

    /// <summary>Returns true if the given key is pressed</summary>
    public static bool Key_Down(Keys key)
    {
      if (keys.IsKeyDown(key))
        return true;
      return false;
    }

    /// <summary>Returns true if the given key began being released this frame</summary>
    public static bool Key_Release(Keys key)
    {
      if (keys.IsKeyUp(key) && prevKeys.IsKeyDown(key))
        return true;
      return false;
    }

    #endregion

    #region Mouse

    /// <summary>Returns true if the left mouse button is pressed</summary>
    public static bool Mouse_LeftDown()
    {
      return mouse.LeftButton == ButtonState.Pressed;
    }

    /// <summary>Returns true if the left mouse button began being pressed this frame</summary>
    public static bool Mouse_LeftNewDown()
    {
      return mouse.LeftButton == ButtonState.Pressed &&
        prevMouse.LeftButton == ButtonState.Released;
    }

    /// <summary>Returns true if the left mouse button began being released this frame</summary>
    public static bool Mouse_LeftNewUp()
    {
      return mouse.LeftButton == ButtonState.Released &&
        prevMouse.LeftButton == ButtonState.Pressed;
    }

    /// <summary>Returns true if the right mouse button is pressed</summary>
    public static bool Mouse_RightDown()
    {
      return mouse.RightButton == ButtonState.Pressed;
    }

    /// <summary>Returns true if the right mouse button began being pressed this frame</summary>
    public static bool Mouse_RightNewDown()
    {
      return mouse.RightButton == ButtonState.Pressed &&
        prevMouse.RightButton == ButtonState.Released;
    }

    /// <summary>Returns true if the right mouse button began being released this frame</summary>
    public static bool Mouse_RightNewUp()
    {
      return mouse.RightButton == ButtonState.Released &&
        prevMouse.RightButton == ButtonState.Pressed;
    }

    /// <summary>Returns the mouse position in screen coordinates</summary>
    public static Vector2 Mouse_Position()
    {
      return new Vector2(mouse.X, mouse.Y);
    }

    /// <summary>Returns the previous mouse position in screen coordinates</summary>
    public static Vector2 Mouse_PreviousPosition()
    {
      return new Vector2(prevMouse.X, prevMouse.Y);
    }

    #endregion
  }
}
