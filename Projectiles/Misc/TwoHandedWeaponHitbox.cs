using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using TLoZ.Items.Weapons;
using TLoZ.Items.Weapons.NoGroup;

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
            Texture2D texture = Main.itemTexture[Owner.HeldItem.type];

            projectile.timeLeft = 2;
            projectile.velocity = Owner.direction == -1 ? new Vector2(-0.5f, -0.5f) : new Vector2(0.5f, -0.5f);

            if (!TLoZPlayer.isSwinging && !TLoZPlayer.downwardsSlash && !TLoZPlayer.twoHanderChargeAttack)
                projectile.Kill();

            if (TLoZPlayer.twoHanderChargeAttack)
            {
                projectile.width = (int)(texture.Width * 0.75f + texture.Height * 0.75f);
                projectile.height = (int)(texture.Width * 0.75f + texture.Height * 0.75f);
            }
            else
                projectile.width = (int)((texture.Width * 0.5f + texture.Height * 0.5f) * 2f);

            if (TLoZPlayer.twoHanderChargeAttack)
                projectile.Center = Owner.Center - new Vector2(45, -14 * Owner.direction * -1).RotatedBy((TLoZPlayer.swingRotation + (Owner.direction == -1 ? MathHelper.Pi * 1.33f : MathHelper.Pi * 0.33f)) * Owner.direction * -1);
            else
                projectile.Center = Owner.Center + new Vector2(texture.Width * .75f * Owner.direction, TLoZPlayer.downwardsSlash ? 0 : -20);

            if (TLoZPlayer.downwardsSlash)
                projectile.height = 140;
            Weapon?.HitboxEffects(projectile, Owner);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = TLoZPlayer.leftClickTimer >= 160 ? 7 : 24;
            Weapon?.OnHitEffects(target, Owner);
        }
    }
}
