using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TLoZ.Players;
using TLoZ.Projectiles.Runes;

namespace TLoZ.Runes
{
    public class MagnesisRune : Rune
    {
        public MagnesisRune() : base("magnesisRune", "Magnesis", TLoZTextures.UIMagnesisRune)
        {

        }
        public override bool UseItem(ModItem item, Player player, TLoZPlayer tlozPlayer)
        {
            int x = (int)(Main.MouseWorld.X);
            int y = (int)(Main.MouseWorld.Y);
            int proj = TLoZ.Instance.ProjectileType<PickedUpTile>();

            if (WorldGen.SolidTile(Main.tile[x / 16, y / 16]) && player.ownedProjectileCounts[proj] <= 0)
                Projectile.NewProjectile(new Vector2(x, y), Vector2.Zero, proj, 0, 0, player.whoAmI, 1);
            else
                return false;
            return true;
        }
    }
}
