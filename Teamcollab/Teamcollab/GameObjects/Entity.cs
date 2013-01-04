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

    /// <summary>
    /// Used by the entity to update itself
    /// </summary>
    /// <param name="gameTime"></param>
    public virtual void Update(GameTime gameTime)
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
