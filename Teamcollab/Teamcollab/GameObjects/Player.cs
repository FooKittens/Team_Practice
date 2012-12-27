using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Teamcollab.Engine.Helpers;
using Teamcollab.GUI;
using Teamcollab.Engine.WorldManagement;
using Microsoft.Xna.Framework.Graphics;

namespace Teamcollab.GameObjects
{
  class Player : MovingEntity
  {
    #region Properties

    #endregion

    #region Members
    // Movement
    Vector2 targetPosition;
    const float MovementSpeed = 0.07f;

    // Animation
    Texture2D texture;
    Vector2 origin;
    bool isMoving;
    #endregion

    public Player(Vector2 worldPosition)
      : base(EntityType.Player, worldPosition)
    {
      NeedsUpdate = true;

      #region Animation
      texture = Resources.ResourceManager.SpriteTextureBank.Query("Ogre");
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
        int generalDirection = Convert.ToInt32(percentualDir *= 8);
        currentRow = (generalDirection + 1) % 8;

        // Movement
        diff.Normalize();
        diff *= MovementSpeed;
        worldPosition += diff;
      }
    }

    protected override void UpdateState()
    {
      
    }

    const int MillisecondsPerFrame = 100;
    int animationTimer = MillisecondsPerFrame;

    protected virtual void UpdateAnimation(GameTime gameTime)
    {
      if (isMoving)
      {
        animationTimer -= gameTime.ElapsedGameTime.Milliseconds;
        if (animationTimer < 0)
        {
          animationTimer += MillisecondsPerFrame;
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

    const int ColumnCount = 8;
    const int RowCount = 8;
    int currentColumn;
    int currentRow;
    int columnWidth;
    int rowHeight;

    public override void Draw(IsoBatch batch)
    {
      Rectangle source = new Rectangle(
        currentColumn * columnWidth,
        currentRow * rowHeight,
        columnWidth,
        rowHeight
      );
      batch.Draw(texture, WorldPosition, source, Color.White, 0, origin, 0.5f, SpriteEffects.None, 0);
    }
  }
}
