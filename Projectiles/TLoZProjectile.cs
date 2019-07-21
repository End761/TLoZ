using Terraria;
using Terraria.ModLoader;
using TLoZ.Players;

namespace TLoZ.Projectiles
{
    public abstract class TLoZProjectile : ModProjectile
    {
        public Player Owner => Main.player[projectile.owner];
        public TLoZPlayer TLoZPlayer => TLoZPlayer.Get(Owner);
    }
}