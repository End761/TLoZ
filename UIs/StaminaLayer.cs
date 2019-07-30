using Terraria;
using Terraria.UI;

namespace TLoZ.UIs
{
    public class StaminaLayer : GameInterfaceLayer
    {
        public StaminaLayer(StaminaUIState uiState) : base("StaminaLayer", InterfaceScaleType.UI)
        {
            State = uiState;
        }

        protected override bool DrawSelf()
        {
            State?.Draw(Main.spriteBatch);

            return true;
        }

        private StaminaUIState State { get; }
    }
}
