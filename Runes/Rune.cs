using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using TLoZ.Players;
using WebmilioCommons.Managers;

namespace TLoZ.Runes
{
    public abstract class Rune : IHasUnlocalizedName
    {
        protected Rune(string unlocalizedName, string displayName, Texture2D runeTexture)
        {
            UnlocalizedName = unlocalizedName;
            DisplayName = displayName;

            RuneTexture = runeTexture;
        }

        public abstract bool UseItem(ModItem item, Player player, TLoZPlayer tlozPlayer);

        public string UnlocalizedName { get; }
        public string DisplayName { get; }

        public Texture2D RuneTexture { get; }
    }
}