using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Resources;
using Microsoft.Xna.Framework;
using Teamcollab.Engine.WorldManagement;
using Teamcollab.Engine.Helpers;

namespace Teamcollab.GameObjects
{
  struct EntityData
  {

  }

  /// <summary>
  /// A blueprint for entities
  /// </summary>
  public abstract class Entity
  {
    #region Properties
    public Vector2 WorldPosition { get { return worldPosition; } }
    public EntityType EntityType { get; protected set; }
    public bool NeedsUpdate { get; protected set; }
    #endregion

    #region Members
    protected bool isDead;
    protected Vector2 worldPosition;
    #endregion

    public Entity(EntityType type, Vector2 worldPosition)
    {
      this.worldPosition = worldPosition;
      this.EntityType = type;
    }

    /// <summary>
    /// Used by the entity to update itself
    /// </summary>
    /// <param name="gameTime"></param>
    public virtual void Update(GameTime gameTime)
    {
      UpdateState();
    }

    public virtual void Draw(IsoBatch batch)
    {

    }

    protected abstract void UpdateState();

    /// <summary>
    /// Destroys the entity and sets IsDead to true
    /// </summary>
    public virtual void Destroy()
    {
      isDead = true;
    }

    /// <summary>
    /// Supposed to return a struct of data, cast to object
    /// </summary>
    /// <returns>Tell me if you see this and in what context
    /// (martingronlund@live.se)
    /// </returns>
    public abstract object GetData();
  }
}
