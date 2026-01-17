using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Project1
{
    /*
     * SOLID - Single Responsibility: This class now ONLY manages the map layout.
     * It doesn't know HOW to create a block or a spike; it asks the Factory.
     */
    public class Level
    {
        // Use IGameObject to support both physics and drawing
        public List<IGameObject> LevelObjects { get; private set; } = new();
        private readonly int _tileSize = 16;
        private readonly List<string[]> _levels = new();
        private readonly LevelObjectFactory _factory;

        public Level(LevelObjectFactory factory)
        {
            _factory = factory;
            InitializeLevels();
            LoadLevel(0);
        }

        private void InitializeLevels()
        {
            // Level layout string array (kept same as before)
            _levels.Add(new string[]
            {
                "..................................................",
                "..................................................",
                "..........#####...................................",
                "..................................................",
                ".....................###..........................",
                ".........###......................................",
                "..................................................",
                "..........................^^...###................",
                ".....###..................####....................",
                ".....................#####........................",
                "..................................................",
                "...........###....................................",
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
        }

        public void LoadLevel(int index)
        {
            if (index < 0 || index >= _levels.Count) return;

            LevelObjects.Clear();
            string[] map = _levels[index];

            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    // The factory now returns an object that implements IGameObject
                    var obj = _factory.CreateObject(map[y][x], new Vector2(x * _tileSize, y * _tileSize), _tileSize);
                    if (obj != null) LevelObjects.Add(obj);
                }
            }
        }
    }
}