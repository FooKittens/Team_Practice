using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teamcollab.DataSerialization
{
  [Serializable]
  public struct ApplicationSettings
  {
    public int ScreenHeight { get; private set; }
    public int ScreenWidth { get; private set; }


    /// <summary>
    /// Obtain a default initialized ApplicationSettings object.
    /// </summary>
    public static ApplicationSettings GetDefault()
    {
      ApplicationSettings appSettings;
      appSettings.ScreenWidth = 1280;
      appSettings.ScreenHeight = 720;

      return appSettings;
    }
  }
}
