using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Engine.WorldManagement;
using Microsoft.Xna.Framework;
using Teamcollab.Engine.Helpers;
using Teamcollab.GUI;
using Microsoft.Xna.Framework.Input;

namespace Teamcollab.GameStates
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
    IsoBatch spriteBatch;
    WorldManager worldManager;

    Direction previousDirection;
    #endregion

    Effect testShader;

    public GameManager(Game game)
    {
      // Initialize the gamestate.
      Game = game; 
      Initialize();

      testShader = game.Content.Load<Effect>("BasicShader");
    }

    protected void Initialize()
    {
      spriteBatch = new IsoBatch(Game.GraphicsDevice);
      worldManager = WorldManager.GetInstance(Game);
    }

    float inten = 0f;
    float t;


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
      Vector2 change = Coordinates.Zero;
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
      Camera2D.SetPosition(WorldManager.GetClusterScreenCenter(change));
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

      Vector2 camTile = WorldManager.TransformScreenToTile(
        Camera2D.TargetPosition
      );
      change.X += Convert.ToInt32(camTile.X);
      change.Y += Convert.ToInt32(camTile.Y);
      Camera2D.SetTargetPosition(WorldManager.GetTileScreenPosition(change));
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

      previousDirection = currentDirection;
      #endregion

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
    }

    public void Draw()
    {
      Game.GraphicsDevice.Clear(Color.Black);

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
