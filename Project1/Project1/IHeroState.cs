using Microsoft.Xna.Framework;

namespace Project1
{
    /*
     * DESIGN PATTERN - State Pattern:
     * This interface defines the behavior for different Hero states. 
     * It allows us to swap the Hero's behavior at runtime without large 'if-else' blocks.
     */
    public interface IHeroState
    {
        void Update(Hero hero, GameTime gameTime);
        void Enter(Hero hero);
    }
}