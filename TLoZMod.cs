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
using TLoZ.Notes;
using TLoZ.Players;
using TLoZ.NPCs.Minibosses.Guardian;
using TLoZ.Songs;
using TLoZ.Time;
using TLoZ.Worlds;
using WebmilioCommons.Networking;

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
            NetworkPacketLoader.Instance.HandlePacket(reader, whoAmI);
        }


        public override void Load()
        {
            FillMagnesisList();

            StasisableProjectiles.Load();
            TimeManagement.Load();

            NoteLoader.Load();

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

            TLoZWorld tlozWorld = ModContent.GetInstance<TLoZWorld>();
            WorldSong song = tlozWorld.CurrentSong;

            if (song != null)
            {
                if (song.Variant == SongVariant.Normal && song.Player.player == Main.LocalPlayer)
                    Main.blockInput = true;

                music = GetSoundSlot(SoundType.Music, song.Path);
                priority = MusicPriority.Event;

                if (tlozWorld.TicksLeftOnSong > 0)
                    tlozWorld.TicksLeftOnSong--;

                if (tlozWorld.TicksLeftOnSong == 0)
                    tlozWorld.ResetSong();
            }
            foreach (NPC npc in Main.npc)
            {
                if (npc.type != ModContent.NPCType<Guardian>() || !npc.active)
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