using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class MovementManager
    {
        public void Move(IMovable movable)
        {
            var direction = movable.InputReader.ReadInput();

            var distance = direction * movable.Speed;
            var futurePosition = movable.Position + distance;
            if((futurePosition.X < (800-26)&& futurePosition.X > 0)&& (futurePosition.Y < (480-28)&&futurePosition.Y > 0))
            {
                movable.Position = futurePosition;
            }

        }
    }
}
