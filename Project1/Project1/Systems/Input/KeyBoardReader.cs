using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CherryCollector.Systems.Input
{
    public class KeyBoardReader : IInputReader
    {
        public Vector2 ReadInput()
        {
            KeyboardState state = Keyboard.GetState();
            var direction = Vector2.Zero;

            if (state.IsKeyDown(Keys.Left))
            {
                direction.X -= 1;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                direction.X += 1;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                direction.Y = 1;
            }

            return direction;
        }
    }
}