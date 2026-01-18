using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Project1
{
    /* 
     * SOLID - Open/Closed Principle:
     * This base class is now "Closed" to further modification of logic, 
     * but "Open" for different animations to define their own frame rates 
     * through the constructor.
     */
    public class Animation
    {
        public Texture2D Texture { get; protected set; }
        public AnimationFrame CurrentFrame { get; private set; }
        
        private readonly List<AnimationFrame> _frames = new();
        private int _counter;
        private double _accumulatedTime = 0;

        private readonly bool _loop;
        private bool _isFinished;

        public bool IsFinished => _isFinished;

        /* 
         * SOLID - Single Responsibility: 
         * Storing the FPS here allows subclasses to define how fast they run 
         * (e.g., a fast Walk vs a slow Idle).
         */
        public int Fps { get; set; }

        public Animation(int fps = 10, bool loop = true)
        {
            Fps = fps;
            _loop = loop;
        }

        public void AddFrame(AnimationFrame frame)
        {
            _frames.Add(frame);
            CurrentFrame ??= frame;
        }

        public void Update(GameTime gameTime)
        {
            if (_isFinished || _frames.Count == 0) return;

            /* 
             * REFACTORING - Frame Independence (Lag Compensation):
             * By using a 'while' loop, we subtract the exact frame duration from our counter.
             * If the game takes a long time to update, the animation will skip frames 
             * to "catch up," ensuring the animation length is always consistent in seconds.
             */
            _accumulatedTime += gameTime.ElapsedGameTime.TotalSeconds;
            double secondsPerFrame = 1.0 / Fps;

            while (_accumulatedTime >= secondsPerFrame)
            {
                _accumulatedTime -= secondsPerFrame;
                _counter++;

                if (_counter >= _frames.Count)
                {
                    if (_loop)
                    {
                        _counter = 0;
                    }
                    else
                    {
                        _counter = _frames.Count - 1;
                        _isFinished = true;
                        break; 
                    }
                }
            }

            CurrentFrame = _frames[_counter];
        }

        public void Reset()
        {
            _counter = 0;
            _accumulatedTime = 0;
            _isFinished = false;
            if (_frames.Count > 0) CurrentFrame = _frames[0];
        }
    }
}
