using Microsoft.Xna.Framework;
using System;
using Midgard.Engine.WorldManagement;
using Midgard.Engine.Helpers;

namespace Midgard.GUI
{
  /// <summary>
  /// A automatically moving camera, holding a Transform Matrix used for drawing.
  /// </summary>
  static class Camera2D
  {
    #region Properties
    static public Vector2 Position { get { return position; } }
    static public Vector2 TargetPosition { get { return targetPosition; } }
    static public Vector2 Origin { get; private set; }
    static public Matrix View { get; private set; }
    static public Matrix Projection { get; private set; }
    static public float Scale { get; private set; }
    /// <summary>
    /// The bounds of the camera, in screen(!) coordinates
    /// </summary>
    static public Rectangle Bounds
    {
      get
      {
        Vector2 topLeft = Vector2.Zero;
        Vector2 btmRight = halfScreenSize * 2;
        topLeft = TranlatePositionToScreen(topLeft);
        btmRight = TranlatePositionToScreen(btmRight);
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
    static private Vector2 halfScreenSize; // Half is used more than whole
    static private Vector2 position;
    static private Vector2 targetPosition;
    static private float targetScale;
    static private Matrix translationMatrix;
    #endregion

    static Camera2D()
    {
      Scale = 1f;
      halfScreenSize = new Vector2(
        Settings.ScreenWidth / 2,
        Settings.ScreenHeight / 2
      );
      position = Vector2.Zero;
      Origin = halfScreenSize / Scale;
      targetScale = 1f;
      UpdateMatrices();
    }

    /// <summary>
    /// Sets the target zoom factor
    /// </summary>
    /// <param name="scale">Target zoom factor</param>
    static public void SetTargetScale(float target)
    {
      targetScale = target;
    }

    /// <summary>
    /// Sets the world coordinate to move to
    /// </summary>
    /// <param name="worldCoordinate">World coordinate</param>
    static public void SetTargetPosition(Vector2 worldCoordinate)
    {
      targetPosition = WorldManager.TransformIsometric(worldCoordinate);
      targetPosition = WorldManager.TransformWorldToScreen(targetPosition);
    }

    /// <summary>
    /// Sets the world coordinate to focus on
    /// </summary>
    /// <param name="worldCoordinate">World coordinate</param>
    static public void SetPosition(Vector2 worldCoordinate)
    {
      position = WorldManager.TransformIsometric(worldCoordinate);
      position = WorldManager.TransformWorldToScreen(position);
      targetPosition = position;
    }

    /// <summary>
    /// Needs to be called for the camera to update
    /// </summary>
    static public void Update(float delta)
    {
      AutoPan(delta);
      AutoZoom(delta);
      UpdateMatrices();

      float sqtwo = (float)Math.Sqrt(2f);
   
      Vector2 camPos = Position;
      Vector2 camTile = WorldManager.TransformScreenToWorld(camPos);
      camTile = WorldManager.TransformInvIsometric(camTile);

      camPos = WorldManager.TransformScreenToCluster(camPos);
      
      camPos = WorldManager.TransformInvIsometric(camPos);

      camPos = new Vector2(Convert.ToInt32(camPos.X),
        Convert.ToInt32(camPos.Y)
      );

      ImmediateDrawer.GetInstance(null).DrawString(
        "Cluster: " + camPos.ToString() + "\nTile: " + camTile.ToString(),
        Vector2.Zero
      );
    }

    /// <summary>
    /// Scales the camera towards the target scale
    /// </summary>
    static private void AutoZoom(float deltaTime)
    {
      float diff = targetScale - Scale;
      if (Math.Abs(diff) < 0.001f)
      {
        Scale = targetScale;
      }
      else
      {
        float acc = diff * 320; // TODO (Martin): 320? Random working value...
        Scale = (acc * (float)Math.Pow(deltaTime, 2) +
          (acc * (float)Math.Pow(deltaTime, 2)) / 2 + Scale);
      }

      Origin = halfScreenSize / Scale;
    }

    /// <summary>
    /// Moves the camera towards the target position
    /// </summary>
    static private void AutoPan(float deltaTime)
    {
      Vector2 diff = targetPosition - position;
      if (diff.LengthSquared() < 1)
      {
        position = targetPosition;
      }
      else
      {
        Vector2 acc = diff * 320; // TODO (Martin): 320? Random working value...
        position = (acc * (float)Math.Pow(deltaTime, 2) +
          (acc * (float)Math.Pow(deltaTime, 2)) / 2 + position);
      }
    }

    /// <summary>
    /// Updates the matrix used for drawing
    /// </summary>
    static private void UpdateMatrices()
    {
      View = 
        Matrix.CreateLookAt(
          new Vector3(position.X, position.Y, -8),
          new Vector3(position.X, position.Y, 0),
          -Vector3.UnitY
        );

      Projection = Matrix.CreateOrthographicOffCenter(
          -Settings.ScreenWidth / (2 * Scale),
          Settings.ScreenWidth / (2 * Scale),
          -Settings.ScreenHeight / (2 * Scale),
          Settings.ScreenHeight / (2 * Scale),
          0,
          10
        );
      
      translationMatrix = Matrix.CreateScale(1 / Scale) *
      Matrix.CreateTranslation(-Origin.X, -Origin.Y, 0) *
      Matrix.CreateTranslation(Position.X, Position.Y, 0);
    }

    /// <summary>
    /// Translates a camera coordinate into a world coordinate
    /// </summary>
    /// <param name="camCoord">Camera coordinate</param>
    /// <returns>World coordinate</returns>
    static public Vector2 TranslateScreenToWorld(Vector2 camCoord)
    {
      camCoord = Vector2.Transform(camCoord, translationMatrix);
      camCoord = WorldManager.TransformScreenToWorld(camCoord);
      return WorldManager.TransformInvIsometric(camCoord);
    }

    static private Vector2 TranlatePositionToScreen(Vector2 camCoord)
    {
      return Vector2.Transform(camCoord, translationMatrix);
    }
  }
}
