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
using TLoZ.Network;
using TLoZ.Players;
using TLoZ.NPCs.Minibosses.Guardian;
using TLoZ.Time;

namespace TLoZ
{
    public class TLoZMod : Mod
    {
        internal static TLoZClientConfig loZClientConfig;

        public TLoZMod()
        {
            Instance = this;
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            NetworkPacketManager.Instance.HandlePacket(reader, whoAmI);
        }


        public override void Load()
        {
            FillMagnesisList();

            StasisableProjectiles.Load();
            TimeManagement.Load();

            if (!Main.dedServ)
            {
                TLoZInput.Load(Instance);
                TLoZTextures.Load();
                UIManager.Load();
                TLoZDialogues.Load();
            }
        }

        public override void Unload()
        {
            MagnesisRune.magnesisWhiteList.Clear();

            RuneManager.Instance.Unload();

            TimeManagement.Unload();

            StasisableProjectiles.Unload();
            TLoZInput.Unload();
            TLoZTextures.Unload();
            TLoZDialogues.Unload();

            MagnesisRune.magnesisWhiteList = null;

            Instance = null;
            loZClientConfig = null;
        }


        public override void UpdateUI(GameTime gameTime)
        {
            UIManager.UpdateUIs(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Insert(0, new RuneSelectionLayer(UIManager.RuneSelectionUI));
            layers.Insert(1, new MiscInputsLayer(UIManager.MiscInputsUI));

            layers.Add(new StaminaLayer(UIManager.StaminaUI));

            layers.Add(new InstrumentPlayLayer(UIManager.InstrumentPlayUIState));
        }


        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
            {
                return;
            }
            foreach (NPC npc in Main.npc)
            {
                if (npc.type != NPCType<Guardian>() || !npc.active)
                    continue;

                Guardian guardian = npc.modNPC as Guardian;
                if (guardian != null && guardian.IsGuardianActive)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/GuardianTheme");
                    priority = MusicPriority.BossMedium;
                }
            }
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

        public static TLoZMod Instance { get; private set; }
    }
}