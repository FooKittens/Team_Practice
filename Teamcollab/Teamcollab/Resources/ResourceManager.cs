﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Midgard.Resources
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

      #region Characters
      // Resource query key needs to match EntityType if used for base animation
      SpriteTextureBank.Add("Ogre", content.Load<Texture2D>("Art\\Characters\\ogre"));
      SpriteTextureBank.Add("Goblin", content.Load<Texture2D>("Art\\Characters\\goblin"));
      SpriteTextureBank.Add("Slime", content.Load<Texture2D>("Art\\Characters\\slime"));
      SpriteTextureBank.Add("Werewolf", content.Load<Texture2D>("Art\\Characters\\werewolf"));
      SpriteTextureBank.Add("Zombie", content.Load<Texture2D>("Art\\Characters\\zombie"));
      SpriteTextureBank.Add("Player", content.Load<Texture2D>("Art\\Characters\\male_unarmored"));
      SpriteTextureBank.Add("MaleLight", content.Load<Texture2D>("Art\\Characters\\male_light"));
      SpriteTextureBank.Add("MaleHeavy", content.Load<Texture2D>("Art\\Characters\\male_heavy"));
      #endregion

      #region Items
      SpriteTextureBank.Add("Sword", content.Load<Texture2D>("Art\\Items\\male_longsword"));
      SpriteTextureBank.Add("Bow", content.Load<Texture2D>("Art\\Items\\male_longbow"));
      SpriteTextureBank.Add("Shield", content.Load<Texture2D>("Art\\Items\\male_shield"));
      SpriteTextureBank.Add("Staff", content.Load<Texture2D>("Art\\Items\\male_staff"));
      #endregion
      //SpriteTextureBank.Add("Grassland", content.Load<Texture2D>("Art\\grassland_tiles"));
    }
  }
}
