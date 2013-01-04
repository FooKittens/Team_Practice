using Microsoft.Xna.Framework;
using Midgard.Engine.Helpers;

namespace Midgard.GameObjects
{
  struct EntityData
  {

  }

  /// <summary>
  /// A blueprint for entities
  /// </summary>
  public abstract class Entity
  {
    #region Properties
    public Vector2 WorldPosition { get { return worldPosition; } }
    public EntityType EntityType { get; protected set; }
    public bool NeedsUpdate { get; protected set; }
    #endregion

    #region Members
    protected bool isDead;
    protected Vector2 worldPosition;
    #endregion

    public Entity(EntityType type, Vector2 worldPosition)
    {
      this.worldPosition = worldPosition;
      this.EntityType = type;
    }

    public Entity(EntityType type)
      : this(type, Vector2.Zero) { }

    /// <summary>
    /// Used by the entity to update itself
    /// </summary>
    public virtual void Update(float deltaTime)
    {
      UpdateState();
    }

    public abstract void Draw(IsoBatch batch);

    protected abstract void UpdateState();

    /// <summary>
    /// Destroys the entity and sets IsDead to true
    /// </summary>
    public virtual void Destroy()
    {
      isDead = true;
    }

    /// <summary>
    /// Supposed to return a struct of data, cast to object
    /// </summary>
    public abstract object GetData();
  }
}
