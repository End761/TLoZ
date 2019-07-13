using Terraria;
using Terraria.ModLoader;
using TLoZ.NPCs;

namespace TLoZ.Buffs
{
    public sealed class StasisDebuff : TLoZBuff
    {
        public StasisDebuff() : base("Stasis!", "You are encased in stasis!")
        {
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Main.debuff[Type] = true;
            canBeCleared = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            LoZnpCs.GetFor(npc).stasised = true;
            LoZnpCs.GetFor(npc).postStasisFlyTimer = 3;
        }
    }
}
