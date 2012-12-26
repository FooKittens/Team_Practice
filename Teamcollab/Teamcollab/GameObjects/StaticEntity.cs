using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Engine.Helpers;
using Teamcollab.Resources;

namespace Teamcollab.GameObjects
{
  class StaticEntity : Entity
  {
    #region Properties

    #endregion

    #region Members
    Resource<Texture2D> texture;
    #endregion

    public StaticEntity(EntityType type, Vector2 worldPos)
      : base(type, worldPos)
    {
      texture = GetResource(EntityType);
    }

    public override void Draw(IsoBatch batch)
    {
      batch.Draw(texture, WorldPosition, null, Color.White,
        0f, new Vector2(64, 64), 1f, SpriteEffects.None, 0f);
    }

    public static Resource<Texture2D> GetResource(EntityType type)
    {
      Resource<Texture2D> res = null;
      switch (type)
      {
        case EntityType.Player:
          break;
        case EntityType.Tree:
          res = ResourceManager.SpriteTextureBank.Query("Pine");
          break;
        default:
          throw new ArgumentException("EntityType undefined.");
      }

      return res;
    }

    protected override void UpdateState()
    {
      throw new NotImplementedException();
    }

    public override object GetData()
    {
      throw new NotImplementedException();
    }
  }
}
