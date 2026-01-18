using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Project1
{
    /*
     * SOLID - Single Responsibility: 
     * This class only handles basic button rendering and click detection.
     */
    public class Button
    {
        private readonly Rectangle _bounds;
        private readonly string _text;
        private readonly SpriteFont _font;
        private readonly Color _color;

        public Button(Rectangle bounds, string text, SpriteFont font, Color color)
        {
            _bounds = bounds;
            _text = text;
            _font = font;
            _color = color;
        }

        public bool IsClicked()
        {
            var mouse = Mouse.GetState();
            // Note: Since we use a scaleMatrix, we'd normally need to transform mouse coordinates.
            // For now, checking direct containment is simpler for the start/reset logic.
            return _bounds.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Simple text-based button for this example
            spriteBatch.DrawString(_font, _text, new Vector2(_bounds.X, _bounds.Y), _color);
        }
    }
}