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

        // Message system fields
        private string _activeMessage = "";
        private float _messageTimer = 0f;

        public UIManager(SpriteFont font, Hero hero)
        {
            _font = font;
            _heroLives = hero.Lives;

            // Subscribe to the Hero's event (Observer Pattern)
            hero.LivesChanged += OnLivesChanged;
        }

        /* 
         * DESIGN PATTERN - Command/Notification:
         * This allows the game logic to "request" a message be shown 
         * without the UI knowing WHY it is being shown.
         */
        public void DisplayMessage(string message, float duration)
        {
            _activeMessage = message;
            _messageTimer = duration;
        }

        public void Update(GameTime gameTime)
        {
            if (_messageTimer > 0)
            {
                _messageTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void OnLivesChanged(object sender, int newLives)
        {
            _heroLives = newLives;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // HUD Responsibility: Only show the lives
            spriteBatch.DrawString(_font, $"LIVES: {_heroLives}", new Vector2(20, 20), Color.White);

            // Draw active notification message if timer is running
            if (_messageTimer > 0)
            {
                // Positioned in the middle-bottom area of your 800x480 screen
                Vector2 textSize = _font.MeasureString(_activeMessage);
                Vector2 position = new Vector2(400 - (textSize.X / 2), 400);
                spriteBatch.DrawString(_font, _activeMessage, position, Color.Yellow);
            }
        }
    }
}