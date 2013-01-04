using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midgard.Engine.Animation;
using Microsoft.Xna.Framework;

namespace Midgard.GameObjects.NPC
{
  class Enemy : Actor
  {
    public Enemy(ActorData initializer, Vector2 worldposition)
      : base(initializer, worldposition)
    {
      movementSpeed = 0.02f;
      NeedsUpdate = true;
    }
    public Enemy(ActorData initializer)
      : this(initializer, Vector2.Zero) { }

    protected override void UpdateInput()
    {
      
    }

    public override void Update(float deltaTime)
    {
      Entity e = EntityManager.GetInstance().GetFirstOccurencyOfType(EntityType.Player);

      targetPosition = e.WorldPosition;

      base.Update(deltaTime);
    }

    public override object GetData()
    {
      throw new NotImplementedException();
    }
  }
}
