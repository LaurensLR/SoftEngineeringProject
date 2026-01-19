using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CherryCollector.Graphics;

namespace CherryCollector.Graphics
{
    internal class DeathAnimation : Animation
    {
        public DeathAnimation(Texture2D texture) : base(fps: 4, loop: false)
        {
            Texture = texture;

            AddFrame(new AnimationFrame(new Rectangle(2, 3, 26, 30)));
            AddFrame(new AnimationFrame(new Rectangle(35, 7, 30, 26)));
            AddFrame(new AnimationFrame(new Rectangle(73, 9, 30, 24)));
            AddFrame(new AnimationFrame(new Rectangle(115, 15, 34, 18)));
            AddFrame(new AnimationFrame(new Rectangle(153, 13, 38, 20)));
            AddFrame(new AnimationFrame(new Rectangle(199, 14, 38, 18)));
        }
    }
}
