using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TLoZ
{
    public static class TLoZInput
    {
        public static ModHotKey ChangeRune;
        public static ModHotKey EquipParaglider;
        public static void Load(Mod mod)
        {
            EquipParaglider = mod.RegisterHotKey("Use paraglider", "F");
            ChangeRune = mod.RegisterHotKey("Rune select", "Q");
        }
        public static void Unload()
        {
            EquipParaglider = null;
            ChangeRune = null;
        }
    }
}
