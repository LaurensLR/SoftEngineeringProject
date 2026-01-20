using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CherryCollector.Systems.Input
{
    /// <summary>
    ///       KeyBoardReader CLASS - KEYBOARD INPUT HANDLER    
    ///   PURPOSE:   
    ///   Concrete implementation of IInputReader that reads from the keyboard.   
    ///   Translates keyboard key states into a direction vector for game use.
    ///   DESIGN PATTERNS APPLIED:
    ///   [STRATEGY PATTERN - Concrete Strategy]       
    ///   KeyBoardReader is a concrete implementation of the IInputReader strategy.  
    ///   The game can use this or swap in a different implementation.    
    ///   [ADAPTER PATTERN (partial)]       
    ///   Adapts MonoGame's keyboard API (KeyboardState, Keys enum) into the       
    ///   simple Vector2 format the game expects.    
    ///   SOLID PRINCIPLES APPLIED:    
    ///   [S] Single Responsibility Principle (SRP):     
    ///       KeyBoardReader ONLY reads keyboard input.   
    ///       Doesn't process input, move characters, or apply physics. 
    ///   [L] Liskov Substitution Principle (LSP):    
    ///       Can replace any IInputReader without changing calling code.    
    ///    PhysicsManager doesn't know or care that this is keyboard-specific.    
    ///   ALTERNATIVE INPUT KEY MAPPINGS:
    ///   Current: Arrow keys (Left, Right, Up) 
    ///   Could be extended to support:         
    ///     - WASD keys  
    ///     - Customizable key bindings       
    ///   - Multiple simultaneous inputs   
    /// </summary>
    public class KeyBoardReader : IInputReader
    {
        /// <summary>
        /// Reads current keyboard state and returns direction vector.
        /// ADAPTER: Converts MonoGame KeyboardState to Vector2.
        /// </summary>
        /// <returns>Direction vector based on arrow key state</returns>
        public Vector2 ReadInput()
        {
            // Get current keyboard state from MonoGame
            var state = Keyboard.GetState();

            // HORIZONTAL INPUT
            float x = 0;
            if (state.IsKeyDown(Keys.Left)) x -= 1;  // Left arrow = negative X
            if (state.IsKeyDown(Keys.Right)) x += 1; // Right arrow = positive X
            // If both pressed, x = 0 (they cancel out)

            // VERTICAL INPUT (Jump)
            float y = 0;
            if (state.IsKeyDown(Keys.Up)) y = 1; // Up arrow = jump request

            return new Vector2(x, y);
        }
    }
}