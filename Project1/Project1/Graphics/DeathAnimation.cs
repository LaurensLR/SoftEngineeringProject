using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CherryCollector.Graphics;

namespace CherryCollector.Graphics
{
    /// <summary>
    ///       DeathAnimation CLASS - HERO DEATH ANIMATION
    ///   PURPOSE:     
    ///   Defines the death animation that plays when the Hero loses all lives. 
    ///   6 frames at 4 FPS, non-looping - creates a dramatic falling/collapsing.    
    ///   DEATH SEQUENCE FLOW:
    ///     1. Hero.Lives reaches 0  
    ///     2. Hero.SetState(new DeadState())    
    ///     3. DeadState.Enter() calls AnimationManager.PlayDeath()     
    ///   4. DeathAnimation plays for 1.5 seconds (6 frames at 4 FPS)     
    ///     5. Animation.IsFinished becomes true   
    ///     6. Game1 detects death and transitions to GameOverState     
    ///   DESIGN PATTERNS APPLIED:    
    ///   [TEMPLATE METHOD PATTERN - Concrete Implementation]     
    ///   DeathAnimation extends Animation base class:  
    ///  • Calls base(fps: 4, loop: false) for slow, non-looping behavior  
    ///     • Sets Texture for this specific animation
    ///   • Defines 6 frames with precise source rectangles     
    ///   Base class handles all timing and state (IsFinished) logic.  
    ///   SOLID PRINCIPLES APPLIED: 
    ///   [S] Single Responsibility Principle (SRP):     
    ///     DeathAnimation ONLY defines death-specific frame data and timing.      
    ///       It doesn't:
    ///         • Decide WHEN to play (DeadState decides)   
    ///  • Handle game over logic (Game1 handles)    
    ///         • Calculate frame timing (Animation base handles)  
    ///   [O] Open/Closed Principle (OCP):  
    ///    Alternative death animations could be created without modifying this.  
    ///    Example: ExplosionDeathAnimation, FadeDeathAnimation, etc.   
    ///   [L] Liskov Substitution Principle (LSP):    
    ///       DeathAnimation can substitute any Animation reference.
    ///       AnimationManager treats all animations polymorphically. 
    /// </summary>
    internal class DeathAnimation : Animation
    {
        public DeathAnimation(Texture2D texture) : base(fps: 4, loop: false)
        {
            Texture = texture;

            AddFrame(new AnimationFrame(new Rectangle(2, 3, 26, 30)));
            AddFrame(new AnimationFrame(new Rectangle(35, 7, 30, 26)));
            AddFrame(new AnimationFrame(new Rectangle(73, 9, 30, 24)));
            AddFrame(new AnimationFrame(new Rectangle(115, 15, 34, 18)));
            AddFrame(new AnimationFrame(new Rectangle(153, 13, 38, 20)));
            AddFrame(new AnimationFrame(new Rectangle(199, 14, 38, 18)));
        }
    }
}
