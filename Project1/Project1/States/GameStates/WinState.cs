using CherryCollector;
using CherryCollector.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.States.GameStates
{
    public class WinState : IGameState
    {
        private readonly Button _menuButton;
        private readonly Button _quitButton;
        private readonly SpriteFont _font;

        public WinState(SpriteFont font)
        {
            _font = font;
            _menuButton = new Button(new Rectangle(325, 230, 150, 40), "MENU", font, Color.Green);
            _quitButton = new Button(new Rectangle(325, 280, 150, 40), "QUIT", font, Color.White);
        }

        public void Update(Game1 game, GameTime gameTime)
        {
            if (_menuButton.IsClicked(game.ScaleMatrix))
            {
                // Going back to the menu implies a full reset next time we play.
                game.SetGameState(new StartState(game.Font));
            }

            if (_quitButton.IsClicked(game.ScaleMatrix))
            {
                game.Exit();
            }
        }

        public void Draw(Game1 game, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "YOU WIN!", new Vector2(355, 150), Color.Gold);
            _menuButton.Draw(spriteBatch);
            _quitButton.Draw(spriteBatch);
        }
    }
}
