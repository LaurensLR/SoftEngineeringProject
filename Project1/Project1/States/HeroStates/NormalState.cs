using CherryCollector.Entities.World;
using Microsoft.Xna.Framework;

namespace CherryCollector.States.HeroStates
{
    // SOLID - Single Responsibility: This class only handles behavior during normal gameplay
    public class NormalState : IHeroState
    {
        public void Enter(Hero hero) { /* nothing needed */ }

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
}
