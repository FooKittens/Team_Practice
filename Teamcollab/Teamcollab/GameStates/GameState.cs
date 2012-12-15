using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Teamcollab.GameStates
{
  public enum EApplicationState
  {
    Undefined = 0,

    Menu,
    Play,
    Exit,

  }


  /// <summary>
  /// 
  /// </summary>
  abstract class GameState
  {
    #region Properties
    public Game Game { get; protected set; }

    #endregion

    public GameState(Game game)
    {
      Game = game;

    }

    public abstract void Update(GameTime gameTime);

    public abstract void Draw();

  }
}
