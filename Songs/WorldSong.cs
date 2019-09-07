using TLoZ.Players;

namespace TLoZ.Songs
{
    public class WorldSong
    {
        public WorldSong(TLoZPlayer player, Song song, SongVariant variant)
        {
            Player = player;

            Song = song;
            Variant = variant;

            Path = Song.GetPath(Variant);
            Duration = Song.GetDuration(Variant);
        }


        public TLoZPlayer Player { get; }

        public Song Song { get; }

        public SongVariant Variant { get; }

        public string Path { get; }
        public int Duration { get; }
    }
}