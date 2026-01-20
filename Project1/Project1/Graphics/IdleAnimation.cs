using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.Graphics
{
    /// <summary>
    /// IdleAnimation CLASS - HERO IDLE ANIMATION  
    ///   PURPOSE:  
    ///   Defines the idle animation that plays when the Hero is standing still.    
    ///   4 frames at 6 FPS creates a subtle breathing/bobbing effect.    
    ///   DESIGN PATTERNS APPLIED:    
    ///   [TEMPLATE METHOD PATTERN - Concrete Implementation]      
    ///   IdleAnimation extends Animation base class and:  
    ///     • Calls base constructor with fps=6 and loop=true   
    ///     • Sets the Texture property   
    ///     • Calls AddFrame() for each frame rectangle   
    ///   Base class handles all update/timing logic.    
    ///   SOLID PRINCIPLES APPLIED:     
    ///   [S] Single Responsibility Principle (SRP):     
    /// IdleAnimation ONLY defines idle-specific frame data.  
    ///       Animation base handles timing; AnimationManager handles state.  
    ///   [O] Open/Closed Principle (OCP):  
    ///   Adding new animations (jump, attack) follows same pattern:     
    ///         1. Create new class extending Animation
    /// 2. Define frames in constructor 
    ///       No modification to existing animation classes needed.   
    /// </summary>
    internal class IdleAnimation : Animation
    {
        public IdleAnimation(Texture2D texture) : base(fps: 6, loop: true)
        {
            Texture = texture;

            AddFrame(new AnimationFrame(new Rectangle(2, 2, 26, 28)));
            AddFrame(new AnimationFrame(new Rectangle(32, 2, 26, 28)));
            AddFrame(new AnimationFrame(new Rectangle(62, 2, 26, 28)));
            AddFrame(new AnimationFrame(new Rectangle(92, 2, 26, 28)));
        }
    }
}