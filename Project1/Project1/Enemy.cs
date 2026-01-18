using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    /* 
     * SOLID - Open/Closed Principle:
     * This base class is now open for extension (any enemy can use it) 
     * but closed for modification of core rendering and collision logic.
     */
    public abstract class Enemy : IGameObject
    {
        public Vector2 Position { get; set; }
        public abstract Rectangle Bounds { get; }
        public CollisionType CollisionType => CollisionType.Enemy;

        protected Texture2D Texture;
        
        /* 
         * DESIGN PATTERN - Component Pattern:
         * Using AnimationManager here maintains consistency with the Hero class.
         * This allows all enemies to eventually have Idle, Walk, and Hurt states.
         */
        protected AnimationManager AnimationManager;

        public Enemy(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // DESIGN PATTERN - Null Object Check:
            // Only draw if the animation components are correctly initialized.
            if (AnimationManager?.CurrentAnimation?.CurrentFrame != null)
            {
                var effect = AnimationManager.FacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                
                spriteBatch.Draw(Texture, Position, 
                                 AnimationManager.CurrentAnimation.CurrentFrame.SourceRectangle, 
                                 Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
            }
            else
            {
                spriteBatch.Draw(Texture, Bounds, Color.White);
            }
        }

        public void OnCollision(IGameObject other)
        {
            // Enemies are invincible in this version, so they don't react to being hit.
        }
    }
}