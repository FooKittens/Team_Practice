using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Engine;
using Microsoft.Xna.Framework.Input;
using Teamcollab.Engine.Helpers;
using Teamcollab.Engine.WorldManagement;
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
      Camera2D.Update(gameTime);
      worldManager.Update(gameTime);

      if (InputManager.KeyDown(Keys.W))
      {
        Camera2D.SetTargetPosition(
          Vector2.Transform(
            new Vector2(0, 0),
            Matrix.CreateScale(
              Constants.TileWidth,
              Constants.TileHeight,
              1
            ) *
            Matrix.CreateScale(
              Constants.ClusterWidth,
              Constants.ClusterHeight,
              1
            )
          )
        );
      }
      else if (InputManager.KeyDown(Keys.S))
      {
        Camera2D.SetTargetPosition(
          Vector2.Transform(
            new Vector2(10, 0),
            Matrix.CreateScale(
              Constants.TileWidth,
              Constants.TileHeight,
              1
            ) *
            Matrix.CreateScale(
              Constants.ClusterWidth,
              Constants.ClusterHeight,
              1
            )
          )
        ); // TODO Get WorldManager transform method
      }

      if (InputManager.KeyDown(Keys.A))
      {
        Camera2D.SetTargetScale(20);
      }
      if (InputManager.KeyDown(Keys.D))
      {
        Camera2D.SetTargetScale(1);
      }

      if (InputManager.KeyDown(Keys.E))
      {
        Camera2D.SetTargetScale(1f);
      }
      else if (InputManager.KeyDown(Keys.R))
      {
        Camera2D.SetTargetScale(2.5f);
      }

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
      Game.GraphicsDevice.Clear(Color.Black);
      spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera2D.Transform);
      worldManager.Draw(spriteBatch);
      spriteBatch.End();
    }
  }
}
