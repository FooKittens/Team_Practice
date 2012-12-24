using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Teamcollab.Resources
{
  static class ResourceManager
  {
    #region Properties
    public static ResourceCollection<Texture2D> TileTextureBank { get; private set; }

    #endregion

    #region Members
    static ContentManager content;
    #endregion

    public static void Initialize(ContentManager content)
    {
      ResourceManager.content = content;
      TileTextureBank = new ResourceCollection<Texture2D>();

      TileTextureBank.Add("Square", content.Load<Texture2D>("Art\\square"));
      TileTextureBank.Add("Grass", content.Load<Texture2D>("Art\\grass32x32"));
      TileTextureBank.Add("Water", content.Load<Texture2D>("Art\\WaterTile"));
      TileTextureBank.Add("Stone", content.Load<Texture2D>("Art\\stone"));
    }
  }
}
