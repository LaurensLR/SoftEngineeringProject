using CherryCollector.Entities.World;
using Microsoft.Xna.Framework;

namespace CherryCollector.States.HeroStates
{
    // SOLID - Single Responsibility: This class only handles behavior during normal gameplay
    public class NormalState : IHeroState
    {
        public void Enter(Hero hero) { }

        public void Update(Hero hero, GameTime gameTime)
        {
            var objects = hero.LevelManager.CurrentLevelObjects;

            /* 
             * REFACTORING: Replaced separate Movement/Jump calls with a single Physics Update.
             * The PhysicsManager now handles horizontal, vertical, collision, and inputs internally.
             */
            hero.PhysicsManager.Update(hero, objects, gameTime);

            // Input check for Jump is now passed to PhysicsManager
            if (hero.InputReader.ReadInput().Y > 0)
                hero.PhysicsManager.Jump();

            hero.AnimationManager.Update(new Vector2(hero.InputReader.ReadInput().X, 0), gameTime);
        }
    }
}
