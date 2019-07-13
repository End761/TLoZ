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
        }

        public static void UpdateUIs(GameTime gameTime)
        {
            RuneSelectionUI?.Update(gameTime);
        }

        public static void Unload()
        {

        }

        public static UserInterface RuneInterface { get; private set; }
        public static RuneSelectionUI RuneSelectionUI { get; private set; }
    }
}
