using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Teamcollab.Engine.Helpers;
using Teamcollab.GUI;
using Teamcollab.Engine.WorldManagement;

namespace Teamcollab.GameObjects
{
  class Player : MovingEntity
  {
    #region Properties

    #endregion

    #region Members
    Vector2 targetPosition;
    const float Speed = 5f;
    #endregion

    public Player(Vector2 worldPosition)
      : base(EntityType.Player, worldPosition)
    {
      NeedsUpdate = true;
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }

    protected override void UpdateInput()
    {
      if (InputManager.MouseLeftDown())
      {
        targetPosition = Camera2D.TranslatePositionByCamera(
          InputManager.MousePosition()
        );
      }
    }

    protected override void UpdateMovement()
    {
      Vector2 diff = targetPosition - worldPosition;
      diff.Y *= 2f; // Check for double Y because Y-speed is half X-speed
      if (diff.Length() < Speed)
      {
        worldPosition = targetPosition;
      }
      else
      {
        diff.Normalize();
        diff.X *= Speed;
        diff.Y *= Speed / 2f; // Halve Y-speed
        worldPosition += diff;
      }
    }

    protected override void UpdateState()
    {
      
    }

    public override object GetData()
    {
      throw new NotImplementedException();
    }

    public override void Draw(IsoBatch batch)
    {
      batch.Draw(Resources.ResourceManager.TileTextureBank.Query("Grass"), WorldPosition, Color.White);
    }
  }
}
