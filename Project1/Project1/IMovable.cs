using Microsoft.Xna.Framework;
using Project1.Input;

namespace Project1
{
    public interface IMovable
    {
        Vector2 Position { get; set; }
        Vector2 Speed { get; set; }
        IInputReader InputReader { get; set; }

        // Add these so movement manager can compute collision
        int Width { get; }
        int Height { get; }
    }
}
