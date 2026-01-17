using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    /* 
     * DESIGN PATTERN - Factory Pattern:
     * This class centralizes the creation of game objects. The rest of the game doesn't 
     * need to know how to "new up" a Spike or a Block; it just asks the factory.
     * 
     * SOLID - Open/Closed Principle (OCP):
     * The system is "Open" for extension (you can add Bird or Coin types here) 
     * but "Closed" for modification (you don't have to change the Level loading logic).
     */
    public class LevelObjectFactory
    {
        private readonly Texture2D _blockTexture;
        private readonly Texture2D _spikeTexture;

        public LevelObjectFactory(Texture2D blockTexture, Texture2D spikeTexture)
        {
            _blockTexture = blockTexture;
            _spikeTexture = spikeTexture;
        }

        // Return IGameObject instead of just ICollidable
        public IGameObject CreateObject(char type, Vector2 position, int tileSize)
        {
            return type switch
            {
                '#' => new Block(_blockTexture, new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize)),
                '^' => new Spike(_spikeTexture, new Vector2(position.X, position.Y + 3)),
                _ => null
            };
        }
    }
}