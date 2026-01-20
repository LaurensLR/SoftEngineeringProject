using CherryCollector.Entities.World;
using Microsoft.Xna.Framework;

namespace CherryCollector.States.HeroStates
{
    /// <summary>
    ///  DeadState CLASS - HERO DEATH BEHAVIOR           
    ///   PURPOSE:     
    ///   Represents the Hero's death state where all player input is disabled
    ///   and the death animation plays.            
    ///   DESIGN PATTERNS APPLIED:          
    ///   [STATE PATTERN - Concrete State]         
    ///   DeadState implements IHeroState to provide death-specific behavior.   
    ///   The Hero object doesn't need if-statements checking "am I dead?" -          
    ///   it just delegates to its current state. 
    ///   [NULL OBJECT PATTERN (partial)]   
    ///   The Update() method is intentionally minimal - it acts almost like  
    ///   a "null" update that ignores physics and input, only updating visuals.      
    ///   SOLID PRINCIPLES APPLIED:       
    ///   [S] Single Responsibility Principle (SRP):    
    ///       DeadState ONLY manages the death behavior (freeze + animation).         
    ///       It doesn't handle respawning, lives counting, or game over logic.       
    ///   [L] Liskov Substitution Principle (LSP):      
    ///       DeadState can replace any IHeroState - the Hero calls Update()
    ///       and Enter() the same way regardless of which state it's in. 
    /// </summary>
    public class DeadState : IHeroState
    {
        public void Enter(Hero hero)
        {
            hero.Speed = Vector2.Zero;
            hero.PhysicsManager.CancelJump();
            hero.AnimationManager.PlayDeath();
        }

        public void Update(Hero hero, GameTime gameTime)
        {
            hero.AnimationManager.Update(Vector2.Zero, gameTime);
        }
    }
}
