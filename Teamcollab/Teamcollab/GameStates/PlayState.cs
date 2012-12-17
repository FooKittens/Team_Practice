﻿using System;
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

    Effect testShader;

    public PlayState(Game game)
      :base(game, ApplicationState.Play)
    {
      // Initialize the gamestate.
      Initialize();

      testShader = game.Content.Load<Effect>("BasicShader");
    }

    protected override void Initialize()
    {
      spriteBatch = new SpriteBatch(Game.GraphicsDevice);
      worldManager = WorldManager.GetInstance(Game);
    }

    float inten = 0f;
    float t;

    public override void Update(GameTime gameTime)
    {
      Camera2D.Update(gameTime);
      worldManager.Update(gameTime);

      t = (float)gameTime.TotalGameTime.TotalSeconds;
      inten = (float)Math.Sin(t) / 2 + 0.75f;

      if (InputManager.KeyDown(Keys.W))
      {
        Camera2D.SetTargetPosition(WorldManager.TransformByCluster(new Vector2(-4, -4), Vector2.Zero));
      }
      else if (InputManager.KeyDown(Keys.S))
      {
        
        Camera2D.SetTargetPosition(WorldManager.GetTileScreenPosition(new Vector2(-12, -12)));

        //Camera2D.SetTargetPosition(
        //  Vector2.Transform(
        //    new Vector2(1, 0),
        //    Matrix.CreateScale(
        //      Constants.TileWidth,
        //      Constants.TileHeight,
        //      1
        //    ) *
        //    Matrix.CreateScale(
        //      Constants.ClusterWidth,
        //      Constants.ClusterHeight,
        //      1
        //    )
        //  )
        //); // TODO Get WorldManager transform method
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
      Game.GraphicsDevice.Clear(Color.DimGray);

      testShader.Parameters["AmbientIntensity"].SetValue(inten);

      testShader.Parameters["World"].SetValue(Matrix.Identity);

      testShader.Parameters["View"].SetValue(Camera2D.View);

      testShader.Parameters["Projection"].SetValue(Camera2D.Projection);

      spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, testShader);
      worldManager.Draw(spriteBatch);
      spriteBatch.End();
    }
  }
}
