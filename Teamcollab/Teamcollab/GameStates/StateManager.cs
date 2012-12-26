using System;
using Microsoft.Xna.Framework;

namespace Teamcollab.GameStates
{
  sealed class StateManager
  {
    #region Properties

    #endregion

    #region Members
    private Game game;
    private GameState currentState;
    private MenuState menuState;
    private PlayState playState;
    #endregion

    public StateManager(Game game)
    {
      this.game = game;
      Initialize();
      StateChangeRequestHandler(ApplicationState.Menu);
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
    /// Initalizes the statemanager.
    /// </summary>
    private void Initialize()
    {
      menuState = new MenuState(game);
      menuState.StateChangeRequested += StateChangeRequestHandler;
    }

    /// <summary>
    /// Changes the active state of the statemanager.
    /// </summary>
    /// <param name="nextState">The next active state.</param>
    private void StateChangeRequestHandler(ApplicationState nextState)
    {
      switch (nextState)
      {
        case ApplicationState.Undefined:
          // Should only happen when an uninitialized enum has been passed.
          throw new ArgumentException("Undefined gamestate requested");
        case ApplicationState.Menu:
          currentState = menuState;
          break;
        case ApplicationState.Play:
          if (playState == null)
          {
            CreatePlayState();
          }
          currentState = playState;
          break;
        case ApplicationState.Exit:
          // Exit the game.
          game.Exit();
          break;
        default:
          throw new ArgumentException("Unhandled gamestate change request");
      }
    }

    private void CreatePlayState()
    {
      playState = new PlayState(game);
      playState.StateChangeRequested += StateChangeRequestHandler;
    }
  }
}
