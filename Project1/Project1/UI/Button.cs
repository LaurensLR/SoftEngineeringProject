using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CherryCollector.UI
{
    /// <summary>
    ///          Button CLASS - INTERACTIVE UI ELEMENT    
    ///   PURPOSE:     
    ///   A reusable UI button component that handles mouse click detection and      
    ///   visual rendering. Used in menu screens (Start, GameOver, Win states).      
    ///     CLICK DETECTION:              
    ///      1. Get mouse screen position   
    ///      2. Transform to design space (800x480) using inverse matrix       
    ///      3. Check if transformed position is inside button bounds      
    ///      4. Check if left mouse button is pressed  
    ///      5. Return true only if BOTH conditions met 
    ///   COORDINATE TRANSFORMATION:       
    ///   The game renders at 800x480 but the window can be resized. The scaleMatrix 
    ///   transforms design coordinates to screen coordinates. To check clicks, we   
    ///   INVERT this matrix to transform screen mouse position back to design space.
    ///   DESIGN PATTERNS APPLIED:    
    ///   [LAZY INITIALIZATION PATTERN]            
    ///   The _pixel texture (used to draw borders) is created only when Draw() is   
    ///   first called, not in the constructor. This:           
    ///     • Avoids creating textures before graphics device is ready               
    ///     • Shares one texture across ALL Button instances (static field)        
    ///     • Saves memory - one 1x1 pixel texture instead of one per button     
    ///   [FLYWEIGHT PATTERN (partial)]          
    ///   The static _pixel texture is shared across all Button instances.     
    ///   Each button only stores its unique data (bounds, text, color).             
    ///   SOLID PRINCIPLES APPLIED:  
    ///   [S] Single Responsibility Principle (SRP):        
    ///       Button handles TWO related things:            
    ///   • Click detection (IsClicked)       
    /// • Visual rendering (Draw)              
    ///       It does NOT handle what happens when clicked - that's the caller's job.
    ///   [O] Open/Closed Principle (OCP):                   
    ///     Button can be extended for new behaviors (hover effects, sounds)      
    ///       without modifying existing code. Text property allows dynamic updates.  
    /// </summary>
    public class Button
    {
        private readonly Rectangle _bounds;
        private readonly SpriteFont _font;
        private readonly Color _color;
        private static Texture2D _pixel; // Static texture for border drawing


        public string Text { get; set; }

        public Button(Rectangle bounds, string text, SpriteFont font, Color color)
        {
            _bounds = bounds;
            Text = text;
            _font = font;
            _color = color;
        }


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