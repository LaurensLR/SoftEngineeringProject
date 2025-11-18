using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    internal class IdleAnimation : Animation
    {

        public IdleAnimation(Texture2D texture)
        {
            Texture = texture;

            // Add your idle animation frames here
            AddFrame(new AnimationFrame(new Rectangle(81, 49, 13, 14)));
            AddFrame(new AnimationFrame(new Rectangle(97, 49, 13, 14)));
            AddFrame(new AnimationFrame(new Rectangle(113, 49, 13, 14)));
            AddFrame(new AnimationFrame(new Rectangle(129, 49, 13, 14)));
            // Add more idle frames as needed
        }
    }
}