using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Midgard.Engine.Helpers;
using Midgard.GUI;
using Midgard.Engine.WorldManagement;
using Microsoft.Xna.Framework.Graphics;
using Midgard.Resources;

namespace Midgard.GameObjects
{
  class Player : Actor
  {
    #region Properties

    #endregion

    #region Members
    bool armed;
    #endregion

    public Player(Vector2 worldPosition)
      : base(EntityType.Player, worldPosition)
    {
      // TODO(Peter): Move, fix, burn
      Speed = 0.65f;

      NeedsUpdate = true;
      armed = true;
    }

    public override void Update(float deltaTime)
    {
      base.Update(deltaTime);
    }

    protected override void UpdateInput()
    {
      if (InputManager.MouseLeftDown())
      {
        targetPosition = Camera2D.TranslateScreenToWorld(
          InputManager.MousePosition());
      }
    }

    protected override void UpdateMovement(float deltaTime)
    {
      base.UpdateMovement(deltaTime);
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
      base.Draw(batch);
      if (armed)
      {
        //batch.Draw(ResourceManager.SpriteTextureBank.Query("Shield"), WorldPosition, sourceRectangle, Color.White, 0, origin, scale, SpriteEffects.None, 0);
        //batch.Draw(ResourceManager.SpriteTextureBank.Query("Sword"), WorldPosition, sourceRectangle, Color.White, 0, origin, scale, SpriteEffects.None, 0);
      }
    }
  }
}
