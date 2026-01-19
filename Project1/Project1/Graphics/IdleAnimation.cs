using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.Graphics
{
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