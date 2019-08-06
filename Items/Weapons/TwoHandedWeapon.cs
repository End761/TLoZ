using Microsoft.Xna.Framework;
using Terraria;
using TLoZ.Players;
using TLoZ.Projectiles.Misc;

namespace TLoZ.Items.Weapons
{
    public abstract class TwoHandedWeapon : TLoZItem
    {
        protected TwoHandedWeapon(string displayName, string tooltip, int width, int height, int value = 0, int defense = 0, int rarity = 0) : base(displayName, tooltip, width, height, value, defense, rarity)
        {
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.noUseGraphic = true;
            item.useAnimation = 50;
            item.noMelee = true;
            item.knockBack = 4.5f;
        }
        public override bool CanUseItem(Player player)
        {
            TLoZPlayer zPlayer = TLoZPlayer.Get(player);
            if (!zPlayer.isSwinging && !zPlayer.downwardsSlash && !zPlayer.twoHanderChargeAttack)
            {
                zPlayer.isSwinging = zPlayer.swingRotation == 0.0f ? true : zPlayer.isSwinging;

                zPlayer.windedBack = zPlayer.swingRotation >= 4.25f? false : zPlayer.windedBack;
                zPlayer.downwardsSlash = zPlayer.swingRotation >= 4.25f ? true : zPlayer.downwardsSlash;
            }
            return false;
        }
        public virtual void OnHitEffects(NPC target, Player player)
        {

        }
        public virtual void HitboxEffects(Projectile projectile, Player player)
        {

        }
        public virtual void DrawEffects(Player player)
        {

        }
    }
}
