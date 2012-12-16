using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Teamcollab.Managers;
using Teamcollab.Engine;
using Teamcollab.DataSerialization;
using System.IO;
using Teamcollab.Engine.Helpers;

namespace Teamcollab
{
	public class Main : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
    StateManager stateManager;

		public Main()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

      // Loads settings from Settings.Xml
      LoadSettings();

      graphics.PreferredBackBufferWidth = Settings.ScreenWidth;
      graphics.PreferredBackBufferHeight = Settings.ScreenHeight;
      IsMouseVisible = true;
		}

		protected override void Initialize()
		{
      stateManager = new StateManager(this);
			base.Initialize();
		}

		protected override void LoadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
      InputManager.Update();
      stateManager.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

      stateManager.Draw();

			base.Draw(gameTime);
		}

    /// <summary>
    /// Initializes the static Settings class,
    /// should be called in constructor of Main.
    /// If there is no settings file, a new one will be
    /// created when in debug mode.
    /// </summary>
    private void LoadSettings()
    {
      SettingsData data;
      try
      {
        data = DataSerializer.DeSerializeXml<SettingsData>(
          Constants.SettingsPath
        );
      }
      catch (FileNotFoundException)
      {
      #if DEBUG
        // Obtain a default object to initialize settings with.
        data = SettingsData.GetDefault();

        // Serialize the new data to avoid file missing next time.
        DataSerializer.SerializeXml<SettingsData>(
          data,
          Constants.SettingsPath,
          FileMode.Create
        );
      #else
        // We want to notice problems in the release build.
        throw;
      #endif
      }

      Settings.Initialize(data);
    }
	}
}
