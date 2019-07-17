using Microsoft.Xna.Framework;
using Terraria.UI;
using TLoZ.UIs;

namespace TLoZ
{
    public static class UIManager
    {
        public static void Load()
        {
            RuneSelectionUI = new RuneSelectionUI();
            RuneSelectionUI.Activate();

            RuneInterface = new UserInterface();
            RuneInterface.SetState(RuneSelectionUI);

            MiscInputsUI = new MiscInputsUIState();
            MiscInputsUI.Activate();
        }

        public static void UpdateUIs(GameTime gameTime)
        {
            RuneSelectionUI?.Update(gameTime);
            MiscInputsUI?.Update(gameTime);
        }

        public static void Unload()
        {

        }

        public static UserInterface RuneInterface { get; private set; }
        public static RuneSelectionUI RuneSelectionUI { get; private set; }
        public static MiscInputsUIState MiscInputsUI { get; private set; }
    }
}
