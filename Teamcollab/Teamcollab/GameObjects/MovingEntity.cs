using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Teamcollab.GameObjects
{
  abstract class MovingEntity : DrawableEntity
  {
    public MovingEntity(Vector2 positionInCluster)
      : base(positionInCluster)
    {
    }

    public override void  Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      UpdateInput();
      UpdateMovement();
 	    base.Update(gameTime);
    }

    protected abstract void UpdateInput();
    protected abstract void UpdateMovement();
  }
}
