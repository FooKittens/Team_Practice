using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Resources;
using Microsoft.Xna.Framework;

namespace Teamcollab.GameObjects
{
  /// <summary>
  /// Holds all entity types
  /// </summary>
  enum EntityType
  {
    Undefined = 0,

    Player,
  }

  /// <summary>
  /// A blueprint for entities
  /// </summary>
  abstract class Entity
  {
    #region Properties
    #endregion

    #region Members
    protected EntityType entityType;
    #endregion

    public Entity()
    {
    }

    /// <summary>
    /// Used by the entity to update itself
    /// </summary>
    /// <param name="gameTime"></param>
    public abstract void Update(GameTime gameTime);
  }
}
