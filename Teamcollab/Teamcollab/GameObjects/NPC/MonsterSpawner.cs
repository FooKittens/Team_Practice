using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Midgard.GameObjects.NPC
{
  class MonsterSpawner : Spawner
  {
    #region Properties
    public int MaxActiveCount { get; set; }
    public int SpawnTimeIntervalMs { get; set; }
    #endregion

    #region Members
    List<EntityType> myTypes;
    List<Enemy> myEnemies;
    #endregion

    public MonsterSpawner(EntityType type, Vector2 worldPosition)
      : base(type, worldPosition)
    {

    }

    public MonsterSpawner(EntityType type)
      : this(type, Vector2.Zero) { }


    public override void Update(float delta)
    {
      base.Update(delta);
    }
  }
}
