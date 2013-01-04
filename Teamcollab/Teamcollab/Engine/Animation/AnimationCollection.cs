using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Midgard.Engine.Animation
{
  class AnimationCollection
  {
    #region Properties

    #endregion

    #region Members
    List<Animation> animations;
    #endregion

    public AnimationCollection()
    {
      animations = new List<Animation>();
    }

    /// <summary>
    /// Returns an animation based on the input direction and type.
    /// In the event no such animation exists, null will be returned.
    /// </summary>
    public Animation GetAnimation(AnimationDirection direction,
      AnimationType type)
    {
      Animation anim = animations.Find(delegate(Animation a)
        {
          return a.Direction == direction && a.Identifier == type;
        }
      );

      return anim;
    }

    public void Add(Animation animation)
    {
      animations.Add(animation);
    }

    public void Remove(Animation animation)
    {
      animations.Remove(animation);
    }

    public void RemoveAt(int index)
    {
      animations.RemoveAt(index);
    }
  }
}
