using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TLoZ.Players;
using TLoZ.Worlds;

namespace TLoZ.Items.Tools.Misc
{
    public class OcarinaOfTime : TLoZItem
    {
        public OcarinaOfTime() : base("Ocarina of Time", "Possess magical powers", 40, 40, Item.buyPrice(0, 10, 0, 0), 0, ItemRarityID.Quest)
        {
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.useStyle = 3;
            item.useAnimation = 15;
            item.useTime = 15;
        }


        public override bool CanUseItem(Player player) => !ModContent.GetInstance<TLoZWorld>().IsSongPlaying;

        public override bool UseItem(Player player)
        {
            TLoZPlayer tlozPlayer = TLoZPlayer.Get(player);
            tlozPlayer.IsPlayingInstrument = !tlozPlayer.IsPlayingInstrument;

            if (Main.LocalPlayer == player)
                UIManager.InstrumentPlayUIState.Visible = !UIManager.InstrumentPlayUIState.Visible;

            return true;
        }
    }
}
