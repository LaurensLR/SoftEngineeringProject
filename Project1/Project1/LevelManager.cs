using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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

        public void Update(GameTime gameTime)
        {
            // Update every object in the world without knowing their specific types (Polymorphism)
            foreach (var obj in CurrentLevelObjects)
            {
                obj.Update(gameTime);
            }
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