using Microsoft.Xna.Framework;

namespace Project1
{
    public enum CollisionType
    {
        Hero,
        Block,
        Spike
    }

    public interface ICollidable
    {
        Rectangle Bounds { get; }
        CollisionType CollisionType { get; }
        void OnCollision(ICollidable other);
    }
}
