using CherryCollector;
using CherryCollector.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.States.GameStates
{
    public class GameOverState : IGameState
    {
        private readonly Button _retryButton;
        private readonly Button _rqButton;
        private readonly SpriteFont _font;

        public GameOverState(SpriteFont font)
        {
            _font = font;
            // Smaller sized button
            _retryButton = new Button(new Rectangle(325, 220, 150, 40), "RETRY", font, Color.White);
            _rqButton = new Button(new Rectangle(325, 280, 150, 40), "RAGE QUIT", font, Color.White);
        }

        public void Update(Game1 game, GameTime gameTime)
        {
            // MOUSE ONLY: No more Keys.R
            if (_retryButton.IsClicked(game.ScaleMatrix))
            {
                // When dying, we only want to retry the current level.
                game.RestartLevel();
            }
            if (_rqButton.IsClicked(game.ScaleMatrix))
            {
                game.Exit();
            }
        }

        public void Draw(Game1 game, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "GAME OVER", new Vector2(345, 180), Color.Red);
            _retryButton.Draw(spriteBatch);
            _rqButton.Draw(spriteBatch);
        }
    }
}
