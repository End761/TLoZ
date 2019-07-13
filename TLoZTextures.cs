using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace TLoZ
{
    public static class TLoZTextures
    {
        public static Texture2D Misc_StasisArrow;
        public static Texture2D Misc_StasisArrowMiddle;
        public static Texture2D Misc_StasisChain;

        public static Texture2D UI_BombRune1;
        public static Texture2D UI_BombRune2;
        public static Texture2D UI_MagnesisRune;
        public static Texture2D UI_StasisRune;
        public static Texture2D UI_CryonisRune;
        public static void Load()
        {
            Misc_StasisArrow = ModContent.GetTexture("TLoZ/Textures/Misc/StasisArrow");
            Misc_StasisArrowMiddle = ModContent.GetTexture("TLoZ/Textures/Misc/StasisArrowMiddle");
            Misc_StasisChain = ModContent.GetTexture("TLoZ/Textures/Misc/StasisChains");

            UI_BombRune1 = ModContent.GetTexture("TLoZ/Textures/UI/Runes/BombRune1");
            UI_BombRune2 = ModContent.GetTexture("TLoZ/Textures/UI/Runes/BombRune2");
            UI_StasisRune = ModContent.GetTexture("TLoZ/Textures/UI/Runes/StasisRune");
            UI_MagnesisRune = ModContent.GetTexture("TLoZ/Textures/UI/Runes/MagnesisRune");
            UI_CryonisRune = ModContent.GetTexture("TLoZ/Textures/UI/Runes/CryonisRune");
        }
        public static void Unload()
        {
            Misc_StasisArrow = null;
            Misc_StasisArrowMiddle = null;
            Misc_StasisChain = null;

            UI_BombRune1 = null;
            UI_BombRune2 = null;
            UI_StasisRune = null;
            UI_CryonisRune = null;
            UI_MagnesisRune = null;
        }
    }
}
