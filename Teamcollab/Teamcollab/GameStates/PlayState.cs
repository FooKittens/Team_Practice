using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Engine;
using Microsoft.Xna.Framework.Input;
using Teamcollab.Engine.Helpers;
using Teamcollab.Engine.World;
using Teamcollab.GUI;

namespace Teamcollab.GameStates
{
  /// <summary>
  /// Handles the in-game logic
  /// </summary>
  sealed class PlayState : GameState
  {
    #region Properties
    #endregion

    #region Events
    public override event StateChangeRequestHandler StateChangeRequested;
    #endregion

    #region Members
    SpriteBatch spriteBatch;
    WorldManager worldManager;
    #endregion

    public PlayState(Game game)
      :base(game, ApplicationState.Play)
    {
      // Initialize the gamestate.
      Initialize();
    }

    protected override void Initialize()
    {
      spriteBatch = new SpriteBatch(Game.GraphicsDevice);
      worldManager = new WorldManager(Game);
    }

    public override void Update(GameTime gameTime)
    {
      worldManager.Update(gameTime);

      if (InputManager.KeyRelease(Keys.Escape))
      {
        if (StateChangeRequested != null)
        {
          StateChangeRequested(ApplicationState.Menu);
        }
      }

    }

    public override void Draw()
    {
      Game.GraphicsDevice.Clear(Color.DarkRed);
      //spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, new Camera2D(new Vector2(1280, 720)).Transform);
      worldManager.Draw(spriteBatch);
      //spriteBatch.End();
    }
  }
}
