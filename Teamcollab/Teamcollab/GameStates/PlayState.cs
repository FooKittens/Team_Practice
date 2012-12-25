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

    enum Direction
    {
      Up,
      Down,
      Left,
      Right
    }
    private void MoveCameraOneCluster(Direction dir)
    {
      Coordinates change = Coordinates.Zero;
      switch (dir)
      {
        case Direction.Up:
          change.Y = -1;
          break;
        case Direction.Down:
          change.Y = 1;
          break;
        case Direction.Left:
          change.X = -1;
          break;
        case Direction.Right:
          change.X = 1;
          break;
      }
      Vector2 camCluster = WorldManager.TransformScreenToCluster(Camera2D.TargetPosition);
      change.X += Convert.ToInt32(camCluster.X);
      change.Y += Convert.ToInt32(camCluster.Y);
      Camera2D.SetTargetPosition(WorldManager.GetClusterScreenCenter(change));
    }

    private void MoveCameraOneTile(Direction dir)
    {
      Coordinates change = Coordinates.Zero;
      switch (dir)
      {
        case Direction.Up:
          change.Y = -1;
          break;
        case Direction.Down:
          change.Y = 1;
          break;
        case Direction.Left:
          change.X = -1;
          break;
        case Direction.Right:
          change.X = 1;
          break;
      }
      Vector2 camTile = WorldManager.TransformScreenToTile(Camera2D.TargetPosition);
      change.X += Convert.ToInt32(camTile.X);
      change.Y += Convert.ToInt32(camTile.Y);
      Camera2D.SetTargetPosition(WorldManager.GetTileScreenPosition(change));
    }

    public override void Update(GameTime gameTime)
    {
      Camera2D.Update(gameTime);
      worldManager.Update(gameTime);

      if (Settings.DayNightCycleOn)
      {
        t = (float)gameTime.TotalGameTime.TotalSeconds;
        inten = (float)Math.Sin(t) / 2 + 0.75f;
      }
      else
      {
        inten = 1f;
      }
      

      if (InputManager.KeyNewDown(Keys.W))
      {
        if (InputManager.KeyDown(Keys.LeftShift))
        {
          MoveCameraOneCluster(Direction.Up);
        }
        else
        {
          MoveCameraOneTile(Direction.Up);
        }
      }
      else if (InputManager.KeyNewDown(Keys.A))
      {
        if (InputManager.KeyDown(Keys.LeftShift))
        {
          MoveCameraOneCluster(Direction.Left);
        }
        else
        {
          MoveCameraOneTile(Direction.Left);
        }
      }
      else if (InputManager.KeyNewDown(Keys.S))
      {
        if (InputManager.KeyDown(Keys.LeftShift))
        {
          MoveCameraOneCluster(Direction.Down);
        }
        else
        {
          MoveCameraOneTile(Direction.Down);
        }
      }
      else if (InputManager.KeyNewDown(Keys.D))
      {
        if (InputManager.KeyDown(Keys.LeftShift))
        {
          MoveCameraOneCluster(Direction.Right);
        }
        else
        {
          MoveCameraOneTile(Direction.Right);
        }
      }

      float deltaScroll = InputManager.MouseWheelChange();

      deltaScroll = Math.Sign(InputManager.MouseWheelChange()) * 2.5f;
      Camera2D.SetTargetScale(Camera2D.Scale + deltaScroll);

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

      Matrix iso = Matrix.Identity;      

      testShader.Parameters["AmbientIntensity"].SetValue(inten);

      testShader.Parameters["World"].SetValue(iso);

      

      testShader.Parameters["View"].SetValue(Camera2D.View);

      testShader.Parameters["Projection"].SetValue(Camera2D.Projection);

      RasterizerState rs = new RasterizerState();
      rs.FillMode = FillMode.WireFrame;

      spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, testShader);
      //spriteBatch.Begin();
      worldManager.Draw(spriteBatch);
      spriteBatch.End();
    }
  }
}
