using Terraria.UI;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TLoZ.UIs;

namespace TLoZ
{
    public class LoZ : Mod
    {
        public LoZ()
        {
        }

        public override void Load()
        {
            Instance = this;

            if (!Main.dedServ)
            {
                TLoZInput.Load(Instance);
                TLoZTextures.Load();
                UIManager.Load();
            }

            TLoZDialogues.Load();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            UIManager.UpdateUIs(gameTime);
        }

        public override void Unload()
        {
            Instance = null;

            TLoZInput.Unload();
            TLoZTextures.Unload();
            TLoZDialogues.Unload();
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Insert(0, new RuneSelectionLayer(UIManager.RuneSelectionUI));
        }

        public static LoZ Instance { get; private set; }
    }
}