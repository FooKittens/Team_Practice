﻿using System;
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
    protected override float BaseSpeed
    {
      get { return 0.085f; }
    }
    #endregion

    #region Members
    bool armed;
    #endregion

    public Player(Vector2 worldPosition)
      : base(EntityType.Player, worldPosition)
    {
      NeedsUpdate = true;
      armed = true;
      base.scale = 0.65f;
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
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
      base.UpdateMovement(gameTime);
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
        batch.Draw(ResourceManager.SpriteTextureBank.Query("Shield"), WorldPosition, sourceRectangle, Color.White, 0, origin, scale, SpriteEffects.None, 0);
        batch.Draw(ResourceManager.SpriteTextureBank.Query("Sword"), WorldPosition, sourceRectangle, Color.White, 0, origin, scale, SpriteEffects.None, 0);
      }
    }
  }
}
