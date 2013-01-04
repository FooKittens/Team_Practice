using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Midgard.GameObjects
{
  class Spawner : Entity
  {
    #region Properties

    #endregion

    #region Members

    #endregion
    public Spawner(EntityType type, Vector2 worldPosition)
      : base(type, worldPosition)
    {

    }

    public Spawner(EntityType type)
      : this(type, Vector2.Zero) { }

    public override void Draw(Engine.Helpers.IsoBatch batch)
    {
      throw new NotImplementedException();
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
