using Terraria;

namespace TLoZ.Time.States
{
    public class NPCState : EntityState<NPC>
    {
        public NPCState(NPC npc) : base(npc)
        {
            Damage = npc.damage;
            KnockbackResist = npc.knockBackResist;

            AI = npc.ai;
        }


        public override void Restore()
        {
            base.Restore();

            Entity.damage = Damage;
            Entity.knockBackResist = KnockbackResist;

            Entity.ai = AI;
        }


        public int Damage { get; set; }
        public float KnockbackResist { get; set; }

        public float[] AI { get; set; }
    }
}