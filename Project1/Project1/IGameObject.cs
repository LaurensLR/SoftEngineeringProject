using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    /* 
     * DESIGN PATTERN - Composite Pattern:
     * IGameObject acts as a composite that brings together Updating, Drawing, and Collision.
     * SOLID - Liskov Substitution Principle (LSP):
     * Any class that implements IGameObject can be treated uniformly by the Game engine loops.
     */
    public interface IGameObject : IUpdatable, IDrawable, ICollidable { }
}
