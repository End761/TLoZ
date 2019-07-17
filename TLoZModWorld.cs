using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.World.Generation;
using TLoZ.Tiles.Misc;

namespace TLoZ
{
    public class TLoZModWorld : ModWorld
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            tasks.Add(new PassLegacy("SwordPedestal",
                delegate (GenerationProgress generationProgress)
                {
                    for (int i = 0; i < Main.maxTilesX; i++)
                    {
                        for (int j = 0; j < Main.maxTilesY; j++)
                        {
                            Tile firstTile = Main.tile[i, j];
                            Tile secondTile = Main.tile[i + 1, j];
                            Tile belowFirstTile = Main.tile[i, j + 1];
                            Tile belowSecondTile = Main.tile[i, j + 1];
                            if (firstTile != null && belowFirstTile != null && !firstTile.active() && belowFirstTile.active())
                            {
                                WorldGen.PlaceObject(i, j, mod.TileType<MasterSwordPedestal>());
                            }
                        }
                    }
                }
                )
                );
        }
    }
}
