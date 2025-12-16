using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    internal class WalkAnimation : Animation
    {

        public WalkAnimation(Texture2D texture) : base(loop: true)
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