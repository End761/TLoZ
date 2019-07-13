using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TLoZ.GlowMasks;
using TLoZ.Projectiles;

namespace TLoZ
{
    public class TLoZItems : GlobalItem
    {
        public GlowMaskData GMD;
        public override void SetDefaults(Item item)
        {
            base.SetDefaults(item);
        }
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }
        public override bool HoldItemFrame(Item item, Player player)
        {
            return false;
        }
        public override bool GrabStyle(Item item, Player player)
        {
            return false;
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (TLoZPlayer.Get(player).HasBomb) return false;
            if (TLoZPlayer.Get(player).UsesParaglider) return false;
            return base.CanUseItem(item, player);
        }
        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            foreach(Projectile proj in Main.projectile)
            {
                if (!proj.active)
                    continue;
                TLoZProjectiles zproj = TLoZProjectiles.GetFor(proj);
                if(hitbox.Intersects(proj.Hitbox) && zproj.Stasised && zproj.CantGetHitTimer <= 0)
                {
                    Main.PlaySound(21);
                    zproj.CantGetHitTimer = 20;
                    zproj.StasisLaunchDirection = Helpers.DirectToMouse(proj.Center);
                    zproj.StasisLaunchSpeed += item.knockBack * 0.5f;
                }
            }
        }
    }
}
