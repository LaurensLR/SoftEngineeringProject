using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.Core
{
    /// <summary>
    /// IDrawable INTERFACE       
    ///   PURPOSE:   
    ///   Defines the contract for any object that can be rendered to the screen.    
    ///   This is part of the segregated interface design for game objects.          
    ///      
    ///   HOW IT WORKS:   
    ///   - Any class implementing this interface must provide a Draw() method       
    ///   - The SpriteBatch parameter allows rendering sprites, text, and shapes
    ///   - Called every frame during the game's render loop     
    ///   SOLID PRINCIPLES APPLIED:
    ///              
    ///   [I] Interface Segregation Principle (ISP):
    ///       - This interface is small and focused on ONE responsibility: drawing   
    ///  - Classes that don't need drawing capabilities aren't forced to        
    ///         implement unnecessary methods          
    ///       - Works alongside IUpdatable and ICollidable to form composable        
    ///         behavior contracts
    /// IMPLEMENTED BY: Hero, Tile, Spike, Cherry, Door, Snail, Bat, Button
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Renders the object to the screen using the provided SpriteBatch.
        /// </summary>
        /// <param name="spriteBatch">The MonoGame SpriteBatch used for 2D rendering</param>
        void Draw(SpriteBatch spriteBatch);
    }
}
