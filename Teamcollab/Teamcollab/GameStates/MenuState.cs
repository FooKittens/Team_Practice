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
      :base(game, ApplicationState.Menu)
    {

    }

    public override void Update(GameTime gameTime)
    {

      if (InputManager.KeyRelease(Keys.Escape))
      {
        if (StateChangeRequested != null)
        {
          StateChangeRequested(ApplicationState.Exit);
        }
      }
      else if (InputManager.KeyRelease(Keys.P))
      {
        if (StateChangeRequested != null)
        {
          StateChangeRequested(ApplicationState.Play);
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
