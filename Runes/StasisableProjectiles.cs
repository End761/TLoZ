using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using TLoZ.Projectiles.Runes;

namespace TLoZ.Runes
{
    public static class StasisableProjectiles
    {
        public static List<int> SuitableProjectiles;
        public static void Load()
        {
            SuitableProjectiles = new List<int>();
            SuitableProjectiles.Add(TLoZ.Instance.ProjectileType<BombRound>());
            SuitableProjectiles.Add(TLoZ.Instance.ProjectileType<BombSquare>());
            SuitableProjectiles.Add(ProjectileID.Boulder);
        }
        public static void Unload()
        {
            SuitableProjectiles.Clear();
        }
    }
}
