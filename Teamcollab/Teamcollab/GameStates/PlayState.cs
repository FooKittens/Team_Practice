using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Engine;
using Microsoft.Xna.Framework.Input;

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
    #endregion

    public PlayState(Game game)
      :base(game, EApplicationState.Play)
    {
      // Initialize the gamestate.
      Initialize();
    }

    protected override void Initialize()
    {
      spriteBatch = new SpriteBatch(Game.GraphicsDevice);
    }

    public override void Update(GameTime gameTime)
    {
      if (InputManager.Key_Release(Keys.Escape))
      {
        if (StateChangeRequested != null)
        {
          StateChangeRequested(EApplicationState.Menu);
        }
      }
    }

    public override void Draw()
    {
      Game.GraphicsDevice.Clear(Color.DarkRed);

    }
  }
}
