using Microsoft.Xna.Framework;

namespace CherryCollector.Core
{
    /* 
     * SOLID - Interface Segregation Principle (ISP):
     * We split the interfaces into small, specific roles. 
     * This way, a static object doesn't HAVE to implement a complex Physics interface if it doesn't need it.
     */
    public interface IUpdatable
    {
        void Update(GameTime gameTime);
    }
}
