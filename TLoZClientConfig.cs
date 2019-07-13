using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TLoZ
{
    public class TLoZClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Header("[c/ffff00: General Settings]")]
        [Label("All new characters spawn with Hero Clothes")]
        [DefaultValue(true)]
        public bool SpawnWithClothes;
        public static bool ShouldSpawnWithClothes;
        public override void OnChanged()
        {
            ShouldSpawnWithClothes = SpawnWithClothes;
        }
    }
}
