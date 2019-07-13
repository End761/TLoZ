using Terraria;
using Terraria.ModLoader;
using TLoZ.Buffs;
using TLoZ.Players;
using TLoZ.Projectiles;

namespace TLoZ.Runes
{
    public sealed class StasisRune : Rune
    {
        public StasisRune() : base("stasisRune", "Stasis", TLoZTextures.UIStasisRune)
        {
        }


        public override bool UseItem(ModItem item, Player player, TLoZPlayer tlozPlayer)
        {
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active) continue;

                if (Collision.CheckAABBvLineCollision(npc.position, npc.Hitbox.Size(), player.Center, Main.MouseWorld))
                {
                    npc.AddBuff(item.mod.BuffType<StasisDebuff>(), 420);
                    break;
                }
            }

            foreach (Projectile projectile in Main.projectile)
            {
                if (!projectile.active || !TLoZProjectiles.GetFor(projectile).canBeStasised)
                    continue;

                if (Collision.CheckAABBvLineCollision(projectile.position, projectile.Hitbox.Size(), player.Center, Main.MouseWorld))
                {
                    TLoZProjectiles.GetFor(projectile).stasisTimer = 420;
                    break;
                }
            }

            return true;
        }
    }
}