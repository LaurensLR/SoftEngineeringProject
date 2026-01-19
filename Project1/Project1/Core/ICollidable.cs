using Microsoft.Xna.Framework;

namespace CherryCollector.Core
{
    public enum CollisionType { Hero, Block, Spike, Cherry, Door, Enemy }

    public interface ICollidable
    {
        Rectangle Bounds { get; }
        CollisionType CollisionType { get; }
        void OnCollision(IGameObject other);
    }
}
