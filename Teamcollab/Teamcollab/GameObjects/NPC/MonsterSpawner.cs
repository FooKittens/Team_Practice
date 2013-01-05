using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Midgard.Engine.Helpers;
using Midgard.Resources;
using Microsoft.Xna.Framework.Graphics;
using Midgard.DataSerialization;

namespace Midgard.GameObjects.NPC
{
  class MonsterSpawner : Entity
  {
    #region Properties
    public int MaxActiveCount { get; set; }
    public int SpawnTimeIntervalMs { get; set; }
    #endregion

    #region Members
    List<EntityType> myTypes;
    List<Enemy> myEnemies;
    float currentSpawnTime;
    #endregion

    public MonsterSpawner(EntityType type, Vector2 worldPosition)
      : base(type, worldPosition)
    {
      myTypes = new List<EntityType>();
      myEnemies = new List<Enemy>();
      currentSpawnTime = 0;
      NeedsUpdate = true;
    }

    public MonsterSpawner(EntityType type)
      : this(type, Vector2.Zero) { }

    public void AddSpawnable(EntityType type)
    {
      myTypes.Add(type);
    }

    public override void Update(float delta)
    {
      base.Update(delta);

      currentSpawnTime -= delta;

      if (myEnemies.Count < MaxActiveCount &&
          currentSpawnTime <= 0)
      {
        currentSpawnTime = SpawnTimeIntervalMs;
        SpawnMonster();
      }
    }

    public override void Draw(IsoBatch batch)
    {
      batch.Draw(
        ResourceManager.SpriteTextureBank.Query("Ball"),
        worldPosition,
        null,
        Color.White,
        0,
        new Vector2(16),
        1f,
        SpriteEffects.None,
        1f
      );
    }

    private void SpawnMonster()
    {
      int typeIndex = ResourceManager.Random.Next(0, myTypes.Count);
      EntityType t = myTypes[typeIndex];

      ActorData aData = new ActorData();

      switch (t)
      {
        case EntityType.Ogre:
          aData = ResourceManager.ActorDataBank.Query("Ogre");
          break;
        case EntityType.Player:
          aData = ResourceManager.ActorDataBank.Query("Player");
          break;
      }

      // Temporary test values for spawn offset.
      Vector2 offsetVec = new Vector2(
        ResourceManager.Random.Next(-7, 7),
        ResourceManager.Random.Next(-7, 7)
      );

      Enemy e = new Enemy(aData, worldPosition + offsetVec);
      myEnemies.Add(e);
      EntityManager.GetInstance().AddObject(e);
    }

    public override object GetData()
    {
      throw new NotImplementedException();
    }
  }
}
