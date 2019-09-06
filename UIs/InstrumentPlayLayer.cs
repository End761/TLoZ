using Terraria;
using Terraria.UI;

namespace TLoZ.UIs
{
    public class InstrumentPlayLayer : GameInterfaceLayer
    {
        public InstrumentPlayLayer(InstrumentPlayUIState uiState) : base("InstrumentPlayLayer", InterfaceScaleType.UI)
        {
            State = uiState;
        }

        protected override bool DrawSelf()
        {
            State?.Draw(Main.spriteBatch);

            return true;
        }

        private InstrumentPlayUIState State { get; }
    }
}
