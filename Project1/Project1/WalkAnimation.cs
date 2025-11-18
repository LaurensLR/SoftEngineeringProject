using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    internal class WalkAnimation : Animation
    {

        public WalkAnimation(Texture2D texture)
        {
            Texture = texture;

            AddFrame(new AnimationFrame(new Rectangle(31, 34, 13, 14)));
            AddFrame(new AnimationFrame(new Rectangle(49, 34, 13, 14)));
            AddFrame(new AnimationFrame(new Rectangle(67, 34, 13, 14)));
            AddFrame(new AnimationFrame(new Rectangle(84, 34, 13, 14)));
            AddFrame(new AnimationFrame(new Rectangle(100, 34, 13, 14)));
            AddFrame(new AnimationFrame(new Rectangle(116, 34, 13, 14)));
            AddFrame(new AnimationFrame(new Rectangle(133, 34, 13, 14)));
            AddFrame(new AnimationFrame(new Rectangle(148, 34, 13, 14)));
        }
    }
}