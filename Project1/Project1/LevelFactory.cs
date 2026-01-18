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
     * The system is "Open" for extension
     * but "Closed" for modification (you don't have to change the Level loading logic).
     */
    public class LevelObjectFactory
    {
        private readonly Texture2D _blockTexture, _spikeTexture, _cherryTexture, _doorTexture, _snailTexture, _batTexture;

        public LevelObjectFactory(Texture2D block, Texture2D spike, Texture2D cherry, Texture2D door, Texture2D snail, Texture2D bat)
        {
            _blockTexture = block;
            _spikeTexture = spike;
            _cherryTexture = cherry;
            _doorTexture = door;
            _snailTexture = snail;
            _batTexture = bat;
        }

        public IGameObject CreateObject(char type, Vector2 position, int tileSize)
        {
            return type switch
            {
                '#' => new Block(_blockTexture, new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize)),
                '^' => new Spike(_spikeTexture, new Vector2(position.X, position.Y + 3)),
                'C' => new Cherry(_cherryTexture, position),
                'D' => new Door(_doorTexture, position), 
                'S' => new Snail(_snailTexture, position), 
                'B' => new Bat(_batTexture, position), 

                _ => null
            };
        }
    }
}