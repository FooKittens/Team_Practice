using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Midgard.GameObjects
{
  // TODO(Peter): Move elsewhere
  public struct EnemyData
  {
    public EntityType Type;

    public int BaseHealth;
    public int BaseMana;
    public int BaseStrength;
    public int BaseDexterity;
    public int BaseIntelligence;
    public int BaseVitality;
    public int BaseWisdom;
    public int BaseAttack;
  }

  abstract class Enemy : Actor
  {
    public Enemy(EnemyData initializer)
    :base(initializer.Type) 
    { 

    }

    private void Initialize(EnemyData data)
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
