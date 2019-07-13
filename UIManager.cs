using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;
using TLoZ.UIs;

namespace TLoZ
{
    public static class UIManager
    {
        public static UserInterface RuneInterface;
        public static RuneSelectionUI RuneSelection;
        public static void Load()
        {
            RuneSelection = new RuneSelectionUI();
            RuneSelection.Activate();
            RuneInterface = new UserInterface();
            RuneInterface.SetState(RuneSelection);
        }
        public static void UpdateUIs(GameTime gameTime)
        {
            if(RuneSelection != null)
                RuneSelection.Update(gameTime);
        }
        public static void Unload()
        {

        }
    }
}
