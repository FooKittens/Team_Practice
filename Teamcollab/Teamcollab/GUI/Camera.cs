﻿using Microsoft.Xna.Framework;

namespace Teamcollab.GUI
{
  /// <summary>
  /// A automatically moving camera, holding a Transform Matrix used for drawing.
  /// </summary>
  static class Camera2D
  {
    #region Properties
    static public Vector2 Position { get { return position; } }
    static public Vector2 Origin { get; private set; }
    static public Matrix Transform { get; private set; }
    static public float Scale { get; private set; }
    static public Rectangle Bounds
    {
      get
      {
        Vector2 topLeft = Vector2.Zero;
        Vector2 btmRight = halfScreenSize * 2;
        topLeft = TranslatePositionByCamera(topLeft);
        btmRight = TranslatePositionByCamera(btmRight);
        Rectangle result = new Rectangle(
          (int)topLeft.X,
          (int)topLeft.Y,
          (int)(btmRight.X - topLeft.X),
          (int)(btmRight.Y - topLeft.Y));
        return result;
      }
    }
    #endregion

    #region Members
    static Vector2 halfScreenSize; // Half is used more than whole
    static private Vector2 position;
    static private Vector2 targetPosition;
    static private float targetScale;
    #endregion

    static Camera2D()
    {
      Scale = 1f;
      halfScreenSize = new Vector2(Settings.ScreenWidth, Settings.ScreenHeight);
      position = Vector2.Zero;
      Origin = halfScreenSize / Scale;

      UpdateTransformMatrix();
    }

    /// <summary>
    /// Sets the target zoom factor
    /// </summary>
    /// <param name="scale">Target zoom factor</param>
    static public void SetTargetScale(float scale)
    {
      targetScale = scale;
    }

    /// <summary>
    /// Sets the world coordinate to move to
    /// </summary>
    /// <param name="worldCoordinate">World coordinate</param>
    static public void SetTargetPosition(Vector2 worldCoordinate)
    {
      targetPosition = worldCoordinate;
    }

    /// <summary>
    /// Sets the world coordinate to focus on
    /// </summary>
    /// <param name="worldCoordinate">World coordinate</param>
    static public void SetPosition(Vector2 worldCoordinate)
    {
      position = worldCoordinate;
    }

    /// <summary>
    /// Needs to be called for the camera to update
    /// </summary>
    static public void Update()
    {
      AutoPan();
      AutoZoom();
      UpdateTransformMatrix();
    }

    /// <summary>
    /// Scales the camera towards the target scale
    /// </summary>
    static private void AutoZoom()
    {
      float diff = Scale - targetScale;
      if (diff < 0.01f)
      {
        Scale = targetScale;
      }
      else
      {
        Scale += (diff / 2);
      }
      Scale = MathHelper.Clamp(Scale, 0.1f, 2.5f);

      Origin = halfScreenSize / Scale;
    }

    /// <summary>
    /// Moves the camera towards the target position
    /// </summary>
    static private void AutoPan()
    {
      Vector2 diff = position - targetPosition;
      if (diff.Length() < 1)
      {
        position = targetPosition;
      }
      else
      {
        position += (diff / 2) / Scale;
      }
    }

    /// <summary>
    /// Updates the matrix used for drawing
    /// </summary>
    static private void UpdateTransformMatrix()
    {
      Transform =
        Matrix.Identity *
        Matrix.CreateTranslation(
          Origin.X - position.X,
          Origin.Y - position.Y,
          0) *
        Matrix.CreateScale(Scale);
    }

    /// <summary>
    /// Translates a camera coordinate into a world coordinate
    /// </summary>
    /// <param name="camCoord">Camera coordinate</param>
    /// <returns>World coordinate</returns>
    static public Vector2 TranslatePositionByCamera(Vector2 camCoord)
    {
      return (camCoord / Scale) - (Origin - Position);
    }
  }
}
