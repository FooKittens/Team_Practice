using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Teamcollab.GameObjects
{
  abstract class MovingEntity : Entity
  {
    public MovingEntity(EntityType type, Vector2 worldPosition)
      : base(type, worldPosition)
    {
    }

    public override void Update(GameTime gameTime)
    {
      UpdateInput();
      UpdateMovement();
 	    base.Update(gameTime);
    }

    protected abstract void UpdateInput();
    protected abstract void UpdateMovement();
  }
}
