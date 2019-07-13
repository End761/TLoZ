using Terraria.ModLoader;

namespace TLoZ.Buffs
{
    // Taken from Dragon Ball Terraria by Webmilio.
    public class TLoZBuff : ModBuff
    {
        private readonly string _displayName, _tooltip;

        protected TLoZBuff(string displayName, string tooltip)
        {
            _displayName = displayName;
            _tooltip = tooltip;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            DisplayName.SetDefault(_displayName);
            Description.SetDefault(_tooltip);
        }
    }
}