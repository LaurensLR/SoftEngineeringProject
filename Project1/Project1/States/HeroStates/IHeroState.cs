using CherryCollector.Entities.World;
using Microsoft.Xna.Framework;

namespace CherryCollector.States.HeroStates
{
    /// <summary>
    ///        IHeroState INTERFACE - HERO BEHAVIOR CONTRACT       
    ///   PURPOSE:  
    ///   Defines the contract for Hero state classes, enabling the State Pattern.   
    ///   Each state encapsulates a specific behavior mode for the Hero character.   
    ///   INTERFACE METHODS:   
    ///     • Enter(Hero): Called once when transitioning TO this state  
    ///       - Initialize state-specific settings (stop movement, play animation)   
    ///     • Update(Hero, GameTime): Called every frame while IN this state    
    ///    - Contains the main behavior logic for this state  
    ///   DESIGN PATTERNS APPLIED:  
    ///   [STATE PATTERN - State Interface]    
    ///   IHeroState is the abstract state interface in the State Pattern: 
    ///     • Defines common interface for all concrete states     
    ///     • Hero (Context) holds reference to current IHeroState     
    ///     • Hero delegates Update() to current state    
    ///     • Behavior changes by swapping state objects, not with if-else    
    ///   WHY STATE PATTERN?         
    ///   Without State Pattern (messy):        
    ///     if (isDead) { /* death logic */ }      
    ///   else if (isHurt) { /* hurt logic */ }    
    ///     else { /* normal logic */ }  
    ///   With State Pattern (clean):    
    ///     _currentState.Update(this, gameTime); // State handles its own logic  
    ///   SOLID PRINCIPLES APPLIED:      
    ///   [O] Open/Closed Principle (OCP): 
    ///       New states can be added (PowerUpState, SwimmingState) without  
    ///       modifying Hero or existing state classes.      
    ///   [D] Dependency Inversion Principle (DIP):   
    ///   Hero depends on IHeroState abstraction, not concrete states.  
    ///     This enables:   
    ///         • Easy state switching at runtime    
    ///    • Unit testing with mock states     
    ///     • Adding new states without changing Hero
    ///   [S] Single Responsibility Principle (SRP):  
    /// Each state class handles ONE behavior mode.    
    ///       NormalState = gameplay, DeadState = death animation. 
    ///   [L] Liskov Substitution Principle (LSP):    
    ///       Any IHeroState can substitute another - Hero calls Update()    
    ///     and Enter() the same way for all states.   
    ///   IMPLEMENTED BY: NormalState, DeadState   
    /// </summary>
    public interface IHeroState
    {
        /// <summary>
        /// Called every frame while the Hero is in this state.
        /// Contains the main behavior logic (physics, input, animation).
        /// </summary>
        void Update(Hero hero, GameTime gameTime);

        /// <summary>
        /// Called once when transitioning TO this state.
        /// Use to initialize state-specific settings.
        /// </summary>
        void Enter(Hero hero);
    }
}