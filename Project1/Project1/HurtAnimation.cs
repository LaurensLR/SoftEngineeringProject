using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
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
