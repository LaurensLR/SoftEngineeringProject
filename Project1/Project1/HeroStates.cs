using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Project1
{
    // SOLID - Single Responsibility: This class only handles behavior during normal gameplay
    public class NormalState : IHeroState
    {
        public void Enter(Hero hero) { /* Reset specific physics if needed */ }

        public void Update(Hero hero, GameTime gameTime)
        {
            var objects = hero.LevelManager.CurrentLevelObjects;

            /* 
             * REFACTORING - Frame Independence:
             * We now pass gameTime into the MovementManager. 
             * This ensures that speed * deltaTime is calculated properly inside those methods.
             */
            hero.MovementManager.MoveHorizontally(hero, objects, gameTime);
            hero.MovementManager.MoveVertically(hero, hero.JumpManager, objects, gameTime);

            // Command logic (or old input check) 
            if (hero.InputReader.ReadInput().Y > 0)
                hero.JumpManager.Jump();

            hero.AnimationManager.Update(new Vector2(hero.InputReader.ReadInput().X, 0), gameTime);
        }
    }

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