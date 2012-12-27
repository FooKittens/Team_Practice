using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Resources;
using System;

namespace Teamcollab.GameObjects
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
    #endregion

    public MovingEntity(EntityType type, Vector2 worldPosition)
      : base(type, worldPosition)
    {
      movementSpeed = BaseSpeed;

      #region Animation
      texture = ResourceManager.SpriteTextureBank.Query(type.ToString());
      columnWidth = texture.Width / ColumnCount;
      rowHeight = texture.Height / RowCount;
      origin = new Vector2(columnWidth / 2,
        rowHeight / 2 + Constants.TileHeight // Make sure the sprite is right
      );
      isMoving = false;
      #endregion
    }

    public override void Update(GameTime gameTime)
    {
      UpdateInput();
      UpdateMovement(gameTime);
      UpdateAnimation(gameTime);
 	    base.Update(gameTime);
    }

    protected abstract void UpdateInput();

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

    protected virtual void UpdateAnimation(GameTime gameTime)
    {
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

    public override void Draw(Engine.Helpers.IsoBatch batch)
    {
      batch.Draw(texture, WorldPosition, sourceRectangle, Color.White, 0, origin, 1, SpriteEffects.None, 0);
    }
  }
}
