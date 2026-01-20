using CherryCollector.Entities.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CherryCollector.Graphics;
using CherryCollector.Systems;
using System;

namespace CherryCollector.Entities.Enemies
{
    /// <summary>
    ///   Bat CLASS - FLYING HOVER ENEMY    
    ///   PURPOSE:   
    ///   A flying enemy that hovers in place using a sine wave motion.    
    ///   Creates aerial hazards that the Hero must jump over or avoid. 
    ///   DESIGN PATTERNS APPLIED:    
    ///   [TEMPLATE METHOD PATTERN - Override]      
    ///   Bat overrides BOTH Update() AND Draw() from Enemy:  
    ///     • Update(): Custom sine wave movement  
    ///     • Draw(): Custom centering logic for variable-size frames   
    ///   [COMPONENT PATTERN]     
    ///   Uses shared AnimationManager from Enemy base for consistent animation   
    ///   handling across all enemy types.       
    ///   SOLID PRINCIPLES APPLIED:      
    ///   [S] Single Responsibility Principle (SRP):   
    ///       Bat ONLY handles bat-specific hover behavior and drawing.   
    ///       Collision handling is done by CollisionManager + Hero.       
    ///   [O] Open/Closed Principle (OCP):     
    ///       New flying enemies (Bird, Ghost) can extend Enemy without     
    ///       modifying Bat or Enemy classes. 
    ///   [L] Liskov Substitution Principle (LSP):
    ///       Bat can replace Enemy (or any IGameObject) polymorphically. 
    ///    LevelManager treats all game objects uniformly.     
    /// </summary>
    public class Bat : Enemy
    {
        private double _timer;
        private readonly float _amplitude = 30f; // Hover height
        private readonly float _frequency = 3f; // Hover speed
        private readonly Vector2 _startPos;

        // PHYSICS BOUNDS: Fixed size (16x12) ensures collision remains stable 
        // even when visual frames change size.
        public override Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 16, 12);

        public Bat(Texture2D texture, Vector2 position) : base(texture, position)
        {
            _startPos = position;


            var flyAnim = new Animation(fps: 8, loop: true);
            flyAnim.AddFrame(new AnimationFrame(new Rectangle(1, 8, 29, 15)));
            flyAnim.AddFrame(new AnimationFrame(new Rectangle(33, 7, 26, 16)));

            // Reusing fly anim for all states for this hazard
            AnimationManager = new AnimationManager(flyAnim, flyAnim, flyAnim, flyAnim);
        }

        public override void Update(GameTime gameTime)
        {
            _timer += gameTime.ElapsedGameTime.TotalSeconds * _frequency;

            // Hover logic: Move vertically in a Sine wave
            float offset = (float)Math.Sin(_timer) * _amplitude;
            Position = new Vector2(_startPos.X, _startPos.Y + offset);

            // Update visuals
            AnimationManager.Update(Vector2.Zero, gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (AnimationManager?.CurrentAnimation?.CurrentFrame == null) return;

            var sourceRect = AnimationManager.CurrentAnimation.CurrentFrame.SourceRectangle;

            // Calculate center offset so the 29px and 26px frames align at their centers
            Vector2 origin = new Vector2(sourceRect.Width / 2f, sourceRect.Height / 2f);

            spriteBatch.Draw(Texture,
                             Position + origin, // Draw relative to center
                             sourceRect,
                             Color.White,
                             0f,
                             origin,
                             1f,
                             SpriteEffects.None,
                             0f);
        }
    }
}