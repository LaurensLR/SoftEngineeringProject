using CherryCollector.Core;
using CherryCollector.Entities.Enemies;
using CherryCollector.Entities.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace CherryCollector.Levels
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

        /* 
         * REFACTORING - Encapsulation:
         * Exposed high-level questions ("Can I win?") rather than low-level details ("Is door.IsPlayerInside true?").
         */
        public bool CheckLevelCompletion()
        {
            var door = CurrentLevelObjects.OfType<Door>().FirstOrDefault();
            return door != null && door.IsPlayerInside && AllCherriesCollected();
        }

        public bool IsPlayerAtLockedDoor()
        {
            var door = CurrentLevelObjects.OfType<Door>().FirstOrDefault();
            return door != null && door.IsPlayerInside && !AllCherriesCollected();
        }

        /* 
         * SOLID - Single Responsibility: 
         * This method handles resetting the level state.
         * We add an optional parameter to force a full game reset (back to level 0).
         */
        public void ResetLevel(bool backToStart = false)
        {
            if (backToStart)
            {
                _currentLevelIndex = 0;
            }

            _levelData.LoadLevel(_currentLevelIndex);
        }

        /* 
         * SOLID - Single Responsibility: 
         * Logic to determine if the player has finished all available content.
         */
        public bool HasMoreLevels()
        {
            return _currentLevelIndex + 1 < _levelData.LevelCount;
        }

        public void NextLevel()
        {
            _currentLevelIndex++;
            _levelData.LoadLevel(_currentLevelIndex);
        }

        public void Update(GameTime gameTime)
        {
            // Create a copy list to iterate safely
            var objectsToUpdate = new List<IGameObject>(CurrentLevelObjects);

            foreach (var obj in objectsToUpdate)
            {
                if (obj is Snail snail) snail.Update(gameTime, CurrentLevelObjects);
                else obj.Update(gameTime);
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