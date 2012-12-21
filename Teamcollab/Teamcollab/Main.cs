using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.DataSerialization;
using Teamcollab.Engine.Helpers;
using Teamcollab.Managers;
using Teamcollab.Resources;
using System;
using Teamcollab.Engine;
using Microsoft.Xna.Framework.Input;

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
      ResourceManager.Initialize(Content);
      DevConsole.Initialize(this);
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

      // Dev Console test code
      if (InputManager.KeyNewDown(Keys.C))
      {
        DevConsole.Visible = !DevConsole.Visible;
      }

      if (InputManager.KeyNewDown(Keys.X))
      {
        DevConsole.WriteLine("Testing {0}, {1} and {2}", 1, 2, 3);
      }
      if (InputManager.KeyNewDown(Keys.Z))
      {
        DevConsole.WriteLine("TROLOLOLOL");
      }

      if (InputManager.KeyNewDown(Keys.F4))
      {
        graphics.IsFullScreen = !graphics.IsFullScreen;
        graphics.ApplyChanges();
      }

      DevConsole.Update(gameTime);
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

      stateManager.Draw();

      ImmediateDrawer.GetInstance(this).Draw();
      DevConsole.Draw();
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
      #if DEBUG
      Settings.Initialize(SettingsData.GetDefault());
      return;
      #endif

      bool error = false;

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
