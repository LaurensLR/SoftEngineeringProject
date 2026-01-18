using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    /*
     * DESIGN PATTERN - State Pattern:
     * This defines the behavior for global game states (Menu, Playing, Game Over).
     * SOLID - Single Responsibility: Each concrete state class only handles one screen's logic.
     */
    public interface IGameState
    {
        void Update(Game1 game, GameTime gameTime);
        
        /* 
         * REFACTORING: Added Game1 parameter so states can access world objects 
         * during render without relying on global scope, fixing the 'top-level 
         * statement' variable confusion.
         */
        void Draw(Game1 game, SpriteBatch spriteBatch);
    }
}