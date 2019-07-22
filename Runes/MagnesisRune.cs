using Microsoft.Xna.Framework;
using System.Collections.Generic;
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
            int x = (int)(Main.MouseWorld.X / 16);
            int y = (int)(Main.MouseWorld.Y / 16);
            int proj = TLoZ.Instance.ProjectileType<PickedUpTile>();
            if (WorldGen.SolidTile(Main.tile[x, y]) && player.ownedProjectileCounts[proj] <= 0)
            {
                List<Vector2> tilesToKill = new List<Vector2>();
                int projectile = Projectile.NewProjectile(new Vector2(x * 16, y * 16), Vector2.Zero, proj, 0, 0, player.whoAmI, 1);
                PickedUpTile upTile = (PickedUpTile)Main.projectile[projectile].modProjectile;
                for(int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (WorldGen.SolidTile(Main.tile[x + j, y + i]))
                        {
                            upTile.tileIDs[i * 4 + j] = Main.tile[x + j, y + i].type;
                            upTile.tilePositions[i * 4 + j] = new Vector2(j, i) * 16;
                            upTile.tileFrames[i * 4 + j] = new Vector2(Main.tile[x + j, y + i].frameX, Main.tile[x + j, y + i].frameY);
                            tilesToKill.Add(new Vector2(x + j, y + i));
                        }
                    }
                }
                int width = 16;
                int height = 16;
                for (int i = 0; i < 16; i++)
                {
                    if (upTile.tileIDs[i] != -1)
                    {
                        if (i < 4)
                            width = 16 + (i) * 16;
                        else
                            height = 16 + ((i - 1) / 4 * 16);

                    }
                }
                Main.projectile[projectile].width = width;
                Main.projectile[projectile].height = height;
                foreach (Vector2 vector in tilesToKill)
                {
                    WorldGen.KillTile((int)vector.X, (int)vector.Y, false, false, true);
                }
                tilesToKill.Clear();
            }
            else
                return false;
            return true;
        }
    }
}
