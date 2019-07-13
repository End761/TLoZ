using Terraria;
using Terraria.UI;

namespace TLoZ.UIs
{
    public class RuneSelectionLayer : GameInterfaceLayer
    {
        public RuneSelectionLayer(RuneSelectionUI runeSelectionUI) : base("RuneSelectionLayer", InterfaceScaleType.UI)
        {
            RuneSelectionUI = runeSelectionUI;
        }

        protected override bool DrawSelf()
        {
            RuneSelectionUI?.Draw(Main.spriteBatch);

            return true;
        }

        private RuneSelectionUI RuneSelectionUI { get; }
    }
}