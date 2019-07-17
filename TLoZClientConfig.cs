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

        public override void OnLoaded()
        {
            TLoZ.loZClientConfig = this;
        }

        public override ConfigScope Mode => ConfigScope.ClientSide;
    }
}
