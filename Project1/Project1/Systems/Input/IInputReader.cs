using Microsoft.Xna.Framework;

namespace CherryCollector.Systems.Input
{
    /// <summary>
    ///     IInputReader INTERFACE - INPUT ABSTRACTION      
    ///   PURPOSE:     
    ///   Defines a contract for reading player input, returning a Vector2 that   
    ///   represents the direction the player wants to move.   
    ///   DESIGN PATTERNS APPLIED:  
    ///   [STRATEGY PATTERN - Strategy Interface]   
    ///   IInputReader is the Strategy interface that defines HOW input is read.    
    ///   Different concrete strategies can implement this:    
    ///     • KeyBoardReader: Reads from keyboard    
    ///     • GamePadReader: Could read from controller (future)         
    ///     • AIInputReader: Could simulate input for AI (testing/NPCs)
    ///   SOLID PRINCIPLES APPLIED:       
    ///   [D] Dependency Inversion Principle (DIP):        
    ///       Hero depends on the IInputReader abstraction, NOT on KeyBoardReader.    
    ///       This means: 
    ///     • Hero doesn't know or care if input comes from keyboard, gamepad,
    ///      or an AI simulation     
    ///     • New input methods can be added without modifying Hero 
    ///    • Input can be mocked for unit testing  
    ///   [O] Open/Closed Principle (OCP):  
    ///     The input system is OPEN for extension (new input readers)     
    ///    but CLOSED for modification (Hero code doesn't change).    
    ///   [I] Interface Segregation Principle (ISP):       
    ///       IInputReader has only ONE method - exactly what clients need.     
    ///       No unnecessary methods that some implementations wouldn't use.    
    /// </summary>
    public interface IInputReader
    {
        /// <summary>
        /// Reads the current input state and returns a direction vector.
        /// </summary>
        /// <returns>
        /// Vector2 where:
        /// - X: -1 (left), 0 (none), +1 (right)
        /// - Y: 0 (none), +1 (jump request)
        /// </returns>
        Vector2 ReadInput();
    }
}
