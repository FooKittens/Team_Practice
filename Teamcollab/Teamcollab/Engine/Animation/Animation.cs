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
    Undefined = 0,

    Idle,
    Walking,
    Running,
    Melee,
    Ranged,
    TargetSpell,
    OmniSpell,
    Dying,
  }

  [Serializable]
  public struct AnimationData
  {
    public AnimationType Identifier;
    public AnimationDirection Direction;
    public string ResourceKey;
    public Point2D FrameSize;
    public Point2D Offset;
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
    Point2D frameSize;
    Point2D offset;
    int frameCount;
    int timeInMilliSeconds;

    int timePerFrameInMs;
    Point2D currentFrame;
    #endregion

    public Animation(string resourceKey, AnimationType identifier,
      AnimationDirection direction, Point2D frameSize,
      int frameCount, Point2D offset, int timeInMilliSeconds)
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

    public void Update(float deltaTime)
    {

    }
  }
}