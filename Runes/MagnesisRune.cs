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
        internal static List<int> magnesisWhiteList;

        public MagnesisRune() : base("magnesisRune", "Magnesis", TLoZTextures.UIMagnesisRune)
        {

        }
        public override bool UseItem(ModItem item, Player player, TLoZPlayer tlozPlayer)
        {
            int x = (int)(Main.MouseWorld.X / 16);
            int y = (int)(Main.MouseWorld.Y / 16);
            int proj = ModContent.ProjectileType<PickedUpTile>();

            if (WorldGen.SolidTile(Main.tile[x, y]) && player.ownedProjectileCounts[proj] <= 0 && magnesisWhiteList.Contains(Main.tile[x, y].type))
            {
                int startX = x;
                int startY = y;

                List<Vector2> tilesToKill = new List<Vector2>();

                int projectile = Projectile.NewProjectile(new Vector2(x * 16, y * 16), Vector2.Zero, proj, 0, 0, player.whoAmI, 1);

                PickedUpTile upTile = (PickedUpTile)Main.projectile[projectile].modProjectile;

                for(int i = 0; i < totalHeight; i++)
                    for(int j = 0; j < totalWidth; j++)
                        if(WorldGen.SolidTile(Main.tile[x - j, y - i]) && magnesisWhiteList.Contains(Main.tile[x - j, y - i].type))
                        {
                            startX = x - j;
                            startY = y - i;
                        }

                for (int i = 0; i < totalHeight; i++)
                    for (int j = 0; j < totalWidth; j++)
                        if (WorldGen.SolidTile(Main.tile[startX + j, startY + i]) && magnesisWhiteList.Contains(Main.tile[startX + j, startY + i].type))
                        {
                            upTile.TileIDs[i * totalWidth + j] = Main.tile[startX + j, startY + i].type;
                            upTile.TilePositions[i * totalWidth + j] = new Vector2(j, i) * 16;
                            upTile.TileFrames[i * totalWidth + j] = new Vector2(Main.tile[startX + j, startY + i].frameX, Main.tile[startX + j, startY + i].frameY);

                            tilesToKill.Add(new Vector2(startX + j, startY + i));
                        }

                Main.projectile[projectile].position = new Vector2(startX, startY) * 16;

                foreach (Vector2 vector in tilesToKill)
                    WorldGen.KillTile((int)vector.X, (int)vector.Y, false, false, true);

                tilesToKill.Clear();
                upTile.MousePosOffset = -new Vector2(startX - x, startY - y) * 16;
            }
            else
                return false;

            return true;
        }

        private const int totalWidth = 5;
        private const int totalHeight = 5;
    }
}
