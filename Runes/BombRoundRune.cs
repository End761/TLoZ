using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TLoZ.Players;
using TLoZ.Projectiles.Runes;

namespace TLoZ.Runes
{
    public sealed class BombRoundRune : Rune
    {
        public BombRoundRune() : base("bombRoundRune", "Bomb (Round)", TLoZTextures.UIBombSquareRune)
        {
        }

        public override bool UseItem(ModItem item, Player player, TLoZPlayer tlozPlayer)
        {
            if (!tlozPlayer.HasBomb && player.ownedProjectileCounts[item.mod.ProjectileType<BombRound>()] <= 0)
                Projectile.NewProjectile(player.Center, Vector2.Zero, item.mod.ProjectileType<BombRound>(), 0, 0, player.whoAmI);

            else if (player.ownedProjectileCounts[item.mod.ProjectileType<BombRound>()] > 0)
            {
                player.itemAnimation = 0;
                return false;
            }

            return true;
        }
    }
}