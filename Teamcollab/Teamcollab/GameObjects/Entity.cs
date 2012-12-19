using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Resources;
using Microsoft.Xna.Framework;
using Teamcollab.Engine.World;

namespace Teamcollab.GameObjects
{
  struct EntityData
  {
    public bool needsUpdate;
    public bool isDead;
    public Vector2 positionInCluster;
    public EntityType entityType;
  }

  /// <summary>
  /// A blueprint for entities
  /// </summary>
  abstract class Entity
  {
    #region Properties
    public bool NeedsUpdate
    {
      get
      {
        return data.needsUpdate;
      }
      protected set
      {
        data.needsUpdate = value;
      }
    }
    public bool IsDead
    {
      get
      {
        return data.isDead;
      }
      protected set
      {
        data.isDead = value;
      }
    }
    public Vector2 PositionInCluster
    {
      get
      {
        return data.positionInCluster;
      }
      protected set
      {
        data.positionInCluster = value;
      }
    }
    public EntityType EntityType
    {
      get
      {
        return data.entityType;
      }
      protected set
      {
        data.entityType = value;
      }
    }
    #endregion

    #region Members
    protected EntityData data;
    #endregion

    public Entity(Vector2 positionInCluster)
    {
      PositionInCluster = positionInCluster;
    }

    /// <summary>
    /// Used by the entity to update itself
    /// </summary>
    /// <param name="gameTime"></param>
    public virtual void Update(GameTime gameTime)
    {
      UpdateInput();
      UpdateMovement();
      UpdateState();
      UpdateAnimation();
    }

    protected abstract void UpdateInput();
    protected abstract void UpdateMovement();
    protected abstract void UpdateState();
    protected abstract void UpdateAnimation();

    /// <summary>
    /// Destroys the entity and sets IsDead to true
    /// </summary>
    public virtual void Destroy()
    {
      IsDead = true;
    }

    /// <summary>
    /// Supposed to return a struct of data, cast to object
    /// </summary>
    /// <returns>Tell me if you see this and in what context
    /// (martingronlund@live.se)
    /// </returns>
    public abstract object GetData();

    /// <summary>
    /// Changes the entity's cluster, and moves its
    /// position relative to the new cluster
    /// </summary>
    /// <param name="newClusterPosition">
    /// The new position, relative to the new cluster
    /// </param>
    /// <param name="worldManager">
    /// Used to ensure that it is the world manager that calls the method
    /// </param>
    public void ChangeCluster(Vector2 newClusterPosition, WorldManager worldManager)
    {
      PositionInCluster = newClusterPosition;
    }
  }
}
