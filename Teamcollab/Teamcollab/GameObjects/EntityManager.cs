using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Midgard.Engine.Helpers;

namespace Midgard.GameObjects
{
  public sealed class EntityManager
  {
    #region Properties
    #endregion

    static object addLocker = new object();

    #region Members
    Queue<Entity> addQueue;
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
      addQueue = new Queue<Entity>();
    }

    public void Update(GameTime gameTime)
    {
      EmptyAddQueue();

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
      lock (addLocker)
      {
        addQueue.Enqueue(entity);
      }
    }

    public Entity[] GetAllEntitiesOfType(EntityType type)
    {
      List<Entity> typeMatches = new List<Entity>();
      for (int i = 0; i < entities.Count; ++i)
      {
        if (entities[i].EntityType == type)
        {
          typeMatches.Add(entities[i]);
        }
      }
      return typeMatches.ToArray();
    }

    /// <summary>
    /// Returns null if no occurency was found
    /// </summary>
    /// <param name="type">EntityType to check for</param>
    public Entity GetFirstOccurencyOfType(EntityType type)
    {
      for (int i = 0; i < entities.Count; ++i)
      {
        if (entities[i].EntityType == type)
        {
          return entities[i];
        }
      }
      return null;
    }

    private void EmptyAddQueue()
    {
      lock (addLocker)
      {
        while (addQueue.Count > 0)
        {
          entities.Add(addQueue.Dequeue());
        }
      }
    }
  }
}
