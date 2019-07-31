using Terraria.UI;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TLoZ.UIs;
using TLoZ.Runes;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using System.IO;
using TLoZ.Enums;
using TLoZ.Players;

namespace TLoZ
{
    public class TLoZ : Mod
    {
        internal static TLoZClientConfig loZClientConfig;
        public TLoZ()
        {
            Instance = this;
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            int type = reader.ReadInt32();
            switch(type)
            {
                case (int)MessageType.Paraglider:
                    Player player = Main.player[(int)reader.ReadInt32()];
                    TLoZPlayer.Get(player).usesParaglider = reader.ReadBoolean();
                    break;
            }
        }
        public override void Load()
        {
            FillMagnesisList();
            StasisableProjectiles.Load();
            if (!Main.dedServ)
            {
                TLoZInput.Load(Instance);
                TLoZTextures.Load();
                UIManager.Load();
                TLoZDialogues.Load();
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            UIManager.UpdateUIs(gameTime);
        }

        public override void Unload()
        {
            MagnesisRune.magnesisWhiteList.Clear();

            RuneManager.Instance.Unload();

            StasisableProjectiles.Unload();
            TLoZInput.Unload();
            TLoZTextures.Unload();
            TLoZDialogues.Unload();

            MagnesisRune.magnesisWhiteList = null;

            Instance = null;
            loZClientConfig = null;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Insert(0, new RuneSelectionLayer(UIManager.RuneSelectionUI));
            layers.Insert(1, new MiscInputsLayer(UIManager.MiscInputsUI));
            layers.Add(new StaminaLayer(UIManager.StaminaUI));
        }

        public void FillMagnesisList()
        {
            MagnesisRune.magnesisWhiteList = new List<int>()
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