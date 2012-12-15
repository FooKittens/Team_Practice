using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Teamcollab.Engine
{
  /// <summary>
  /// Handles... input!
  /// </summary>
  public sealed static class InputManager
  {
    #region Members
    private static KeyboardState keys;
    private static KeyboardState prevKeys;
    private static MouseState mouse;
    private static MouseState prevMouse;
    #endregion

    public static void Update()
    {
      prevKeys = keys;
      prevMouse = mouse;
      keys = Keyboard.GetState();
      mouse = Mouse.GetState();
    }

    #region Keyboard

    public static bool Key_AnyNewKey()
    {
      if (keys.GetPressedKeys().Length > 0 &&
        prevKeys.GetPressedKeys().Length == 0)
        return true;
      return false;
    }

    public static bool Key_NewDown(Keys key)
    {
      if (keys.IsKeyDown(key) && prevKeys.IsKeyUp(key))
        return true;
      return false;
    }

    public static bool Key_Down(Keys key)
    {
      if (keys.IsKeyDown(key))
        return true;
      return false;
    }

    public static bool Key_Release(Keys key)
    {
      if (keys.IsKeyUp(key) && prevKeys.IsKeyDown(key))
        return true;
      return false;
    }

    #endregion

    #region Mouse

    public static bool Mouse_LeftDown()
    {
      return mouse.LeftButton == ButtonState.Pressed;
    }

    public static bool Mouse_LeftNewDown()
    {
      return mouse.LeftButton == ButtonState.Pressed &&
        prevMouse.LeftButton == ButtonState.Released;
    }

    public static bool Mouse_LeftNewUp()
    {
      return mouse.LeftButton == ButtonState.Released &&
        prevMouse.LeftButton == ButtonState.Pressed;
    }

    public static bool Mouse_RightDown()
    {
      return mouse.RightButton == ButtonState.Pressed;
    }

    public static bool Mouse_RightNewDown()
    {
      return mouse.RightButton == ButtonState.Pressed &&
        prevMouse.RightButton == ButtonState.Released;
    }

    public static bool Mouse_RightNewUp()
    {
      return mouse.RightButton == ButtonState.Released &&
        prevMouse.RightButton == ButtonState.Pressed;
    }

    public static Vector2 Mouse_Position()
    {
      return new Vector2(mouse.X, mouse.Y);
    }

    public static Vector2 Mouse_PreviousPosition()
    {
      return new Vector2(prevMouse.X, prevMouse.Y);
    }

    #endregion
  }
}
