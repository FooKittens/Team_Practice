using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Engine;
using Microsoft.Xna.Framework.Input;

namespace Teamcollab.GameStates
{
  sealed class MenuState : GameState
  {
    #region Properties

    #endregion

    #region Events
    public override event StateChangeRequestHandler StateChangeRequested;
    #endregion

    #region Members
    SpriteBatch spriteBatch;
    #endregion

    public MenuState(Game game)
      :base(game, EApplicationState.Menu)
    {

    }

    public override void Update(GameTime gameTime)
    {

      if (InputManager.Key_Release(Keys.Escape))
      {
        if (StateChangeRequested != null)
        {
          StateChangeRequested(EApplicationState.Exit);
        }
      }
      else if (InputManager.Key_Release(Keys.P))
      {
        if (StateChangeRequested != null)
        {
          StateChangeRequested(EApplicationState.Play);
        }
      }
    }

    public override void Draw()
    {
      Game.GraphicsDevice.Clear(Color.DarkGreen);
    }

    protected override void Initialize()
    {
      spriteBatch = new SpriteBatch(Game.GraphicsDevice);
    }
  }
}
