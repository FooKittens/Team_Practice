using Microsoft.Xna.Framework;

namespace Midgard.GameStates
{
  /// <summary>
  /// All variations of GameStates must be added.
  /// </summary>
  public enum ApplicationState
  {
    Undefined = 0,

    Menu,
    Play,
    Exit,
  }

  /// <summary>
  /// Base class for all gamestates. Keeps a reference to the game object which is
  /// accessible from derived classes.
  /// </summary>
  abstract class GameState
  {
    #region Properties
    /// <summary>
    /// An enum representing the type of state.
    /// </summary>
    public ApplicationState State { get; protected set; }

    /// <summary>
    /// A reference to the game object that uses the state.
    /// </summary>
    protected Game Game { get; set; }
    #endregion

    #region Delegates
    public delegate void StateChangeRequestHandler(ApplicationState nextState);
    #endregion

    #region Events
    public abstract event StateChangeRequestHandler StateChangeRequested;
    #endregion

    public GameState(Game game, ApplicationState thisState)
    {
      Game = game;
    }

    protected abstract void Initialize();

    public abstract void Update(GameTime gameTime);

    public abstract void Draw();

  }
}
