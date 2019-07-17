using Terraria;
using Terraria.UI;

namespace TLoZ.UIs
{
    public class MiscInputsLayer : GameInterfaceLayer
    {
        public MiscInputsLayer(MiscInputsUIState miscInputsUI) : base("MiscInputsLayer", InterfaceScaleType.UI)
        {
            MiscInputsUIState = miscInputsUI;
        }

        protected override bool DrawSelf()
        {
            MiscInputsUIState?.Draw(Main.spriteBatch);

            return true;
        }

        private MiscInputsUIState MiscInputsUIState { get; }
    }
}