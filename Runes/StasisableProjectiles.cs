using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using TLoZ.Projectiles.Runes;

namespace TLoZ.Runes
{
    public static class StasisableProjectiles
    {
        public static List<int> SuitableProjectiles;
        public static void Load()
        {
            SuitableProjectiles = new List<int>();
            SuitableProjectiles.Add(ModContent.ProjectileType<BombRound>());
            SuitableProjectiles.Add(ModContent.ProjectileType<BombSquare>());
            SuitableProjectiles.Add(ProjectileID.Boulder);
        }
        public static void Unload()
        {
            SuitableProjectiles.Clear();
        }
    }
}
