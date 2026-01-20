using CherryCollector.Core;
using CherryCollector.Entities.Base;
using CherryCollector.Graphics;
using CherryCollector.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CherryCollector.Entities.Enemies
{
    /// <summary>
    ///       Snail CLASS - GROUND PATROL ENEMY       
    ///   PURPOSE:  
    ///   A ground-based enemy that patrols horizontally along platforms.  
    ///   Turns around when hitting walls or reaching platform edges.   
    ///   DESIGN PATTERNS APPLIED:       
    ///   [SENSOR PATTERN]  
    ///   Snail uses two invisible "sensor" rectangles to detect its environment:   
    ///     • wallSensor: Detects walls ahead (triggers turn)    
    ///     • edgeSensor: Detects floor ahead (no floor = turn)  
    ///   This is a common AI pattern in platformer games.      
    ///   [TEMPLATE METHOD PATTERN - Override]    
    ///   Snail inherits Draw() from Enemy but provides its own Update().    
    ///   The Update(GameTime, List) overload provides world awareness.   
    ///   SOLID PRINCIPLES APPLIED:  
    ///   [S] Single Responsibility Principle (SRP):      
    ///   Snail ONLY handles snail-specific patrol behavior.  
    ///   Common enemy logic (texture, animation, collision type) is in Enemy base. 
    ///   [O] Open/Closed Principle (OCP):    
    ///       New ground enemies (Spider, Beetle) can extend Enemy without 
    ///       modifying Snail or Enemy classes.   
    ///   [D] Dependency Inversion Principle (DIP): 
    ///       Snail receives worldObjects as parameter - it doesn't create or own    
    ///       the world data. This allows testing with mock objects.
    /// </summary>
    public class Snail : Enemy
    {
        private float _speed = 40f;
        private int _direction = 1; // 1 for right, -1 for left

        // Physics bounds (slightly narrower than visual for a more forgiving hit)
        public override Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 20, 16);

        public Snail(Texture2D texture, Vector2 position) : base(texture, position)
        {

            var walkAnim = new Animation(fps: 6, loop: true);
            walkAnim.AddFrame(new AnimationFrame(new Rectangle(10, 81, 25, 16)));
            walkAnim.AddFrame(new AnimationFrame(new Rectangle(58, 81, 25, 16)));
            walkAnim.AddFrame(new AnimationFrame(new Rectangle(106, 81, 25, 16)));

            // Reusing walk animation for all states since this snail is always moving.
            AnimationManager = new AnimationManager(walkAnim, walkAnim, walkAnim, walkAnim);
        }

        public void Update(GameTime gameTime, List<IGameObject> worldObjects)
        {
            CheckPatrol(worldObjects);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Basic horizontal patrol movement
            Position = new Vector2(Position.X + _speed * _direction * dt, Position.Y);


            AnimationManager.Update(new Vector2(_direction, 0), gameTime);
        }

        // Default interface implementation
        public override void Update(GameTime gameTime) { }


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