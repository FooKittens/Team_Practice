﻿using Microsoft.Xna.Framework;
using System;

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
      halfScreenSize = new Vector2(Settings.ScreenWidth / 2, Settings.ScreenHeight / 2);
      position = Vector2.Zero;
      Origin = halfScreenSize / Scale;
      targetScale = 1f;
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
    static public void Update(GameTime gameTime)
    {
      AutoPan(gameTime);
      AutoZoom(gameTime);
      UpdateTransformMatrix();
    }

    /// <summary>
    /// Scales the camera towards the target scale
    /// </summary>
    static private void AutoZoom(GameTime gameTime)
    {
      float diff = targetScale - Scale;
      if (Math.Abs(diff) < 0.01f)
      {
        Scale = targetScale;
      }
      else
      {
        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float acc = diff * 320; // TODO (Martin): 320? Random working value... Probably because of pixel size?
        Scale = (acc * (float)Math.Pow(time, 2) + (acc * (float)Math.Pow(time, 2)) / 2 + Scale);
      }

      Origin = halfScreenSize / Scale;
    }

    /// <summary>
    /// Moves the camera towards the target position
    /// </summary>
    static private void AutoPan(GameTime gameTime)
    {
      Vector2 diff = targetPosition - position;
      if (diff.LengthSquared() < 1)
      {
        position = targetPosition;
      }
      else
      {
        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 acc = diff * 320; // TODO (Martin): 320? Random working value... Probably because of pixel size?
        position = (acc * (float)Math.Pow(time, 2) + (acc * (float)Math.Pow(time, 2)) / 2 + position);
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
