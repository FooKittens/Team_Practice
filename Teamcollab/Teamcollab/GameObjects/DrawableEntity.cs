using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Resources;
using Microsoft.Xna.Framework;

namespace Teamcollab.GameObjects
{
  abstract class DrawableEntity : Entity
  {
    #region Properties
    #endregion

    #region Members
    #endregion

    public DrawableEntity(Vector2 positionInCluster)
      :base(positionInCluster)
    {
    }

    /// <summary>
    /// Used by the entity to draw itself
    /// </summary>
    /// <param name="spriteBatch"></param>
    public abstract void Draw(SpriteBatch spriteBatch);
  }
}
