using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Teamcollab.Engine.Helpers;
using Teamcollab.Resources;

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

    ResourceCollection<Texture2D> textures;

    public MenuState(Game game)
      :base(game, ApplicationState.Menu)
    {
      Initialize();
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
      UglyHackTestOfDoomPleaseKillMe();
    }

    protected override void Initialize()
    {
      textures = new ResourceCollection<Texture2D>();
      spriteBatch = new SpriteBatch(Game.GraphicsDevice);
      textures.Add("Viking", Game.Content.Load<Texture2D>("Art\\Vikingbg"));
    }

    // TODO(Zerkish): Remove 
    private void UglyHackTestOfDoomPleaseKillMe()
    {
      spriteBatch.Begin();

      spriteBatch.Draw(textures.Query("Viking"), Vector2.Zero, Color.White);

      spriteBatch.End();
    }
  }
}
