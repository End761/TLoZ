using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TLoZ.Notes;
using TLoZ.Players;

namespace TLoZ.Songs
{
    public class EponasSong : Song
    {
        public EponasSong() : base("eponasSong",
            new TimeSpan(0, 0, 0, 0, 864), TimeSpan.Zero,
            new NoteUp(), new NoteLeft(), new NoteRight(), new NoteUp(), new NoteLeft(), new NoteRight())
        {
        }


        public override void PostPlay(TLoZPlayer tlozPlayer, SongVariant variant)
        {
            Mount mount = new Mount();

            if (!mount.CanMount(MountID.Unicorn, tlozPlayer.player))
                return;

            mount.SetMount(MountID.Unicorn, tlozPlayer.player);
            //tlozPlayer.player.mount = mount;
        }


        public override string DisplayName { get; } = "Epona's Song";

        public override string NormalSongPath { get; } = "Sounds/Music/Songs/bruh";
    }
}