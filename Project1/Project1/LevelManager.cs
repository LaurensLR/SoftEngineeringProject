using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Project1
{
    /* 
     * DESIGN PATTERN - Mediator/Manager Pattern:
     * LevelManager acts as a mediator between the Level data and the Game engine.
     * 
     * SOLID - Single Responsibility Principle (SRP):
     * This class is ONLY responsible for the lifecycle of the level (loading, updating, and drawing the world).
     */
    public class LevelManager
    {
        private readonly LevelObjectFactory _factory;
        private readonly Level _levelData;
        private int _currentLevelIndex = 0;

        // The list of all active objects in the current level
        public List<IGameObject> CurrentLevelObjects => _levelData.LevelObjects;

        public LevelManager(LevelObjectFactory factory)
        {
            _factory = factory;
            _levelData = new Level(_factory); 
        }

        /* 
         * SOLID - Single Responsibility: 
         * Logic to check if level requirements are met.
         */
        public bool AllCherriesCollected()
        {
            return !CurrentLevelObjects.OfType<Cherry>().Any(c => !c.IsCollected);
        }

        public void ResetLevel()
        {
            _levelData.LoadLevel(_currentLevelIndex);
        }

        public void NextLevel()
        {
            _currentLevelIndex++;
            _levelData.LoadLevel(_currentLevelIndex);
        }

        public void Update(GameTime gameTime)
        {
            // REFACTORING: Passing the world list specifically to types that need it.
            // This is a common pattern in game engines where logic depends on context.
            foreach (var obj in CurrentLevelObjects.ToList()) // ToList to avoid collection modification errors
            {
                if (obj is Snail snail)
                {
                    snail.Update(gameTime, CurrentLevelObjects);
                }
                else
                {
                    obj.Update(gameTime);
                }
            }

            _levelData.LevelObjects.RemoveAll(o => o is Cherry c && c.IsCollected);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var obj in CurrentLevelObjects)
            {
                obj.Draw(spriteBatch);
            }
        }
    }
}