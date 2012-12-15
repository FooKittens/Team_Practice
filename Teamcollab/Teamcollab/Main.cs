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
	}
}
