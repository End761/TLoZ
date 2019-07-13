using Terraria.ModLoader;

namespace TLoZ
{
    public static class TLoZInput
    {
        public static ModHotKey changeRune;
        public static ModHotKey equipParaglider;
        public static void Load(Mod mod)
        {
            equipParaglider = mod.RegisterHotKey("Use paraglider", "F");
            changeRune = mod.RegisterHotKey("Rune select", "Q");
        }
        public static void Unload()
        {
            equipParaglider = null;
            changeRune = null;
        }
    }
}
