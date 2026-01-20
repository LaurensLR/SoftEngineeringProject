using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CherryCollector.Graphics
{
    /// <summary>
    ///   Animation CLASS - SPRITE ANIMATION SYSTEM  
    ///   PURPOSE:       
    ///   Manages a sequence of animation frames, updating which frame to display    
    ///   based on elapsed time and frames per second (FPS).   
    ///   LAG COMPENSATION:  
    ///   The while-loop in Update() handles frame skipping when the game lags.    
    ///   If deltaTime is large, multiple frames may advance in one update call.
    ///   This ensures animations complete in consistent real-world time.   
    /// DESIGN PATTERNS APPLIED:    
    /// [TEMPLATE METHOD PATTERN (partial)]   
    ///   Animation is a base class that concrete animations extend:     
    ///    • IdleAnimation, WalkAnimation, HurtAnimation, DeathAnimation     
    ///   Subclasses call AddFrame() in constructor to define their frames.    
    ///   [FLYWEIGHT PATTERN (implicit)]   
    ///   AnimationFrame objects store only SourceRectangle data.     
    ///   The actual Texture2D is shared (stored once in Animation.Texture).  
    ///   This saves memory when many frames exist.   
    ///   SOLID PRINCIPLES APPLIED:   
    ///   [S] Single Responsibility Principle (SRP):     
    /// Animation ONLY handles frame sequencing and timing.    
    ///       It doesn't:
    ///         • Draw sprites (AnimationManager/Entity does that)     
    ///       • Decide which animation to play (AnimationManager does)        
    ///         • Load textures (done externally in Content pipeline)      
    ///   [O] Open/Closed Principle (OCP):    
    ///       New animation types are created by extending Animation.         
    ///    Base class provides Update(), Reset(), AddFrame() - subclasses only 
    ///       define their specific frames.  
    /// </summary>
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


        public int Fps { get; set; }

        public Animation(int fps = 10, bool loop = true)
        {
            Fps = fps;
            _loop = loop;
        }

        public void AddFrame(AnimationFrame frame)
        {
            _frames.Add(frame);
            CurrentFrame = frame;
        }

        public void Update(GameTime gameTime)
        {
            if (_isFinished || _frames.Count == 0) return;


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
