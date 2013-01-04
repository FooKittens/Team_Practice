using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Midgard.Resources;
using Midgard.Engine.Helpers;
using System;

namespace Midgard.GameObjects
{
  abstract class MovingEntity : Entity
  {
    #region Properties
    protected abstract float BaseSpeed { get; }
    #endregion

    #region Members
    // Movement
    protected Vector2 targetPosition;
    protected float movementSpeed;

    // Animation
    protected Texture2D texture;
    protected Vector2 origin;
    protected bool isMoving;
    protected const int ColumnCount = 8;
    protected const int RowCount = 8;
    protected int currentColumn;
    protected int currentRow;
    protected int columnWidth;
    protected int rowHeight;
    protected int millisecondsPerFrame = 150;
    protected int animationTimer = 150;
    protected int DirectionsCount = 8;
    protected Rectangle sourceRectangle;
    protected float scale;
    #endregion

    public MovingEntity(EntityType type, Vector2 worldPosition)
      : base(type, worldPosition)
    {
      movementSpeed = BaseSpeed;

      #region Animation
      scale = 1f;
      texture = ResourceManager.SpriteTextureBank.Query(type.ToString());
      columnWidth = texture.Width / ColumnCount;
      rowHeight = texture.Height / RowCount;
      origin = new Vector2(columnWidth / 2,
        rowHeight / 2 + Constants.TileHeight // Make sure the sprite is right
      );
      isMoving = false;
      #endregion
    }

    /// <summary>
    /// Updates the entity
    /// </summary>
    /// <param name="gameTime"></param>
    public override void Update(GameTime gameTime)
    {
      UpdateInput();
      UpdateMovement(gameTime);
      UpdateAnimation(gameTime);
 	    base.Update(gameTime);
    }

    /// <summary>
    /// Abstract method for input handling
    /// </summary>
    protected abstract void UpdateInput();

    /// <summary>
    /// Moves the entity toward it's target position
    /// </summary>
    protected virtual void UpdateMovement(GameTime gameTime)
    {
      Vector2 diff = targetPosition - worldPosition;
      if (diff.Length() < movementSpeed)
      {
        isMoving = false;

        worldPosition = targetPosition;
      }
      else
      {
        // Animation
        isMoving = true;
        float direction = (float)Math.Atan2(diff.Y, diff.X);
        float percentualDir = direction / MathHelper.TwoPi + 0.5f;
        int generalDirection = Convert.ToInt32(percentualDir *= DirectionsCount);
        currentRow = (generalDirection + 1) % DirectionsCount;

        // Movement
        diff.Normalize();
        diff *= movementSpeed;
        worldPosition += diff;
      }
    }

    /// <summary>
    /// Updates the entity animation
    /// </summary>
    protected virtual void UpdateAnimation(GameTime gameTime)
    {
      // TODO(Martin): Rewrite for new animation system
      if (isMoving)
      {
        animationTimer -= gameTime.ElapsedGameTime.Milliseconds;
        if (animationTimer < 0)
        {
          animationTimer += millisecondsPerFrame;
          currentColumn += 1;
          if (currentColumn == 4)
          {
            currentColumn = 0;
          }
        }
      }
      else
      {
        currentColumn = 0;
      }

      sourceRectangle = new Rectangle(
        currentColumn * columnWidth,
        currentRow * rowHeight,
        columnWidth,
        rowHeight
      );
    }

    /// <summary>
    /// Draws the entity
    /// </summary>
    /// <param name="batch">IsoBatch for drawing</param>
    public override void Draw(IsoBatch batch)
    {
      batch.Draw(texture, WorldPosition, sourceRectangle, Color.White, 0, origin, scale, SpriteEffects.None, 0);
    }
  }
}
