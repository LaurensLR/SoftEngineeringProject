using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Project1
{
    /* 
     * SOLID - Single Responsibility: 
     * Manages the Snail's specific patrol movement and its 25x16 animation slicing.
     */
    public class Snail : Enemy
    {
        private float _speed = 40f; 
        private int _direction = 1; // 1 for right, -1 for left
        
        // Physics bounds (slightly narrower than visual for a more forgiving hit)
        public override Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 20, 16);

        public Snail(Texture2D texture, Vector2 position) : base(texture, position) 
        {
            /* 
             * DESIGN PATTERN - Animation Setup:
             * We slice the 25x16 frames from the sprite sheet.
             * Frame 1: (0,0), Frame 2: (25,0), Frame 3: (50,0).
             */
            var walkAnim = new Animation(fps: 6, loop: true);
            walkAnim.AddFrame(new AnimationFrame(new Rectangle(10, 81, 25, 16)));
            walkAnim.AddFrame(new AnimationFrame(new Rectangle(58, 81, 25, 16)));
            walkAnim.AddFrame(new AnimationFrame(new Rectangle(106, 81, 25, 16)));

            // Reusing walk animation for all states since this snail is always moving.
            AnimationManager = new AnimationManager(walkAnim, walkAnim, walkAnim, walkAnim);
        }

        /* 
         * SOLID - Dependency Inversion:
         * We pass the 'worldObjects' so the Snail can 'see' the floor and walls.
         */
        public void Update(GameTime gameTime, List<IGameObject> worldObjects)
        {
            CheckPatrol(worldObjects);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Basic horizontal patrol movement
            Position = new Vector2(Position.X + (_speed * _direction * dt), Position.Y);
            
            /* 
             * SOLID - Delegation:
             * Passing direction to the manager automatically handles the facing-left flip.
             */
            AnimationManager.Update(new Vector2(_direction, 0), gameTime);
        }

        // Default interface implementation
        public override void Update(GameTime gameTime) { /* logic moved to overload */ }

        /* 
         * DESIGN PATTERN - Sensor Pattern:
         * We create two imaginary boxes: one in front (Wall Sensor) 
         * and one down-and-front (Edge Sensor).
         */
        private void CheckPatrol(List<IGameObject> worldObjects)
        {
            int lookAhead = _direction > 0 ? 22 : -5;
            
            // 1. Wall Sensor: Checking if we hit a solid block
            Rectangle wallSensor = new Rectangle((int)Position.X + lookAhead, (int)Position.Y, 5, 16);
            
            // 2. Edge Sensor: Checking if there is still floor ahead
            Rectangle edgeSensor = new Rectangle((int)Position.X + lookAhead, (int)Position.Y + 17, 5, 5);

            bool wallHit = false;
            bool floorFound = false;

            foreach (var obj in worldObjects)
            {
                if (obj.CollisionType != CollisionType.Block) continue;

                if (obj.Bounds.Intersects(wallSensor)) wallHit = true;
                if (obj.Bounds.Intersects(edgeSensor)) floorFound = true;
            }

            // Flip if we hit a wall OR if we reach the end of the platform
            if (wallHit || !floorFound)
            {
                _direction *= -1;
            }
        }
    }
}