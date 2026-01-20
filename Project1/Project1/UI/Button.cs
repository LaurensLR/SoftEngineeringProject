using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CherryCollector.UI
{
    /*
     * SOLID - Single Responsibility: 
     * This class handles UI interaction logic and rendering including borders.
     */
    public class Button
    {
        private readonly Rectangle _bounds;
        private readonly SpriteFont _font;
        private readonly Color _color;
        private static Texture2D _pixel; // Static texture for border drawing

        // MODIFIED: Text is now a public property so it can be updated dynamicall (e.g. for toggles)
        public string Text { get; set; }

        public Button(Rectangle bounds, string text, SpriteFont font, Color color)
        {
            _bounds = bounds;
            Text = text;
            _font = font;
            _color = color;
        }

        /* 
         * DESIGN PATTERN - Coordinate Transformation:
         * We take the world scaling matrix and invert it to find where the mouse 
         * would be if the screen was still 800x480.
         */
        public bool IsClicked(Matrix scaleMatrix)
        {
            var mouse = Mouse.GetState();

            // Transform mouse coordinates back to design space
            Vector2 mousePos = Vector2.Transform(new Vector2(mouse.X, mouse.Y), Matrix.Invert(scaleMatrix));

            return _bounds.Contains(mousePos) && mouse.LeftButton == ButtonState.Pressed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Create the 1x1 pixel texture if it doesn't exist (Design Pattern: Lazy Initialization)
            if (_pixel == null)
            {
                _pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pixel.SetData(new[] { Color.White });
            }

            // Draw Border (top, bottom, left, right)
            int borderSize = 2;
            spriteBatch.Draw(_pixel, new Rectangle(_bounds.Left, _bounds.Top, _bounds.Width, borderSize), Color.White); // Top
            spriteBatch.Draw(_pixel, new Rectangle(_bounds.Left, _bounds.Bottom - borderSize, _bounds.Width, borderSize), Color.White); // Bottom
            spriteBatch.Draw(_pixel, new Rectangle(_bounds.Left, _bounds.Top, borderSize, _bounds.Height), Color.White); // Left
            spriteBatch.Draw(_pixel, new Rectangle(_bounds.Right - borderSize, _bounds.Top, borderSize, _bounds.Height), Color.White); // Right

            // Center original text inside the border
            Vector2 textSize = _font.MeasureString(Text);
            Vector2 textPos = new Vector2(
                _bounds.Center.X - textSize.X / 2,
                _bounds.Center.Y - textSize.Y / 2
            );

            spriteBatch.DrawString(_font, Text, textPos, _color);
        }
    }
}