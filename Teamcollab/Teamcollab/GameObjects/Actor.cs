using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Midgard.Engine.Helpers;

namespace Midgard.GameObjects
{
  abstract class Actor : Entity
  {
    #region Properties
    public float Speed { get; protected set; }
    public int Strength { get; protected set; }
    public int Dexterity { get; protected set; }
    public int Intelligence { get; protected set; }
    public int Vitality { get; protected set; }
    public int Wisdom { get; protected set; }
    public int Attack { get; protected set; }

    #endregion

    #region Members
    int baseHealth;
    int baseMana;
    int baseStrength;
    int baseDexterity;
    int baseIntelligence;
    int baseVitality;
    int baseWisdom;
    int baseAttack;

    // Movement
    protected Vector2 targetPosition;
    protected float movementSpeed;

    #endregion

    public Actor(EntityType type, Vector2 worldPosition)
      :base(type, worldPosition)
    {

    }

    public Actor(EntityType type)
      : this(type, Vector2.Zero) {}

    // TODO(Peter): Apply modifiers
    #region StatsAndRolls

    public int Health
    {
      get { return baseHealth; }
    }
    public int Mana
    {
      get { return baseMana; }
    }
    public int Damage
    {
      get { return baseAttack; }
    }

    #endregion

    /// <summary>
    /// Updates the entity
    /// </summary>
    public override void Update(float deltaTime)
    {
      UpdateInput();
      UpdateMovement(deltaTime);
      base.Update(deltaTime);
    }

    protected abstract void UpdateInput();

    protected virtual void UpdateMovement(float deltaTime)
    {
      Vector2 diff = targetPosition - worldPosition;
      if (diff.LengthSquared() < (movementSpeed * movementSpeed))
      {
        worldPosition = targetPosition;
      }
      else
      {
        // Movement
        diff.Normalize();
        diff *= movementSpeed;
        worldPosition += diff;
      }
    }

    public override void Draw(IsoBatch batch)
    {
      
    }
  }
}
