using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Midgard.Engine.Helpers;
using Midgard.Engine.WorldManagement;
using Midgard.GUI;
using Midgard.GameObjects;

namespace Midgard.GameStates
{
  class GameManager
  {
    #region Properties
    /// <summary>
    /// A reference to the game object that uses the state.
    /// </summary>
    protected Game Game { get; set; }
    #endregion

    #region Members
    // Utils
    IsoBatch spriteBatch;
    WorldManager worldManager;

    // Cam
    Direction previousDirection;
    bool lockCamToPlayer;

    // Time
    float inten = 0f;
    float t;
    #endregion

    Effect testShader;

    public GameManager(Game game)
    {
      // Initialize the gamestate.
      Game = game; 
      Initialize();
      EntityManager.GetInstance().AddObject(new Player(Vector2.Zero));

      testShader = game.Content.Load<Effect>("BasicShader");
    }

    protected void Initialize()
    {
      spriteBatch = new IsoBatch(Game.GraphicsDevice);
      worldManager = WorldManager.GetInstance(Game);
    }

    #region Camera utils
    [Flags]
    enum Direction
    {
      Up = 1,
      Down = 2,
      Left = 4,
      Right = 8,
    }
    private void MoveCameraOneCluster(Direction dir)
    {
      Vector2 change = Vector2.Zero;
      if (((int)dir & 1) == 1)
      {
        change.Y -= 1;
      }
      if (((int)dir & 2) == 2)
      {
        change.Y += 1;
      }
      if (((int)dir & 4) == 4)
      {
        change.X -= 1;
      }
      if (((int)dir & 8) == 8)
      {
        change.X += 1;
      }

      Vector2 camCluster = WorldManager.TransformScreenToCluster(
        Camera2D.TargetPosition
      );
      change.X += Convert.ToInt32(camCluster.X);
      change.Y += Convert.ToInt32(camCluster.Y);
      change = WorldManager.GetClusterWorldCenter(change);
      Camera2D.SetTargetPosition(WorldManager.TransformInvIsometric(change));
    }

    private void MoveCameraOneTile(Direction dir)
    {
      Coordinates change = Coordinates.Zero;
      if (((int)dir & 1) == 1)
      {
        change.Y -= 1;
      }
      if (((int)dir & 2) == 2)
      {
        change.Y += 1;
      }
      if (((int)dir & 4) == 4)
      {
        change.X -= 1;
      }
      if (((int)dir & 8) == 8)
      {
        change.X += 1;
      }

      Vector2 camTile = WorldManager.TransformScreenToWorld(
        Camera2D.TargetPosition
      );
      change.X += Convert.ToInt32(camTile.X);
      change.Y += Convert.ToInt32(camTile.Y);
      Camera2D.SetTargetPosition(WorldManager.TransformInvIsometric(change));
    }
    #endregion

    public void Update(GameTime gameTime)
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

      #region Camera
      Direction currentDirection = (Direction)0;

      if (InputManager.KeyDown(Keys.W))
      {
        currentDirection += (int)Direction.Up;
      }
      if (InputManager.KeyDown(Keys.A))
      {
        currentDirection += (int)Direction.Left;
      }
      if (InputManager.KeyDown(Keys.S))
      {
        currentDirection += (int)Direction.Down;
      }
      if (InputManager.KeyDown(Keys.D))
      {
        currentDirection += (int)Direction.Right;
      }
      if (currentDirection != 0) // Only move if there's input
      {
        if (InputManager.KeyDown(Keys.LeftShift))
        {
          if (previousDirection != currentDirection)
          {
            MoveCameraOneCluster(currentDirection);
          }
        }
        else
        {
          MoveCameraOneTile(currentDirection);
        }
      }

      previousDirection = currentDirection;

      if (InputManager.KeyNewDown(Keys.C))
      {
        lockCamToPlayer = !lockCamToPlayer;
      }

      if (lockCamToPlayer)
      {
        Camera2D.SetPosition(
          EntityManager.GetInstance().GetFirstOccurencyOfType(
            EntityType.Player
          ).WorldPosition
        );
      }
      #endregion

      float deltaScroll = InputManager.MouseWheelChange();

      deltaScroll = Math.Sign(InputManager.MouseWheelChange()) * 2.5f;
      Camera2D.SetTargetScale(Camera2D.Scale + deltaScroll);

      if (InputManager.KeyDown(Keys.R))
      {
        Camera2D.SetTargetScale(1f);
      }

      EntityManager.GetInstance().Update(gameTime);
    }

    public void Draw()
    {
      Game.GraphicsDevice.Clear(Color.Black);

      testShader.Parameters["AmbientIntensity"].SetValue(inten);
      testShader.Parameters["World"].SetValue(Matrix.Identity);
      testShader.Parameters["View"].SetValue(Camera2D.View);
      testShader.Parameters["Projection"].SetValue(Camera2D.Projection);

      spriteBatch.Begin(SpriteSortMode.Deferred,
        null, null, null, null, testShader
      );
      worldManager.Draw(spriteBatch);

      // TODO(Martin): move player
      EntityManager.GetInstance().Draw(spriteBatch);
      spriteBatch.End();
    }
  }
}
