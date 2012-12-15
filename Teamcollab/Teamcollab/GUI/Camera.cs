using Microsoft.Xna.Framework;

namespace Teamcollab.GUI
{
  /// <summary>
  /// A automatically moving camera, holding a Transform Matrix used for drawing.
  /// </summary>
  class Camera2D
  {
    #region Properties
    public Vector2 Position { get { return position; } }
    public Vector2 Origin { get; private set; }
    public Matrix Transform { get; private set; }
    public float Scale { get; private set; }
    public Rectangle Bounds
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
    private Vector2 halfScreenSize; // Half is used more than whole
    private Vector2 position;
    private Vector2 targetPosition;
    private float targetScale;
    #endregion
    
    public Camera2D(Vector2 screenSize)
    {
      Scale = 1f;
      halfScreenSize = screenSize / 2;
      position = halfScreenSize / Scale;
      Origin = halfScreenSize / Scale;

      UpdateTransformMatrix();
    }

    /// <summary>
    /// Sets the target zoom factor
    /// </summary>
    /// <param name="scale">Target zoom factor</param>
    public void SetTargetScale(float scale)
    {
      targetScale = scale;
    }

    /// <summary>
    /// Sets the world coordinate to focus on
    /// </summary>
    /// <param name="worldCoordinate">World coordinate</param>
    public void SetTargetPosition(Vector2 worldCoordinate)
    {
      targetPosition = worldCoordinate;
    }

    /// <summary>
    /// Needs to be called for the camera to update
    /// </summary>
    public void Update()
    {
      AutoPan();
      AutoZoom();
      UpdateTransformMatrix();
    }

    /// <summary>
    /// Scales the camera towards the target scale
    /// </summary>
    private void AutoZoom()
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
    private void AutoPan()
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
    private void UpdateTransformMatrix()
    {
      Transform =
        Matrix.Identity *
        Matrix.CreateTranslation(
          Origin.X - position.X,
          Origin.Y - position.Y,
          -0.1f) *
        Matrix.CreateScale(Scale);
    }

    /// <summary>
    /// Translates a camera coordinate into a world coordinate
    /// </summary>
    /// <param name="camCoord">Camera coordinate</param>
    /// <returns>World coordinate</returns>
    public Vector2 TranslatePositionByCamera(Vector2 camCoord)
    {
      return (camCoord / Scale) - (Origin - Position);
    }
  }
}
