using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using TLoZ.GlowMasks;
using TLoZ.Players;
using TLoZ.Projectiles;

namespace TLoZ.Items
{
    public class TLoZGlobalItem : GlobalItem
    {
        public GlowMaskData gmd;
        
        public override bool HoldItemFrame(Item item, Player player) => false;

        public override bool GrabStyle(Item item, Player player) => false;

        public override bool CanUseItem(Item item, Player player)
        {
            TLoZPlayer tlozPlayer = TLoZPlayer.Get(player);

            if (tlozPlayer.HasBomb || tlozPlayer.usesParaglider || tlozPlayer.itemUseDelay > 0) return false;

            return base.CanUseItem(item, player);
        }

        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            foreach(Projectile proj in Main.projectile)
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

        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;
    }
}
