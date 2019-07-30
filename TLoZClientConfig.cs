using Microsoft.Xna.Framework;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TLoZ
{
    public class TLoZClientConfig : ModConfig
    {
        [Header("[c/ffff00: General Settings]")]
        [Label("All new characters spawn with Hero Clothes")]
        [DefaultValue(true)]
        public bool spawnWithClothes;

        [Label("Disable In-Game controls tips")]
        [DefaultValue(false)]
        [Tooltip("Will disable some in-game help tooltips( such as one during Rune selection process )")]
        public bool disableTips;

        [Label("Z/L-Target: Switch")]
        [DefaultValue(false)]
        [Tooltip("Checking this setting will make your character continue targeting unless you press Z/L-Target again")]
        public bool switchTarget;

        [Label("Targeting Border Color")]
        public Color targetBorderColor;

        [Label("Targeting Core Color")]
        public Color targetCoreColor;

        public override void OnLoaded()
        {
            TLoZ.loZClientConfig = this;
        }

        public override ConfigScope Mode => ConfigScope.ClientSide;
    }
}
