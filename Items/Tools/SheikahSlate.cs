using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using TLoZ.Buffs;
using TLoZ.Projectiles;
using Microsoft.Xna.Framework;
using TLoZ.Projectiles.Runes;
using TLoZ.Enums;

namespace TLoZ.Items.Tools
{
    public class SheikahSlate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sheikah Slate");
            Tooltip.SetDefault("Ancient Sheikah technology created this very object." +
                "\nCapable of using Sheikah Runes");
        }
        public override void SetDefaults()
        {
            item.rare = 4;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.width = 40;
            item.height = 40;
            item.melee = true;
            item.autoReuse = true;
            item.useTurn = true;
            item.noUseGraphic = true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            var tlozPlayer = TLoZPlayer.Get(player);
            if (tlozPlayer.SelectedRune == (int)RuneID.BombRound)
            {
                if (!tlozPlayer.HasBomb && player.ownedProjectileCounts[mod.ProjectileType<BombRound>()] <= 0 && tlozPlayer.SelectedRune == (int)RuneID.BombRound)
                {
                    Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType<BombRound>(), 0, 0, player.whoAmI);
                }
                else if (player.ownedProjectileCounts[mod.ProjectileType<BombRound>()] > 0)
                {
                    player.itemAnimation = 0;
                    return false;
                }
            }
            return base.CanUseItem(player);
        }
        public override bool UseItem(Player player)
        {
            var tlozPlayer = TLoZPlayer.Get(player);
            if (tlozPlayer.SelectedRune == (int)RuneID.Stasis)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (!npc.active)
                        continue;
                    if (Collision.CheckAABBvLineCollision(npc.position, npc.Hitbox.Size(), player.Center, Main.MouseWorld))
                    {
                        npc.AddBuff(mod.BuffType<StasisDebuff>(), 420);
                        break;
                    }
                }
                foreach (Projectile projectile in Main.projectile)
                {
                    if (!projectile.active || !TLoZProjectiles.GetFor(projectile).CanBeStasised)
                        continue;
                    if (Collision.CheckAABBvLineCollision(projectile.position, projectile.Hitbox.Size(), player.Center, Main.MouseWorld))
                    {
                        TLoZProjectiles.GetFor(projectile).StasisTimer = 420;
                        break;
                    }
                }
            }
            return base.UseItem(player);
        }
    }
}
