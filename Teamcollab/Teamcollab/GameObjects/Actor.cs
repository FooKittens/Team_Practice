using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Midgard.Engine.Helpers;
using Midgard.Engine.Animation;
using Midgard.GameObjects.NPC;

namespace Midgard.GameObjects
{
  // TODO(Peter): Move elsewhere
  [Serializable]
  public struct ActorData
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

    public AnimationData[] Animations;
  }

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
    protected bool isMoving;

    // Animation
    AnimationCollection animCollection;
    protected AnimationDirection animationDirection;
    protected AnimationType currentAnimType;
    #endregion

    public Actor(ActorData init, Vector2 worldPosition)
      :base(init.Type, worldPosition)
    {
      animCollection = new AnimationCollection();
      foreach (AnimationData a in init.Animations)
      {
        animCollection.Add(
          new Animation(
            a.ResourceKey,
            a.Identifier,
            a.Direction,
            a.FrameSize,
            a.FrameCount,
            a.Offset,
            a.TimeInMilliSeconds  
          )
        );
      }
    }

    public Actor(ActorData init)
      : this(init.Type, Vector2.Zero) {}

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
      if (diff.LengthSquared() <= (movementSpeed * movementSpeed))
      {
        isMoving = false;

        worldPosition = targetPosition;
      }
      else
      {
        // Animation
        isMoving = true;
        float direction = (float)Math.Atan2(diff.Y, diff.X);
        float percentualDir = direction / MathHelper.TwoPi + 0.5f;
        int generalDirection = Convert.ToInt32(
          percentualDir *= Constants.AnimationDirectionCount
        );
        animationDirection = (AnimationDirection)((generalDirection + 1) %
          Constants.AnimationDirectionCount
        );

        // Movement
        diff.Normalize();
        diff *= movementSpeed;
        worldPosition += diff;
      }
    }

    public override void Draw(IsoBatch batch)
    {
      Animation anim = animCollection.GetAnimation(animationDirection, currentAnimType);
      batch.Draw(anim.TextureResource, worldPosition, anim.Source, Color.White);
    }
  }
}
