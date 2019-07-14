using Terraria.ID;
using Terraria;
using TLoZ.Players;

namespace TLoZ.Items.Tools
{
    public class SheikahSlate : TLoZItem
    {
        public SheikahSlate() : base("Sheikah Slate", "Ancient Sheikah technology created this very object.\nCapable of using Sheikah Runes", 40, 40, 0, 0, ItemRarityID.LightRed)
        {
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.melee = true;
            item.autoReuse = true;
            item.useTurn = true;
            item.noUseGraphic = true;
        }

        public override bool UseItem(Player player)
        {
            TLoZPlayer tlozPlayer = TLoZPlayer.Get(player);

            if (tlozPlayer.SelectedRune == null)
                return base.UseItem(player);

            tlozPlayer.SelectedRune.UseItem(this, player, tlozPlayer);

            return base.UseItem(player);
        }
    }
}
