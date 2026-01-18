using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Project1
{
    /* 
     * SOLID - Single Responsibility: 
     * Manages the Bat's specific flying pattern and irregular sprite slicing.
     */
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

            /* 
             * DESIGN PATTERN - Component Setup:
             * Frame 1 is 29x15, Frame 2 is 26x16. 
             * We define them exactly as they appear on your sprite sheet.
             */
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

        /* 
         * REFACTORING - Visual Centering:
         * Because the frames have different widths, we calculate a drawing offset 
         * to keep the bat centered on its actual Position.
         */
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