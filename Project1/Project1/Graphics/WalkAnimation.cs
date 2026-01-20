using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CherryCollector.Graphics;

namespace CherryCollector.Graphics
{
    /// <summary>
    ///  WalkAnimation CLASS - HERO WALKING ANIMATION 
    ///   PURPOSE:   
    ///   Defines the walk animation that plays when the Hero is moving.  
    ///   8 frames at 12 FPS creates smooth, responsive movement feel.  
    ///   DESIGN PATTERNS APPLIED:    
    ///   [TEMPLATE METHOD PATTERN - Concrete Implementation]  
    ///   Same pattern as IdleAnimation:     
    ///     • Extends Animation base class     
    ///     • Provides walk-specific fps (12) and frame rectangles 
    ///     • Inherits update/timing behavior from base    
    ///   SOLID PRINCIPLES APPLIED: 
    ///   [S] Single Responsibility Principle (SRP):    
    ///  WalkAnimation ONLY defines walk-specific frame data.       
    ///       Each animation class is focused and minimal.    
    ///   [L] Liskov Substitution Principle (LSP):     
    ///    WalkAnimation can substitute any Animation reference.     
    ///   AnimationManager treats all animations polymorphically.     
    /// </summary>
    internal class WalkAnimation : Animation
    {
        public WalkAnimation(Texture2D texture) : base(fps: 12, loop: true)
        {
            Texture = texture;

            AddFrame(new AnimationFrame(new Rectangle(2, 2, 26, 28)));
            AddFrame(new AnimationFrame(new Rectangle(30, 2, 26, 28)));
            AddFrame(new AnimationFrame(new Rectangle(58, 2, 26, 28)));
            AddFrame(new AnimationFrame(new Rectangle(86, 2, 26, 28)));
            AddFrame(new AnimationFrame(new Rectangle(112, 2, 26, 28)));
            AddFrame(new AnimationFrame(new Rectangle(138, 2, 26, 28)));
            AddFrame(new AnimationFrame(new Rectangle(166, 2, 26, 28)));
            AddFrame(new AnimationFrame(new Rectangle(192, 2, 26, 28)));
        }
    }
}