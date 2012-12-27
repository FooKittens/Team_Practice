using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Teamcollab.Engine.Helpers;
using Teamcollab.GUI;
using Teamcollab.Engine.WorldManagement;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Resources;

namespace Teamcollab.GameObjects
{
  class Player : MovingEntity
  {
    #region Properties

    #endregion

    #region Members
    // Movement
    Vector2 targetPosition;
    const float MovementSpeed = 0.085f;

    // Animation
    Texture2D texture;
    Vector2 origin;
    bool isMoving;
    const int ColumnCount = 8;
    const int RowCount = 8;
    int currentColumn;
    int currentRow;
    int columnWidth;
    int rowHeight;
    int millisecondsPerFrame = 150;
    int animationTimer = 150;
    int DirectionsCount = 8;
    #endregion

    public Player(Vector2 worldPosition)
      : base(EntityType.Player, worldPosition)
    {
      NeedsUpdate = true;

      #region Animation
      texture = ResourceManager.SpriteTextureBank.Query("MaleHeavy");
      columnWidth = texture.Width / ColumnCount;
      rowHeight = texture.Height / RowCount;
      origin = new Vector2(columnWidth / 2,
        rowHeight / 2 + Constants.TileHeight // This will change per sprite
      );
      isMoving = false;
      #endregion
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      UpdateAnimation(gameTime);
    }

    protected override void UpdateInput()
    {
      if (InputManager.MouseLeftDown())
      {
        targetPosition = Camera2D.TranslateScreenToWorld(
          InputManager.MousePosition());
      }
    }

    protected override void UpdateMovement(GameTime gameTime)
    {
      Vector2 diff = targetPosition - worldPosition;
      if (diff.Length() < MovementSpeed)
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
        diff *= MovementSpeed;
        worldPosition += diff;
      }
    }

    protected override void UpdateState()
    {
      
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
    }

    public override object GetData()
    {
      throw new NotImplementedException();
    }

    public override void Draw(IsoBatch batch)
    {
      Rectangle source = new Rectangle(
        currentColumn * columnWidth,
        currentRow * rowHeight,
        columnWidth,
        rowHeight
      );
      batch.Draw(texture, WorldPosition, source, Color.White, 0, origin, 1, SpriteEffects.None, 0);
      batch.Draw(ResourceManager.SpriteTextureBank.Query("Shield"), WorldPosition, source, Color.White, 0, origin, 1, SpriteEffects.None, 0);
      batch.Draw(ResourceManager.SpriteTextureBank.Query("Sword"), WorldPosition, source, Color.White, 0, origin, 1, SpriteEffects.None, 0);
    }
  }
}
