using CherryCollector.Entities.World;
using Microsoft.Xna.Framework;

namespace CherryCollector.States.HeroStates
{
    // SOLID - Single Responsibility: Handles logic when the hero has died
    public class DeadState : IHeroState
    {
        public void Enter(Hero hero)
        {
            hero.Speed = Vector2.Zero;
            hero.JumpManager.CancelJump();
            hero.AnimationManager.PlayDeath();
        }

        public void Update(Hero hero, GameTime gameTime)
        {
            hero.AnimationManager.Update(Vector2.Zero, gameTime);
        }
    }
}
