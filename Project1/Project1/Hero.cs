using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Hero : IGameObject
    {
        private Texture2D _texture;
        private Vector2 _positie;
        Animation animation;
        private Vector2 _snelheid;
        private Vector2 _versnelling;

        public Hero(Texture2D textureIn)
        {
            _texture = textureIn;
            animation = new Animation();
            animation.AddFrame(new AnimationFrame(new Rectangle(81, 49, 13, 14)));
            animation.AddFrame(new AnimationFrame(new Rectangle(97, 49, 13, 14)));
            animation.AddFrame(new AnimationFrame(new Rectangle(113, 49, 13, 14)));
            animation.AddFrame(new AnimationFrame(new Rectangle(129, 49, 13, 14)));
            //animation.GetFramesFromTextureProperties(_texture.Width, _texture.Height, 16, 14);
            _positie = new Vector2(50, 50);
            _snelheid = new Vector2(1, 1);
            _versnelling = new Vector2(0.01f, 0.01f);
        }

        private Vector2 Limit(Vector2 v, float max)
        {
            if (v.Length() > max)
            {
                var ratio = max / v.Length();
                v.X *= ratio;
                v.Y *= ratio;
            }

            return v;
        }
        private void Move()
        {
            // Don't update position here - let MoveWithMouse handle it
            _snelheid += _versnelling;
            float maximaleSnelheid = 10;
            _snelheid = Limit(_snelheid, maximaleSnelheid);

            MoveWithMouse(); // This updates the position

            // Check boundaries AFTER position is updated
            if (_positie.X > 787 || _positie.X < 0)
            {
                _snelheid = new Vector2
                    (_snelheid.X < 0 ? 1 : -1, _snelheid.Y);
                _versnelling.X *= -1;
                // Clamp position to stay in bounds
                _positie.X = Math.Clamp(_positie.X, 0, 787);
            }
            if (_positie.Y > 466 || _positie.Y < 0)
            {
                _snelheid = new Vector2
                    (_snelheid.X, _snelheid.Y < 0 ? 1 : -1);
                _versnelling.Y *= -1;
                // Clamp position to stay in bounds
                _positie.Y = Math.Clamp(_positie.Y, 0, 466);
            }
        }

        private void MoveWithMouse()
        {
            MouseState state = Mouse.GetState();
            Vector2 mouseVector = new Vector2(state.X, state.Y);
            var richting = mouseVector - _positie;
            richting.Normalize();
            richting = Vector2.Multiply(richting, 0.1f);
            _snelheid += richting;
            _snelheid = Limit(_snelheid, 2);
            _positie += _snelheid; // Only position update
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _positie, animation.CurrentFrame.SourceRectangle, Color.White);


        }

        public void Update(GameTime gameTime)
        {

            animation.Update(gameTime);
            Move();
        }
    }
}
