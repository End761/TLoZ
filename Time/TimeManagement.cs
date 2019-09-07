using System.Collections.Generic;
using On.Terraria;
using TLoZ.Time.States;

namespace TLoZ.Time
{
    public static class TimeManagement
    {
        internal static Dictionary<long, TileState[]> tileStatesPerTime;
        internal static Dictionary<long, NPCState[]> npcStatesPerTime;


        public static void Load()
        {
            tileStatesPerTime = new Dictionary<long, TileState[]>();

            
        }

        public static void Unload()
        {
            tileStatesPerTime.Clear();
            tileStatesPerTime = null;
        }


        public static void Rewind()
        {

        }
    }
}