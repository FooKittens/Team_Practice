using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Teamcollab.Engine.WorldManagement;

namespace Teamcollab.GameObjects
{
  class EntityManager
  {
    #region Properties
    Cluster parent;
    #endregion

    #region Members
    List<Entity> entities;
    #endregion

    #region Events
    public event ObjectOutOfBoundsHandler ObjectOutOfBoundsEvent;
    #endregion

    public delegate void ObjectOutOfBoundsHandler(Entity entity); // TODO(Martin): Move to world manager

    public EntityManager(Cluster cluster)
    {
      parent = cluster;
      entities = new List<Entity>();
    }

    private void BoundsCheck(Entity entity)
    {
      Vector2 pos = entity.PositionInCluster;
      if (pos.X < Constants.ClusterWidth / 2 &&
        pos.X > -Constants.ClusterWidth / 2 &&
        pos.Y < Constants.ClusterHeight / 2 &&
        pos.Y > -Constants.ClusterHeight / 2)
      {
        ObjectOutOfBoundsEvent(entity);
      }
    }

    public void Update(GameTime gameTime)
    {
      foreach (Entity entity in entities)
      {
        if (entity.NeedsUpdate)
        {
          entity.Update(gameTime);
        }
      }
    }

    public void AddObject(Entity entity)
    {
      entities.Add(entity);
    }
  }
}
