using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Teamcollab.Engine.Helpers;

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
    GameManager gManager;
    bool isPaused;
    #endregion

    public PlayState(Game game)
      :base(game, ApplicationState.Play)
    {
      // Initialize the gamestate.
      Initialize();
      isPaused = false;
    }

    protected override void Initialize()
    {
      gManager = new GameManager(Game);
    }

    
    public override void Update(GameTime gameTime)
    {
      if (!isPaused)
      {
        gManager.Update(gameTime);
      }

      if (InputManager.KeyRelease(Keys.Escape))
      {
        if (StateChangeRequested != null)
        {
          StateChangeRequested(ApplicationState.Menu);
        }
      }

      if (InputManager.KeyNewDown(Keys.P))
      {
        isPaused = !isPaused;
      }
    }

    public override void Draw()
    {
      gManager.Draw();
    }
  }
}
