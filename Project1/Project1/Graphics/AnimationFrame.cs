using Microsoft.Xna.Framework;

namespace CherryCollector.Graphics
{
    /// <summary>
    ///       AnimationFrame CLASS - SINGLE FRAME DATA
    ///   PURPOSE:   
    ///   Represents a single frame in an animation sequence by storing the       
    ///   source rectangle that defines where the frame is on the sprite sheet.    
    ///   DESIGN PATTERNS APPLIED:
    ///   [VALUE OBJECT PATTERN]    
    ///   AnimationFrame is essentially a value object - it holds data (the 
    ///   rectangle) with minimal behavior. It's immutable in practice since
    ///   frames are defined once during animation construction.   
    ///   [FLYWEIGHT PATTERN (partial)]  
    ///   Multiple Animation objects can share AnimationFrame instances if they      
    ///   use the same source rectangles. The actual pixel data lives in the
    ///   shared Texture2D, not in each frame.   
    ///   SOLID PRINCIPLES APPLIED:  
    ///   [S] Single Responsibility Principle (SRP): 
    /// AnimationFrame has ONE job: store the source rectangle for a frame.    
    ///       It doesn't manage timing, textures, or rendering.
    ///   [I] Interface Segregation Principle (ISP):    
    ///       AnimationFrame exposes only what's needed (SourceRectangle).    
    ///       No unnecessary methods or properties. 
    /// </summary>
    public class AnimationFrame
    {
        public Rectangle SourceRectangle { get; set; }
        public AnimationFrame(Rectangle sourceRectangleIn)
        {
            SourceRectangle = sourceRectangleIn;
        }
    }
}
