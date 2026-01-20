using CherryCollector.Core;
using CherryCollector.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CherryCollector.Entities.Base
{
    /// <summary>
    ///    Enemy CLASS - ABSTRACT BASE FOR ALL ENEMIES
    ///   PURPOSE:
    ///   Abstract base class that provides common functionality for all enemy types.
    ///   Defines the contract (abstract methods) that concrete enemies must fulfill.
    ///   DESIGN PATTERNS APPLIED:     
    ///   [TEMPLATE METHOD PATTERN]     
    ///   Enemy provides a default Draw() implementation that subclasses inherit.    
    ///   Subclasses can override Draw() (like Bat does for centering).    
    ///   Update() is abstract - subclasses MUST provide their own logic.    
    ///   [COMPONENT PATTERN]  
    ///   AnimationManager is a reusable component shared by Hero and all enemies.   
    ///   This avoids duplicating animation code in each enemy class.
    ///   SOLID PRINCIPLES APPLIED:  
    ///   [O] Open/Closed Principle (OCP):   
    ///       Enemy is OPEN for extension (new enemy types like Spider, Ghost)
    ///       but CLOSED for modification of core enemy behavior.   
    ///       New enemies extend Enemy; they don't modify it.     
    ///   [L] Liskov Substitution Principle (LSP):
    /// All Enemy subclasses can be used wherever IGameObject is expected.     
    ///  LevelManager stores enemies in List<IGameObject> polymorphically.  
    ///   [D] Dependency Inversion Principle (DIP):      
    ///      Enemy depends on abstractions (AnimationManager) not concrete logic.   
    ///       Animation details are encapsulated in the manager.  
    ///   [S] Single Responsibility Principle (SRP):     
    ///       Enemy handles common enemy behavior only.     
    ///       Specific movement patterns are in Snail, Bat, etc.  
    /// </summary>
    public abstract class Enemy : IGameObject
    {
        public Vector2 Position { get; set; }
        public abstract Rectangle Bounds { get; }
        public CollisionType CollisionType => CollisionType.Enemy;

        protected Texture2D Texture;


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
        }
    }
}