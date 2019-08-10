using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TLoZ.GlowMasks;
using TLoZ.Players;
using TLoZ.Projectiles;

namespace TLoZ.Items
{
    public class TLoZGlobalItem : GlobalItem
    {
        public GlowMaskData gmd;


        public override void SetDefaults(Item item)
        {
            _defaultUseTurn = item.useTurn;
        }
        public override bool CanUseItem(Item item, Player player)
        {
            TLoZPlayer tlozPlayer = TLoZPlayer.Get(player);
            if (tlozPlayer.myTarget != null && item.useStyle == 1 && item.melee && !item.noUseGraphic)
            {
                player.velocity += Helpers.DirectToPosition(player.Center, tlozPlayer.myTarget.Center, 4f);
                item.useTurn = false;
            }
            else
                item.useTurn = _defaultUseTurn;

            if (tlozPlayer.HasBomb || tlozPlayer.itemUseDelay > 0) return false;

            if (tlozPlayer.Paragliding)
                tlozPlayer.Paragliding = false;
            return base.CanUseItem(item, player);
        }
        public override bool UseItem(Item item, Player player)
        {
            if (item.type == ItemID.PumpkinPie && player.itemAnimation >= item.useAnimation - 1)
                TLoZPlayer.Get(player).BonusStamina += 50;
            return base.UseItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if(item.type == ItemID.PumpkinPie)
            {
                tooltips.Add(BonusStaminaValue(50));
            }
        }
        public override float UseTimeMultiplier(Item item, Player player)
        {
            return TLoZPlayer.Get(player).exhausted ? 0.5f : 1f;
        }
        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            foreach (Projectile proj in Main.projectile)
            {
				if(proj == null )
					continue;
                if (!proj.active)
                    continue;

                TLoZGlobalProjectile tlozPlayer = TLoZGlobalProjectile.GetFor(proj);

                if (hitbox.Intersects(proj.Hitbox) && tlozPlayer.Stasised && tlozPlayer.cantGetHitTimer <= 0)
                {
                    Main.PlaySound(21);

                    tlozPlayer.cantGetHitTimer = 20;
                    tlozPlayer.stasisLaunchDirection = Helpers.DirectToMouse(proj.Center);
                    tlozPlayer.stasisLaunchSpeed += item.knockBack * 0.5f;
                }
            }
        }

        private TooltipLine BonusStaminaValue(int value)
        {
            return new TooltipLine(mod, "BonusStaminaLine", "Provides [c/ffff00:" + value.ToString() + "] bonus stamina when eaten.");
        }

        private bool _defaultUseTurn; 

        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;
    }
}
