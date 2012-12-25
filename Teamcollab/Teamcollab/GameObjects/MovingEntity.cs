using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Teamcollab.GameObjects
{
  abstract class MovingEntity : DrawableEntity
  {
    public MovingEntity(Vector2 worldPosition)
      : base(worldPosition)
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
