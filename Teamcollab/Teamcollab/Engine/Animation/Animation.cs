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
    public Rectangle Source { get { return source; } }
    public Resource<Texture2D> TextureResource { get; protected set; }
    #endregion

    #region Members
    // Timing variables
    int timeInMilliSeconds;
    int timePerFrameInMs;
    float countdownTimer;

    // Frame variables
    string resourceKey;
    Coordinates offset;
    Coordinates frameSize;
    int frameCount;
    int currentFrame;
    Rectangle source;
    #endregion

    public Animation(string resourceKey, AnimationType identifier,
      AnimationDirection direction, Coordinates frameSize,
      int frameCount, Coordinates offset, int timeInMilliSeconds)
    {
      // Sets identifiers
      Identifier = identifier;
      Direction = direction;

      // Sets texture
      this.resourceKey = resourceKey;
      TextureResource = ResourceManager.SpriteTextureBank.Query(resourceKey);
      Texture2D texture = TextureResource.Value;

      // Sets source
      this.frameSize = frameSize;
      this.offset = offset;
      source = new Rectangle(offset.X, offset.Y, frameSize.X, frameSize.Y);

      // Sets timing variables
      this.timeInMilliSeconds = timeInMilliSeconds;
      timePerFrameInMs = timeInMilliSeconds / frameCount;
      countdownTimer = timePerFrameInMs;

      // Sets frame variables
      currentFrame = 0;
      this.frameCount = frameCount;
    }

    /// <summary>
    /// Updates the animation (U surprised?)
    /// </summary>
    /// <param name="deltaTime">Milliseconds since last frame</param>
    public void Update(float deltaTime)
    {
      countdownTimer -= deltaTime;
      // Changes frame
      while (countdownTimer < 0)
      {
        ++currentFrame;
        countdownTimer += timePerFrameInMs;
        source.X += frameSize.X;
        // Jumps to next line in texture
        if (source.X > TextureResource.Value.Width)
        {
          source.X = 0;
          source.Y += frameSize.Y;
        }
        // Restarts animation
        if (currentFrame > frameCount)
        {
          currentFrame = 0;
          source.X = offset.X;
          source.Y = offset.Y;
        }
      }
    }
  }
}