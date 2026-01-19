using CherryCollector.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.States.GameStates
{
    public class StartState : IGameState
    {
        private readonly Button _startButton;
        private readonly Button _quitButton;

        // INPUT DELAY: Prevents accidental double-clicks when transitioning from another state
        private float _inputDelay = 0.5f;

        public StartState(SpriteFont font)
        {
            // Smaller button with design padding
            _startButton = new Button(new Rectangle(325, 220, 150, 40), "START", font, Color.White);
            _quitButton = new Button(new Rectangle(325, 280, 150, 40), "QUIT", font, Color.White);
        }

        public void Update(Game1 game, GameTime gameTime)
        {
            // DEBOUNCE LOGIC:
            if (_inputDelay > 0)
            {
                _inputDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return; // Ignore input while delay is active
            }

            if (_startButton.IsClicked(game.ScaleMatrix))
            {
                // ResetGame() ensures we start at Level 0
                game.ResetGame();
            }
            if (_quitButton.IsClicked(game.ScaleMatrix))
            {
                game.Exit();
            }
        }

        public void Draw(Game1 game, SpriteBatch spriteBatch)
        {
            _startButton.Draw(spriteBatch);
            _quitButton.Draw(spriteBatch);
        }
    }
}
