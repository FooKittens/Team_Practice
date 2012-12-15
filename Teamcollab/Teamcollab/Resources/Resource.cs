using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teamcollab.Resources
{
  /// <summary>
  /// A dynamically loaded object
  /// </summary>
  /// <typeparam name="T"></typeparam>
  class Resource<T>
  {
    public T Value { get; private set; }
    public string Key { get; private set; }

    public delegate void ResourceDoneManager(Resource<T> self);

    public event ResourceDoneManager ResourceDone;

    public Resource(string key, T resource)
    {
      this.Value = resource;
      this.Key = key;
    }

    public bool HasValue()
    {
      return Value != null;
    }

    public void Done()
    {
      if (ResourceDone != null)
      {
        ResourceDone(this);
      }
    }

    public static implicit operator T(Resource<T> me)
    {
      if (me == null)
      {
        throw new NullReferenceException(
          string.Format(
          "Implicit conversion to {0} from Resource<T>" + 
          "attempted when Resource was null.",
          typeof(T).ToString()
          )
        );
      }

      return me.Value;
    }
  }
}