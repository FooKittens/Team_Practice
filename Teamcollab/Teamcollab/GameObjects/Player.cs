using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Teamcollab.Engine.Helpers;

namespace Teamcollab.GameObjects
{
  class Player : MovingEntity
  {
    #region Properties

    #endregion

    #region Members

    #endregion

    public Player(EntityType type, Vector2 worldPosition)
      : base(type, worldPosition)
    {
      base.EntityType = EntityType.Player;
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }

    protected override void UpdateInput()
    {
      if (InputManager.KeyDown(Keys.Down))
      {
        worldPosition.Y -= 1f;
      }
      if (InputManager.KeyDown(Keys.Up))
      {
        worldPosition.Y -= 1f;
      }
      if (InputManager.KeyDown(Keys.Left))
      {
        worldPosition.X -= 1f;
      }
      if (InputManager.KeyDown(Keys.Right))
      {
        worldPosition.X -= 1f;
      }
    }

    protected override void UpdateMovement()
    {

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
