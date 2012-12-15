using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teamcollab.DataSerialization
{
  [Serializable]
  public struct SettingsData
  {
    public int ScreenHeight;
    public int ScreenWidth;


    /// <summary>
    /// Obtain a default initialized ApplicationSettings object.
    /// </summary>
    public static SettingsData GetDefault()
    {
      SettingsData appSettings = new SettingsData();
      appSettings.ScreenWidth = 1280;
      appSettings.ScreenHeight = 720;

      return appSettings;
    }
  }
}
