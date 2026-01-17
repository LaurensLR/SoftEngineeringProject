using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Project1
{
    internal class Animation
    {
        public Texture2D Texture { get; protected set; }
        public AnimationFrame CurrentFrame { get; set; }
        private List<AnimationFrame> _frames;
        private int _counter;
        private double _secondCounter = 0;

        private bool _loop = true;
        private bool _isFinished = false;

        public bool IsFinished => _isFinished;

        // frame rate (frames per second) — small improvement to make it simple to tune
        protected int _fps = 10;

        public Animation()
        {
            _frames = new List<AnimationFrame>();
        }

        public Animation(bool loop = true)
        {
            _frames = new List<AnimationFrame>();
            _loop = loop;
        }

        public void AddFrame(AnimationFrame frame)
        {
            _frames.Add(frame);

            if (CurrentFrame == null)
                CurrentFrame = frame;
        }

        public void Update(GameTime gameTime)
        {
            if (_isFinished) return;
            if (_frames == null || _frames.Count == 0) return;

            CurrentFrame = _frames[_counter];

            _secondCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (_secondCounter >= 1d / _fps)
            {
                _counter++;
                _secondCounter = 0;
            }

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
                }
            }
        }

        public void Reset()
        {
            _counter = 0;
            _secondCounter = 0;
            _isFinished = false;
            CurrentFrame = _frames != null && _frames.Count > 0 ? _frames[0] : null;
        }
    }
}
