using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teamcollab.DataSerialization;

namespace Teamcollab
{
  /// <summary>
  /// Settings class for accessing application settings globally.
  /// </summary>
  public static class Settings
  {
    public static int ScreenWidth { get; private set; }
    public static int ScreenHeight { get; private set; }


    /// <summary>
    /// Initializes the globally accessible Settings class with a data object.
    /// </summary>
    /// <param name="data"></param>
    public static void Initialize(SettingsData data)
    {
      ScreenWidth = data.ScreenWidth;
      ScreenHeight = data.ScreenHeight;
    }
  }
}
