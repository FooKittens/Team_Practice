using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Teamcollab.GameStates;
using Teamcollab.Engine;
using Microsoft.Xna.Framework.Input;

namespace Teamcollab.Managers
{
  sealed class StateManager
  {
    #region Properties

    #endregion

    #region Members
    private Game game;
    private GameState currentState;
    #endregion

    public StateManager(Game game)
    {
      this.game = game;
    }

    public void Update(GameTime gameTime)
    {
      currentState.Update(gameTime);


    }

    public void Draw()
    {
      currentState.Draw();
    }

    /// <summary>
    /// Changes the active state of the statemanager.
    /// </summary>
    /// <param name="nextState">The next active state.</param>
    private void StateChangeRequestHandler(EApplicationState nextState)
    {
      switch (nextState)
      {
        case EApplicationState.Undefined:
          // Should only happen when an uninitialized enum has been passed.
          throw new ArgumentException("Undefined gamestate requested");
        case EApplicationState.Menu:
          break;
        case EApplicationState.Play:
          break;
        case EApplicationState.Exit:
          // Exit the game.
          game.Exit();
          break;
        default:
          throw new ArgumentException("Unhandled gamestate change request");
      }
    }

  }
}
