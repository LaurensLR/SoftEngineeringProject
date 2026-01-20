using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.Graphics
{
    /// <summary>
    ///       HurtAnimation CLASS - HERO DAMAGE ANIMATION   
    ///   PURPOSE:  
    ///   Defines the hurt animation that plays when the Hero takes damage. 
    ///   4 frames at 10 FPS, non-looping - plays once then returns to idle.  
    ///   VARIABLE FRAME SIZES: 
    ///   Hurt frames have slightly different dimensions (26x30, 22x28, etc.)      
    ///   to show the character recoiling. The SourceRectangles are precise.
    ///   DESIGN PATTERNS APPLIED:  
    ///   [TEMPLATE METHOD PATTERN - Concrete Implementation]   
    ///   Extends Animation with loop=false for one-shot behavior.   
    ///   Base class Update() automatically stops at last frame when not looping.    
    ///   SOLID PRINCIPLES APPLIED: 
    ///   [S] Single Responsibility Principle (SRP): 
    ///       HurtAnimation ONLY defines hurt-specific frames.
    ///       Damage logic is in Hero; state transition in AnimationManager.  
    /// </summary>
    internal class HurtAnimation : Animation
    {
        public HurtAnimation(Texture2D texture) : base(fps: 10, loop: false)
        {
            Texture = texture;

            AddFrame(new AnimationFrame(new Rectangle(4, 4, 26, 30)));
            AddFrame(new AnimationFrame(new Rectangle(34, 6, 22, 28)));
            AddFrame(new AnimationFrame(new Rectangle(62, 8, 22, 26)));
            AddFrame(new AnimationFrame(new Rectangle(87, 8, 22, 26)));
        }
    }
}
