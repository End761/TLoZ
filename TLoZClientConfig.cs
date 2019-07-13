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
        public static bool shouldSpawnWithClothes;

        public override void OnChanged()
        {
            shouldSpawnWithClothes = spawnWithClothes;
        }

        public override ConfigScope Mode => ConfigScope.ClientSide;
    }
}
