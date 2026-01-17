using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Project1
{
    public class Level
    {
        public List<ICollidable> LevelObjects { get; private set; }
        private int _tileSize = 16; 
        
        private List<string[]> _levels = new List<string[]>();

        public Level(Texture2D blockTexture, Texture2D spikeTexture)
        {
            LevelObjects = new List<ICollidable>();
            InitializeLevels();
            LoadLevel(0, blockTexture, spikeTexture); 
        }

        private void InitializeLevels()
        {
            // Same level data...
             _levels.Add(new string[]
            {
                "..................................................",
                "..................................................",
                "..................................................",
                "..................................................",
                "..................................................",
                ".........###......................................",
                "..................................................",
                "..................................................",
                ".....###...........................###............",
                ".................................###..............",
                "...............................###................",
                "...........###...............###..................",
                "..................................................",
                ".......................................^^^........",
                ".....................................#####........",
                "...............###.......###......................",
                "..................................................",
                "......####........................................",
                "..........................###.....................",
                "..................................................",
                "...........^^..........................###...#....",
                ".......##########........................#...#....",
                ".........................................#...#....",
                ".....................###.................#...#....",
                ".........................................#...#....",
                ".............................###.........#...#....",
                ".........................................#...#....",
                ".......................^^^...............#...#....",
                ".#....................#####..............#...#....",
                "##################################################", 
            });
             _levels.Add(new string[]
            {
                "..................................................",
                "..................................................",
                "##################################################",
            });
        }

        public void LoadLevel(int levelIndex, Texture2D blockTex, Texture2D spikeTex)
        {
            if (levelIndex < 0 || levelIndex >= _levels.Count) return;

            LevelObjects.Clear();
            string[] map = _levels[levelIndex];

            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    char tile = map[y][x];
                    Vector2 position = new Vector2(x * _tileSize, y * _tileSize);

                    if (tile == '#') // Block
                    {
                        Rectangle rect = new Rectangle((int)position.X, (int)position.Y, _tileSize, _tileSize);
                        LevelObjects.Add(new Block(blockTex, rect));
                    }
                    else if (tile == '^') // Spike
                    {
                        // Align to bottom of 16px tile.
                        // Spike is 13px high.
                        // Y = position.Y + (16 - 13) = position.Y + 3
                        Vector2 spikePos = new Vector2(position.X, position.Y + 3);
                        LevelObjects.Add(new Spike(spikeTex, spikePos));
                    }
                }
            }
        }
    }
}