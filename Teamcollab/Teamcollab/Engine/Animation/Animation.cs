using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midgard.Engine.Helpers;
using Microsoft.Xna.Framework;
using Midgard.Resources;
using Microsoft.Xna.Framework.Graphics;

namespace Midgard.Engine.Animation
{
  public enum AnimationDirection
  {
    Undefined = 0,

    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest
  }

  public enum AnimationType
  {
    Unidentified = 0,

    Idle,
    Walking,
    Running,
    Melee,
    Ranged,
    TargetSpell,
    OmniSpell,
    Dying,
  }

  struct AnimationStruct
  {
    public AnimationType Identifier;
    public AnimationDirection Direction;
    public string ResourceKey;
    public Coordinates FrameSize;
    public Coordinates Offset;
    public int FrameCount;
    public int TimeInMilliSeconds;
  }

  class Animation
  {
    #region Properties
    public AnimationType Identifier { get; protected set; }
    public AnimationDirection Direction { get; protected set; }
    public Rectangle Source { get; protected set; }
    public Resource<Texture2D> Texture { get; protected set; }
    #endregion

    #region Members
    string resourceKey;
    Coordinates frameSize;
    Coordinates offset;
    int frameCount;
    int timeInMilliSeconds;

    int timePerFrameInMs;
    Coordinates currentFrame;
    #endregion

    public Animation(string resourceKey, AnimationType identifier,
      AnimationDirection direction, Coordinates frameSize,
      int frameCount, Coordinates offset, int timeInMilliSeconds)
    {
      Identifier = identifier;
      Direction = direction;

      this.resourceKey = resourceKey;
      Texture = ResourceManager.SpriteTextureBank.Query(resourceKey);

      this.frameSize = frameSize;
      this.frameCount = frameCount;
      this.offset = offset;
      this.timeInMilliSeconds = timeInMilliSeconds;

      Source = new Rectangle(offset.X, offset.Y, frameSize.X, frameSize.Y);
    }

    public void Update(GameTime gameTime)
    {

    }
  }
}