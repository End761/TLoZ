using Terraria.UI;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TLoZ
{
    public class TLoZ : Mod
	{
        public static TLoZ instance;

        public TLoZ()
		{
		}
        public override void Load()
        {
            instance = this;
            if (!Main.dedServ)
            {
                TLoZInput.Load(instance);
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
            instance = null;
            TLoZInput.Unload();
            TLoZTextures.Unload();
            TLoZDialogues.Unload();
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Insert(0, new LegacyGameInterfaceLayer("RuneSelectionLayer",
                delegate
                {
                    if (UIManager.RuneSelection != null)
                        UIManager.RuneSelection.Draw(Main.spriteBatch);
                    return true;
                }
                )
                );
        }
    }
}