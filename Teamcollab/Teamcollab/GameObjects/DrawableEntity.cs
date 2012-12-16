using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Resources;
using Microsoft.Xna.Framework;

namespace Teamcollab.GameObjects
{
  class DrawableEntity : Entity
  {
    #region Properties
    #endregion

    #region Members
    protected EntityType entityType;
    protected Resource<Texture2D> resource;
    protected Vector2 position;
    #endregion

    public DrawableEntity()
    {
    }

    /// <summary>
    /// Used by the entity to draw itself
    /// </summary>
    /// <param name="spriteBatch"></param>
    public abstract void Draw(SpriteBatch spriteBatch);
  }
}
