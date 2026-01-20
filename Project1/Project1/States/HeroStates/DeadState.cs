using CherryCollector.Entities.World;
using Microsoft.Xna.Framework;

namespace CherryCollector.States.HeroStates
{
    public class DeadState : IHeroState
    {
        public void Enter(Hero hero)
        {
            hero.Speed = Vector2.Zero;
            hero.PhysicsManager.CancelJump(); // Use PhysicsManager
            hero.AnimationManager.PlayDeath();
        }

        public void Update(Hero hero, GameTime gameTime)
        {
            hero.AnimationManager.Update(Vector2.Zero, gameTime);
        }
    }
}
