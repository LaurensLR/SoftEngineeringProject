using Microsoft.Xna.Framework;

namespace CherryCollector.Graphics
{
    public class AnimationFrame
    {
        public Rectangle SourceRectangle { get; set; }
        public AnimationFrame(Rectangle sourceRectangleIn)
        {
            SourceRectangle = sourceRectangleIn;
        }
    }
}
