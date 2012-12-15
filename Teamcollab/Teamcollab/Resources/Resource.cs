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
    public T Data { get; private set; }
    public string Key { get; private set; }

    public delegate void ResourceDoneManager();

    public event ResourceDoneManager resourceDone;

    public Resource(T resource, string key)
    {
      this.Data = resource;
      this.Key = key;
    }

    public bool HasValue()
    {
      return Data != null;
    }
  }
}