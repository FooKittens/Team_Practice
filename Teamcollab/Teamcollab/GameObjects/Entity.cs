using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Resources;
using Microsoft.Xna.Framework;
using Teamcollab.Engine.WorldManagement;

namespace Teamcollab.GameObjects
{
  struct EntityData
  {

  }

  /// <summary>
  /// A blueprint for entities
  /// </summary>
  abstract class Entity
  {
    #region Properties
    public Vector2 PositionInCluster { get { return positionInCluster; } }
    public abstract EntityType EntityType { get; }
    public bool NeedsUpdate { get; protected set; }
    #endregion

    #region Members
    protected bool isDead;
    protected Vector2 positionInCluster;
    #endregion

    public Entity(Vector2 positionInCluster)
    {
      this.positionInCluster = positionInCluster;
    }

    /// <summary>
    /// Used by the entity to update itself
    /// </summary>
    /// <param name="gameTime"></param>
    public virtual void Update(GameTime gameTime)
    {
      UpdateState();
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
      positionInCluster = newClusterPosition;
    }
  }
}
