using Microsoft.Xna.Framework;
using Project1;

internal class MovementManager
{
    public void Move(IMovable movable)
    {
        var direction = movable.InputReader.ReadInput();
        var distance = new Vector2(direction.X * movable.Speed.X, 0);

        movable.Position = new Vector2(movable.Position.X + distance.X, movable.Position.Y);
    }
}
