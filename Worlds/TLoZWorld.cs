using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;
using TLoZ.Players;
using TLoZ.Songs;

namespace TLoZ.Worlds
{
    public class TLoZWorld : ModWorld
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            /*tasks.Add(new PassLegacy("SwordPedestal",
                delegate (GenerationProgress generationProgress)
                {
                    for (int i = 0; i < Main.maxTilesX; i++)
                    {
                        for (int j = 0; j < Main.maxTilesY; j++)
                        {
                            Tile firstTile = Main.tile[i, j];
                            Tile secondTile = Main.tile[i + 1, j];
                            Tile belowFirstTile = Main.tile[i, j + 1];
                            Tile belowSecondTile = Main.tile[i, j + 1];
                            if (firstTile != null && belowFirstTile != null && !firstTile.active() && belowFirstTile.active())
                            {
                                WorldGen.PlaceObject(i, j, mod.TileType<MasterSwordPedestal>());
                            }
                        }
                    }
                }
                )
                );*/
        }

        private int _invertedSongOfTimeTimer;

        public override void PreUpdate()
        {
            if (InvertedSongOfTimeActive && _invertedSongOfTimeTimer % 2 == 0)
            {
                Main.time -= Main.dayRate;
            }

            System.Diagnostics.Debug.WriteLine(Main.time);
            _invertedSongOfTimeTimer = (_invertedSongOfTimeTimer + 1) % 2;
        }


        public void ToggleInvertedSongOfTime() => InvertedSongOfTimeActive = !InvertedSongOfTimeActive;


        public void PlaySong(TLoZPlayer player, Song song, SongVariant variant)
        {
            CurrentSong = new WorldSong(player, song, variant);
            TicksLeftOnSong = song.GetDuration(variant);
        }

        internal void ResetSong()
        {
            CurrentSong.Song.PostPlay(CurrentSong.Player, CurrentSong.Variant);

            if (CurrentSong.Player.player == Main.LocalPlayer)
                Main.blockInput = false;

            CurrentSong = null;
        }


        public WorldSong CurrentSong { get; private set; }
        public bool IsSongPlaying => CurrentSong != null;

        public int TicksLeftOnSong { get; internal set; }

        public bool InvertedSongOfTimeActive { get; internal set; }
    }
}
