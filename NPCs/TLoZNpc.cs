using Terraria;
using Terraria.ModLoader;

namespace TLoZ.NPCs
{
    public abstract class TLoZNpc : ModNPC
    {
        public TLoZNpc(string name, bool hostile, int maxHP, int width = 16, int height = 16, bool boss = false)
        {
            NPCName = name;
            IsHostile = hostile;
            IsBoss = boss;
            MaxHP = maxHP;
            Width = width;
            Height = height;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(NPCName);
        }
        public override void SetDefaults()
        {
            npc.width = Width;
            npc.height = Height;
            npc.life = npc.lifeMax = MaxHP;
            npc.boss = IsBoss;
            npc.friendly = !IsHostile;
        }
        private string NPCName { get; set; }
        private bool IsHostile { get; set; }
        private bool IsBoss { get; set; }
        private int MaxHP { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }

        public Player Target => Main.player[npc.target];
    }
}
