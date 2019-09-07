using System;
using Terraria;
using Terraria.ModLoader;
using TLoZ.Notes;
using TLoZ.Players;

namespace TLoZ.Songs
{
    public class SongOfHealing : Song
    {
        public SongOfHealing() : base("songOfHealing", 
            new TimeSpan(0, 0, 0, 06, 826), new TimeSpan(0, 0, 1, 21, 920),
            new NoteLeft(), new NoteRight(), new NoteDown(), new NoteLeft(), new NoteRight(), new NoteDown())
        {
        }


        public override void OnPlay(TLoZPlayer tlozPlayer, SongVariant variant)
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];

                if (!player.active || player.dead)
                    return;

                for (int j = 0; j < player.CountBuffs(); j++)
                {
                    int buffType = player.buffType[j];

                    if (Main.debuff[buffType] && BuffLoader.CanBeCleared(buffType))
                        player.ClearBuff(buffType);
                }
            }
        }
    }
}