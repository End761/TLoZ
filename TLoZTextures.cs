using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using TLoZ.Runes;

namespace TLoZ
{
    public static class TLoZTextures
    {
        public static void Load()
        {
            MiscStasisArrow = ModContent.GetTexture("TLoZ/Textures/Misc/StasisArrow");
            MiscStasisArrowMiddle = ModContent.GetTexture("TLoZ/Textures/Misc/StasisArrowMiddle");
            MiscStasisChain = ModContent.GetTexture("TLoZ/Textures/Misc/StasisChains");

            UIBombRoundRune = ModContent.GetTexture("TLoZ/Textures/UI/Runes/" + nameof(BombRoundRune));
            UIBombSquareRune = ModContent.GetTexture("TLoZ/Textures/UI/Runes/" + nameof(BombSquareRune));
            UIStasisRune = ModContent.GetTexture("TLoZ/Textures/UI/Runes/StasisRune");
            UIMagnesisRune = ModContent.GetTexture("TLoZ/Textures/UI/Runes/MagnesisRune");
            UICryonisRune = ModContent.GetTexture("TLoZ/Textures/UI/Runes/CryonisRune");
        }

        public static void Unload()
        {
            MiscStasisArrow = null;
            MiscStasisArrowMiddle = null;
            MiscStasisChain = null;

            UIBombSquareRune = null;
            UIBombRoundRune = null;
            UIStasisRune = null;
            UICryonisRune = null;
            UIMagnesisRune = null;
        }

        public static Texture2D MiscStasisArrow { get; private set; }
        public static Texture2D MiscStasisArrowMiddle { get; set; }
        public static Texture2D MiscStasisChain { get; set; }

        public static Texture2D UIBombSquareRune { get; private set; }
        public static Texture2D UIBombRoundRune { get; private set; }
        public static Texture2D UIMagnesisRune { get; private set; }
        public static Texture2D UIStasisRune { get; private set; }
        public static Texture2D UICryonisRune { get; private  set; }
    }
}
