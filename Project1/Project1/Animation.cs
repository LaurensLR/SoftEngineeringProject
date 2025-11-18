using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Animation
    {
        public AnimationFrame CurrentFrame { get; set; }
        private List<AnimationFrame> _frames;
        private int _counter;
        private double _secondCounter = 0;

        public Animation()
        {
            _frames = new List<AnimationFrame>();
        }

        public void AddFrame(AnimationFrame frame)
        {
            _frames.Add(frame);
            CurrentFrame = _frames[0];
        }


        //TODO: fix textures to be on 1 png and over whole length
        //public void GetFramesFromTextureProperties(int width, int height, int numberOfWidthSprites, int numberofHeightSprites)
        //{
        //    int widthOfFrame = width / numberOfWidthSprites;
        //    int heightOfFrame = height / numberofHeightSprites;

        //    for (int y = 0; y <= height - heightOfFrame; y+=heightOfFrame)
        //    {
        //        for (int x = 0; x <= width / widthOfFrame; x+= widthOfFrame)
        //        {
        //            _frames.Add(new AnimationFrame(new Rectangle(x, y, widthOfFrame, heightOfFrame)));
        //        }
        //    }
        //}

        public void Update(GameTime gameTime)
        {
            CurrentFrame = _frames[_counter];

            _secondCounter += gameTime.ElapsedGameTime.TotalSeconds;
            int fps = 15;
            if (_secondCounter >= 1d / fps)
            {
                _counter++;
                _secondCounter = 0;
            }
            if (_counter >= _frames.Count())
            {
                _counter = 0;
            }
        }
    }
}
