using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Teamcollab.DataSerialization
{
  [Serializable]
  public struct SettingsData
  {
    public int ScreenHeight;
    public int ScreenWidth;
    public bool MultiThreading;
    public Keys DevConsoleKey;
    public bool DayNightCycleOn;


    /// <summary>
    /// Obtain a default initialized ApplicationSettings object.
    /// </summary>
    public static SettingsData GetDefault()
    {
      SettingsData appSettings = new SettingsData();
      appSettings.ScreenWidth = 1280;
      appSettings.ScreenHeight = 768;
      appSettings.MultiThreading = true;
      appSettings.DevConsoleKey = Keys.OemPipe;
      appSettings.DayNightCycleOn = true;
      return appSettings;
    }
  }
}
