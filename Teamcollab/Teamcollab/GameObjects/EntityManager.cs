using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Teamcollab.Engine.World;

namespace Teamcollab.GameObjects
{
  class EntityManager
  {
    #region Properties
    Cluster parent;
    #endregion

    #region Members
    List<Entity> entityList;
    #endregion

    #region Events
    public event ObjectOutOfBoundsHandler ObjectOutOfBoundsEvent;
    #endregion

    public delegate void ObjectOutOfBoundsHandler(Entity entity); // TODO(Martin): Move to world manager

    public EntityManager(Cluster cluster)
    {
      parent = cluster;
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
  }
}
