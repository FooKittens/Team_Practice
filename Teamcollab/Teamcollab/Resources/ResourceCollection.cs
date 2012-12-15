using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teamcollab.Resources
{
  /// <summary>
  /// Manages a collection of resources of type T.
  /// </summary>
  /// <typeparam name="T">The resource type to manage.</typeparam>
  class ResourceCollection<T>
  {
    List<Resource<T>> resources;

    public ResourceCollection()
    {
      resources = new List<Resource<T>>();
    }

    /// <summary>
    /// Adds a resource to the collection.
    /// </summary>
    /// <param name="data">Resource of type T</param>
    /// <param name="resourceKey">The key to identify the resource with</param>
    public void Add(T data, string resourceKey)
    {
      Resource<T> res = new Resource<T>();
    }

    /// <summary>
    /// Attempts to query the resource collection for a specific
    /// resource identified by the resource key.
    /// </summary>
    /// <param name="resourceKey">The resource identifier.</param>
    public Resource<T> Query(string resourceKey)
    {
      Resource<T> res = resources.Find(
        delegate(Resource<T> resource)
        {
          // If the resource key matches the requested key we got a match.
          if (resource.Key == resourceKey)
          {
            return true;
          }

          return false;
        }
      );

      return res;
    }


  }
}
