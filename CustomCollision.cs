using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLoZ
{
    public static class CustomCollision
    {
        public static bool CheckCollision(Vector2 a, Vector2 aSize, Vector2 b, Vector2 bSize)
        {
            if (a.X + aSize.X >= b.X
                && b.X + bSize.X >= a.X &&
                a.Y + aSize.Y >= b.Y &&
                b.Y + bSize.Y >= a.Y)
                return true;
            return false;
        }
    }
}
