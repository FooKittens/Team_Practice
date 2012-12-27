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
    public static ResourceCollection<Texture2D> SpriteTextureBank { get; private set; }

    #endregion

    #region Members
    static ContentManager content;
    #endregion

    public static void Initialize(ContentManager content)
    {
      ResourceManager.content = content;
      TileTextureBank = new ResourceCollection<Texture2D>();

      TileTextureBank.Add("Grass", content.Load<Texture2D>("Art\\grassIso"));
      TileTextureBank.Add("Water", content.Load<Texture2D>("Art\\waterIso"));
      TileTextureBank.Add("Stone", content.Load<Texture2D>("Art\\stoneIso"));

      SpriteTextureBank = new ResourceCollection<Texture2D>();
      SpriteTextureBank.Add("Pine", content.Load<Texture2D>("Art\\IHASTREE"));
      SpriteTextureBank.Add("Ogre", content.Load<Texture2D>("Art\\ogre"));
      //SpriteTextureBank.Add("Grassland", content.Load<Texture2D>("Art\\grassland_tiles"));
    }
  }
}
