using CherryCollector.Entities.World;
using Microsoft.Xna.Framework;

namespace CherryCollector.States.HeroStates
{
    /// <summary>
    ///          NormalState CLASS - DEFAULT HERO BEHAVIOR       
    ///   PURPOSE:      
    ///   Represents the Hero's normal gameplay state where the player has full       
    ///   control over movement and jumping.       
    ///   DESIGN PATTERNS APPLIED:      
    ///   [STATE PATTERN - Concrete State]  
    ///   NormalState implements IHeroState, allowing the Hero to switch behaviors    
    ///   at runtime. When damaged, the Hero transitions to DeadState. When alive,   
    ///   it stays in NormalState with full control.     
    ///   HERO STATE TRANSITIONS:  
    ///     NormalState ──► DeadState (when Lives <= 0 or falls off map)              
    ///     DeadState ──► NormalState (on level reset)
    ///   [DELEGATION PATTERN] 
    ///   NormalState doesn't calculate physics itself - it delegates to:             
    ///     • PhysicsManager: All movement, gravity, collision  
    ///     • AnimationManager: Visual sprite updates 
    ///     • InputReader: Reading player input 
    ///   SOLID PRINCIPLES APPLIED:     
    ///   [S] Single Responsibility Principle (SRP):  
    ///       NormalState ONLY orchestrates the Hero's normal behavior.
    ///      Physics logic is in PhysicsManager, animation in AnimationManager.      
    ///   [O] Open/Closed Principle (OCP):  
    ///    New hero states (PowerUpState, InvincibleState) can be added without    
    ///  modifying NormalState or the Hero class.   
    ///   [D] Dependency Inversion Principle (DIP):   
    ///       Hero depends on IHeroState interface, not concrete NormalState.         
    ///       This enables polymorphic behavior switching.       
    /// </summary>
    public class NormalState : IHeroState
    {
        public void Enter(Hero hero) { }

        public void Update(Hero hero, GameTime gameTime)
        {
            var objects = hero.LevelManager.CurrentLevelObjects;

            hero.PhysicsManager.Update(hero, objects, gameTime);

            if (hero.InputReader.ReadInput().Y > 0)
                hero.PhysicsManager.Jump();

            hero.AnimationManager.Update(new Vector2(hero.InputReader.ReadInput().X, 0), gameTime);
        }
    }
}
