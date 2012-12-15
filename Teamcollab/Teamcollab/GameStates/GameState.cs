using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Teamcollab.GameStates
{
  /// <summary>
  /// All variations of GameStates must be added.
  /// </summary>
  public enum EApplicationState
  {
    Undefined = 0,

    Menu,
    Play,
    Exit,
  }

  /// <summary>
  /// Base class for all gamestates. Keeps a reference to the game object which is
  /// accesible from derived classes.
  /// </summary>
  abstract class GameState
  {
    #region Properties
    /// <summary>
    /// An enum representing the type of state.
    /// </summary>
    public EApplicationState State { get; protected set; }

    /// <summary>
    /// A reference to the game object that uses the state.
    /// </summary>
    protected Game Game { get; set; }

    #endregion

    #region Delegates
    public delegate void StateChangeRequestHandler(EApplicationState nextState);
    #endregion

    #region Events
    public virtual event StateChangeRequestHandler StateChangeRequested;
    #endregion

    public GameState(Game game, EApplicationState thisState)
    {
      Game = game;
    }

    protected abstract void Initialize();

    public abstract void Update(GameTime gameTime);

    public abstract void Draw();

  }
}
