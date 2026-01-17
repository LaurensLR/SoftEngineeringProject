using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    /* 
     * SOLID - Single Responsibility Principle (SRP):
     * The UIManager is responsible ONLY for drawing the user interface (HUD).
     * 
     * DESIGN PATTERN - Observer Pattern:
     * This class acts as an Observer. It subscribes to the Hero's 'LivesChanged' 
     * event so it can update the visual display without the Hero needing 
     * to know that a UI even exists.
     */
    public class UIManager
    {
        private readonly SpriteFont _font;
        private int _heroLives;

        public UIManager(SpriteFont font, Hero hero)
        {
            _font = font;
            _heroLives = hero.Lives;

            // Subscribe to the Hero's event (Observer Pattern)
            hero.LivesChanged += OnLivesChanged;
        }

        // This method is called automatically whenever the Hero takes damage
        private void OnLivesChanged(object sender, int newLives)
        {
            _heroLives = newLives;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the lives at the top-left of the screen
            spriteBatch.DrawString(_font, $"LIVES: {_heroLives}", new Vector2(20, 20), Color.White);
            
            if (_heroLives <= 0)
            {
                spriteBatch.DrawString(_font, "GAME OVER", new Vector2(350, 220), Color.Red);
            }
        }
    }
}