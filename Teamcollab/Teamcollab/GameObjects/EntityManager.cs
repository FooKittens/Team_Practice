﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Teamcollab.Engine.Helpers;

namespace Teamcollab.GameObjects
{
  public sealed class EntityManager
  {
    #region Properties
    #endregion

    #region Members
    private static EntityManager singleton;
    List<Entity> entities;
    #endregion

    public static EntityManager GetInstance()
    {
      if (singleton == null)
      {
        singleton = new EntityManager();
      }

      return singleton;
    }

    private EntityManager()
    {
      entities = new List<Entity>();
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

    public void Draw(IsoBatch isoBatch)
    {
      foreach (Entity entity in entities)
      {
        entity.Draw(isoBatch);
      }
    }

    public void AddObject(Entity entity)
    {
      entities.Add(entity);
    }
  }
}
