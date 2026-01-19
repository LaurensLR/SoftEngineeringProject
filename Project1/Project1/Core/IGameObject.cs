namespace CherryCollector.Core
{
    /* 
     * DESIGN PATTERN - Composite Pattern:
     * IGameObject now composes the specific functional interfaces.
     */
    public interface IGameObject : ICollidable, IUpdatable, IDrawable
    {
        // Methods Update() and Draw() are now inherited from the interfaces above.
    }
}
