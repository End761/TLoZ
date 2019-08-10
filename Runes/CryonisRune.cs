using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TLoZ.Players;
using TLoZ.Projectiles.Runes;

namespace TLoZ.Runes
{
    public class CryonisRune : Rune
    {
        public CryonisRune() : base("cryonisRune", "Cryonis", TLoZTextures.UICryonisRune)
        {

        }

        public override bool UseItem(ModItem item, Player player, TLoZPlayer tlozPlayer)
        {
            int x = (int)Main.MouseWorld.X;
            int y = (int)Main.MouseWorld.Y;

            if (Main.tile[x / 16, y / 16].liquid != 0 && player.ownedProjectileCounts[TLoZMod.Instance.ProjectileType("CryonisBlock")] < 3 && tlozPlayer.itemUseDelay == 0)
            {
                tlozPlayer.itemUseDelay = 20;
                for (int i = 0; i < 500; i++)
                {
                    if (Main.tile[x / 16, y / 16 - i].liquid == 0)
                    {
                        Projectile.NewProjectile(new Vector2(x / 16, y / 16) * 16 - new Vector2(0, i * 16), Vector2.Zero, TLoZMod.Instance.ProjectileType("CryonisBlock"), 0, 0, player.whoAmI, 1);
                        break;
                    }
                }
            }
            return true;
        }
    }
}
