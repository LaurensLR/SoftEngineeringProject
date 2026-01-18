using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Project1
{
    /* 
     * SOLID - Single Responsibility: StartState handles only the Menu logic.
     */
    public class StartState : IGameState
    {
        private readonly Button _startButton;

        public StartState(SpriteFont font)
        {
            // Positioned in the middle of the 800x480 screen
            _startButton = new Button(new Rectangle(300, 200, 200, 50), "PRESS ENTER TO START", font, Color.White);
        }

        public void Update(Game1 game, GameTime gameTime)
        {
            // DESIGN PATTERN - State Transition: Moves to PlayingState
            if (_startButton.IsClicked() || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                game.SetGameState(new PlayingState(game));
            }
        }

        // Updated signature: 'game' is provided as a parameter to satisfy IGameState.
        public void Draw(Game1 game, SpriteBatch spriteBatch)
        {
            _startButton.Draw(spriteBatch);
        }
    }

    /* 
     * SOLID - Single Responsibility: PlayingState delegates to the game world.
     */
    public class PlayingState : IGameState
    {
        public PlayingState(Game1 game) { }

        public void Update(Game1 game, GameTime gameTime)
        {
            // Delegate logic to Managers
            game.LevelManager.Update(gameTime);
            game.Hero.Update(gameTime);

            // DESIGN PATTERN - State Transition with Delay:
            // When lives hit 0, the Hero enters 'DeadState', which starts the death animation.
            // We wait until that specific animation is finished before switching screens.
            if (game.Hero.Lives <= 0)
            {
                /* 
                 * SOLID - Dependency Inversion:
                 * We query the Hero's component (AnimationManager) to check progress.
                 * This ensures the player actually sees the character die before 
                 * the Game Over menu appears.
                 */
                if (game.Hero.AnimationManager.CurrentAnimation.IsFinished)
                {
                    game.SetGameState(new GameOverState(game.Font, game));
                }
                return;
            }
            // LEVEL PROGRESSION LOGIC (Only if Alive)
            /* 
             * REFACTORING - State Transition Guard:
             * We only progress if all cherries are collected AND 
             * the Hero has reached the Door object.
             */
            if (game.LevelManager.AllCherriesCollected())
            {
                // Find the door in the current level
                var door = game.LevelManager.CurrentLevelObjects.OfType<Door>().FirstOrDefault();

                if (door != null && door.IsPlayerInside)
                {
                    game.LevelManager.NextLevel();
                    game.Hero.ResetPosition(new Vector2(100, 100)); // Smooth teleport to start of NEXT level
                }
            }
        }

        // Updated signature: Signature fixed. 'game' is now provided as a parameter to avoid 
        // scope confusion with top-level variables in Program.cs.
        public void Draw(Game1 game, SpriteBatch spriteBatch)
        {
            game.LevelManager.Draw(spriteBatch);
            game.Hero.Draw(spriteBatch);
            game.UiManager.Draw(spriteBatch);
        }
    }

    /* 
     * SOLID - Single Responsibility: GameOverState handles the reset loop.
     */
    public class GameOverState : IGameState
    {
        private readonly Button _restartButton;
        private readonly SpriteFont _font;

        public GameOverState(SpriteFont font, Game1 game)
        {
            _font = font;
            _restartButton = new Button(new Rectangle(300, 250, 200, 50), "PRESS R TO RESTART", font, Color.White);
        }

        public void Update(Game1 game, GameTime gameTime)
        {
            // DESIGN PATTERN - Reset: Restores game variables
            if (_restartButton.IsClicked() || Keyboard.GetState().IsKeyDown(Keys.R))
            {
                game.RestartGame();
            }
        }
        public void Draw(Game1 game, SpriteBatch spriteBatch)
        {
            // DESIGN PATTERN - Responsibility: GameOver screen handles its own text
            spriteBatch.DrawString(_font, "GAME OVER", new Vector2(320, 180), Color.Red);
            _restartButton.Draw(spriteBatch);
        }
    }
}