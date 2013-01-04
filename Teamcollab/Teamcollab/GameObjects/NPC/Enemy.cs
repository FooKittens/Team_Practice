using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midgard.Engine.Animation;
using Microsoft.Xna.Framework;

namespace Midgard.GameObjects.NPC
{
  abstract class Enemy : Actor
  {
    public Enemy(ActorData initializer)
    :base(initializer.Type) 
    { 

    }

    private void Initialize(ActorData data)
    {
      base.EntityType = data.Type;
      base.Strength = data.BaseStrength;
      base.Dexterity = data.BaseDexterity;
      base.Intelligence = data.BaseIntelligence;
      base.Vitality = data.BaseVitality;
      base.Wisdom = data.BaseWisdom;
    }
  }
}
