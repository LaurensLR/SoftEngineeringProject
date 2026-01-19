using CherryCollector;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CherryCollector.States.GameStates;

namespace CherryCollector.States.GameStates
{
    public class PlayingState : IGameState
    {
        public PlayingState() { }

        public void Update(Game1 game, GameTime gameTime)
        {
            game.LevelManager.Update(gameTime);
            game.Hero.Update(gameTime);
            game.UiManager.Update(gameTime);

            if (game.Hero.Lives <= 0)
            {
                if (game.Hero.AnimationManager.CurrentAnimation.IsFinished)
                {
                    game.SetGameState(new GameOverState(game.Font));
                }
                return;
            }

            /* 
             * SOLID - SRP/Mediator Fix:
             * We ask the LevelManager if we are done, instead of inspecting the door object manually.
             */
            if (game.LevelManager.CheckLevelCompletion())
            {
                if (game.LevelManager.HasMoreLevels())
                {
                    game.LevelManager.NextLevel();
                    game.Hero.ResetPosition(new Vector2(100, 100));
                }
                else
                {
                    game.SetGameState(new WinState(game.Font));
                }
            }
            else if (game.LevelManager.IsPlayerAtLockedDoor())
            {
                game.UiManager.DisplayMessage("COLLECT ALL CHERRIES TO EXIT!", 1.5f);
            }
        }

        public void Draw(Game1 game, SpriteBatch spriteBatch)
        {
            game.LevelManager.Draw(spriteBatch);
            game.Hero.Draw(spriteBatch);
            game.UiManager.Draw(spriteBatch);
        }
    }
}
