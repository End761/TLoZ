using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TLoZ.NPCs;

namespace TLoZ.Buffs
{
    public class StasisDebuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Stasis!");
            Description.SetDefault("You are encased in stasis!");
            Main.debuff[Type] = true;
            canBeCleared = false;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            TLoZNpcs.GetFor(npc).Stasised = true;
            TLoZNpcs.GetFor(npc).PostStasisFlyTimer = 3;
        }
    }
}
