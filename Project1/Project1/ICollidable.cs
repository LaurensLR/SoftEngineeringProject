using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    public enum CollisionType { Hero, Block, Spike, Cherry, Door, Enemy }

    public interface ICollidable
    {
        Rectangle Bounds { get; }
        CollisionType CollisionType { get; }
        void OnCollision(IGameObject other);
    }
}
