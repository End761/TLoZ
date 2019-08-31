using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using TLoZ.Items.Weapons;
using TLoZ.Items.Weapons.Melee;

namespace TLoZ.Projectiles.Misc
{
    public class TwoHandedWeaponHitbox : TLoZProjectile
    {
        private TwoHandedWeapon Weapon => Owner.HeldItem.modItem as TwoHandedWeapon;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
        }

        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.width = 100;
            projectile.height = 120;
            projectile.friendly = true;
            projectile.penetrate = -1;
        }

        public override string Texture => "TLoZ/Textures/Misc/EmptyPixel";

        public override void AI()
        {
            if (!Main.dedServ)
            {
                Texture2D texture = Main.itemTexture[Owner.HeldItem.type];

                if (TLoZPlayer.TwoHanderChargeAttack)
                {
                    projectile.width = (int)(texture.Width * 0.75f + texture.Height * 0.75f);
                    projectile.height = (int)(texture.Width * 0.75f + texture.Height * 0.75f);
                }

                else
                    projectile.width = (int)((texture.Width * 0.5f + texture.Height * 0.5f) * 2f);


                if (TLoZPlayer.TwoHanderChargeAttack)
                    projectile.Center = Owner.Center - new Vector2(45, -14 * Owner.direction * -1).RotatedBy((TLoZPlayer.SwingRotation + (Owner.direction == -1 ? MathHelper.Pi * 1.33f : MathHelper.Pi * 0.33f)) * Owner.direction * -1);
                else
                    projectile.Center = Owner.Center + new Vector2(texture.Width * .75f * Owner.direction, TLoZPlayer.DownwardsSlash ? 0 : -20);

            }
            projectile.timeLeft = 2;
            projectile.velocity = Owner.direction == -1 ? new Vector2(-0.5f, -0.5f) : new Vector2(0.5f, -0.5f);

            Weapon?.HitboxEffects(projectile, Owner);

            if (!TLoZPlayer.IsSwinging && !TLoZPlayer.DownwardsSlash && !TLoZPlayer.TwoHanderChargeAttack)
                projectile.Kill();

            if (TLoZPlayer.DownwardsSlash)
                projectile.height = 140;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = TLoZPlayer.LeftClickTimer >= 160 ? 7 : 24;
            Weapon?.OnHitEffects(target, Owner);
        }
    }
}
