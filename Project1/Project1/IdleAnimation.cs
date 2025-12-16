using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    internal class IdleAnimation : Animation
    {

        public IdleAnimation(Texture2D texture) : base(loop:true)
        {
            Texture = texture;

            // Add your idle animation frames here
            AddFrame(new AnimationFrame(new Rectangle(2, 2, 26, 28)));
            AddFrame(new AnimationFrame(new Rectangle(32, 2, 26, 28)));
            AddFrame(new AnimationFrame(new Rectangle(62, 2, 26, 28)));
            AddFrame(new AnimationFrame(new Rectangle(92, 2, 26, 28)));
            // Add more idle frames as needed
        }
    }
}