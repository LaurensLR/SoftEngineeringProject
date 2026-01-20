using CherryCollector.Core;
using CherryCollector.Entities.Enemies;
using CherryCollector.Entities.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace CherryCollector.Levels
{
    /// <summary>
    /// LevelManager CLASS - LEVEL LIFECYCLE COORDINATOR   
    ///   PURPOSE:   
    ///   High-level coordinator for level operations. Provides a clean API for     
    ///   the game to interact with levels without knowing internal details.  
    ///   ENCAPSULATION:    
    ///   PlayingState asks high-level questions:     
    ///    • "Is the level complete?" (not "Is door.IsPlayerInside true?")    
    ///     • "Are all cherries collected?" (not manual LINQ queries)  
    ///   This hides implementation details from game state code.   
    ///   DESIGN PATTERNS APPLIED:    
    ///   [FACADE PATTERN]   
    ///   LevelManager provides a simplified interface to the level subsystem
    ///   [MEDIATOR PATTERN]   
    ///   LevelManager mediates between:
    ///  • Game states (PlayingState) and level data
    ///     • Level objects and the game loop (Update/Draw)    
    ///   Objects don't communicate directly with game states.  
    ///   SOLID PRINCIPLES APPLIED:   
    ///   [S] Single Responsibility Principle (SRP):   
    ///       LevelManager coordinates level lifecycle only.    
    ///       It doesn't:     
    ///     • Parse level maps (Level does that)   
    ///    • Create objects (Factory does that)   
    ///    • Handle hero physics (PhysicsManager does that)
    ///   [O] Open/Closed Principle (OCP):    
    ///       New level features (checkpoints, secrets) can be added by:  
    ///       • Adding new query methods (HasCheckpoint(), GetSecretCount())   
    ///    • Existing code doesn't need modification      
    ///   [L] Liskov Substitution Principle (LSP):   
    ///       LevelManager doesn't prevent extending/overriding behavior.   
    ///       (e.g., CustomLevelManager can override NextLevel() logic)   
    ///   [I] Interface Segregation Principle (ISP):  
    ///       LevelManager uses narrow interfaces (e.g., IGameObject).   
    ///       Clients are not forced to depend on unneeded methods.   
    ///   [D] Dependency Inversion Principle (DIP):  
    ///       LevelManager works with IGameObject interface.    
    ///     Uses LINQ OfType<T> for type-specific queries (Cherry, Door).   
    /// </summary>
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

        public bool AllCherriesCollected()
        {
            return !CurrentLevelObjects.OfType<Cherry>().Any(c => !c.IsCollected);
        }


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


        public void ResetLevel(bool backToStart = false)
        {
            if (backToStart)
            {
                _currentLevelIndex = 0;
            }

            _levelData.LoadLevel(_currentLevelIndex);
        }


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