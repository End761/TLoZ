using Terraria.UI;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TLoZ.UIs;
using TLoZ.Runes;
using Terraria.ID;

namespace TLoZ
{
    public class TLoZ : Mod
    {
        internal static TLoZClientConfig loZClientConfig;
        public TLoZ()
        {
            Instance = this;
        }
        public List<int> MagnesisWhiteList;
        public override void Load()
        {
            FillMagnesisList();
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
            MagnesisWhiteList.Clear();
            StasisableProjectiles.Unload();
            TLoZInput.Unload();
            TLoZTextures.Unload();
            TLoZDialogues.Unload();
            MagnesisWhiteList = null;
            Instance = null;
            loZClientConfig = null;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Insert(0, new RuneSelectionLayer(UIManager.RuneSelectionUI));
            layers.Insert(1, new MiscInputsLayer(UIManager.MiscInputsUI));
        }
        public void FillMagnesisList()
        {
            MagnesisWhiteList = new List<int>()
            {
                TileID.Lead,
                TileID.Iron,
                TileID.Copper,
                TileID.Tin,
                TileID.Silver,
                TileID.Tungsten
            };
        }
        public static TLoZ Instance { get; private set; }
    }
}