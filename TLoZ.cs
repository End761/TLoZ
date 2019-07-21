using Terraria.UI;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TLoZ.UIs;
using TLoZ.Runes;

namespace TLoZ
{
    public class TLoZ : Mod
    {
        internal static TLoZClientConfig loZClientConfig;
        public TLoZ()
        {
            Instance = this;
        }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                TLoZInput.Load(Instance);
                TLoZTextures.Load();
                UIManager.Load();
                StasisableProjectiles.Load();
                TLoZDialogues.Load();
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            UIManager.UpdateUIs(gameTime);
        }

        public override void Unload()
        {
            StasisableProjectiles.Unload();
            TLoZInput.Unload();
            TLoZTextures.Unload();
            TLoZDialogues.Unload();
            Instance = null;
            loZClientConfig = null;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Insert(0, new RuneSelectionLayer(UIManager.RuneSelectionUI));
            layers.Insert(1, new MiscInputsLayer(UIManager.MiscInputsUI));
        }

        public static TLoZ Instance { get; private set; }
    }
}