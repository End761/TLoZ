using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace TLoZ.Projectiles.MagnesisStates
{
    public class MoveToCursorState : MagnesisState
    {
        public override void Execute(Projectile projectile)
        {
            projectile.velocity = Main.player[projectile.owner].position - Main.player[projectile.owner].oldPosition;
            projectile.velocity.Y = Helpers.DirectToMouse(projectile.Center, 3f).Y;
            if (Math.Abs(projectile.velocity.X) > 16f)
            {
                Utils.Clamp<float>(projectile.velocity.X, 0, 16f);
            }
            if (Math.Abs(projectile.velocity.Y) > 16f)
            {
                Utils.Clamp<float>(projectile.velocity.Y, 0, 16f);
            }
        }
    }
}
