using CherryCollector.Entities.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.Systems
{
    /// <summary>
    ///        UIManager CLASS - HEADS-UP DISPLAY SYSTEM   
    ///   PURPOSE:     
    ///   Manages the in-game user interface (HUD) that displays during gameplay.   
    ///   Shows player lives and temporary notification messages.     
    ///   DESIGN PATTERNS APPLIED: 
    ///   [OBSERVER PATTERN]  
    ///   UIManager subscribes to Hero's LivesChanged event
    ///   Benefits:      
    ///     • Hero doesn't know UIManager exists (loose coupling)     
    ///     • UI updates automatically when lives change     
    ///     • Multiple observers could subscribe (sound manager, achievements)       
    ///   [MEDIATOR PATTERN (partial)] 
    ///   DisplayMessage() allows game logic to show messages without knowing
    ///   HOW messages are displayed. The UI handles presentation details.    
    ///   SOLID PRINCIPLES APPLIED:       
    ///   [S] Single Responsibility Principle (SRP):    
    ///       UIManager ONLY handles HUD rendering and message display.    
    ///       It doesn't:     
    ///         • Calculate lives (Hero does that)  
    ///  • Decide when to show messages (PlayingState decides)    
    ///         • Handle menu buttons (Button class does that)     
    ///   [O] Open/Closed Principle (OCP):     
    ///       New HUD elements (score, timer, minimap) can be added without 
    ///       modifying existing lives/message code.    
    ///   [D] Dependency Inversion Principle (DIP):     
    ///       UIManager receives Hero via constructor injection.   
    ///       It depends on Hero's event, not Hero's internal implementation.  
    /// </summary>
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
                Vector2 position = new Vector2(400 - textSize.X / 2, 400);
                spriteBatch.DrawString(_font, _activeMessage, position, Color.Yellow);
            }
        }
    }
}