using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using TLoZ.NPCs.Minibosses.Guardian;

namespace TLoZ.Projectiles.Misc
{
    public class DeflectedLaser : TLoZProjectile
    {
        public override void SetDefaults()
        {
            projectile.timeLeft = 600;
            projectile.width = 16;
            projectile.penetrate = -1;
            projectile.height = 16;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.extraUpdates = 4;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            projectile.position += oldVelocity;
            if (projectile.damage > 0)
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<DeflectedLaserExplosion>(), 60, 4f, Main.myPlayer);

            projectile.damage = 0;

            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage *= 4;
            if (target.type == ModContent.NPCType<Guardian>())
            {
                crit = true;
                damage = target.lifeMax;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.Center += projectile.velocity;
            projectile.damage = 0;
            projectile.velocity = Vector2.Zero;
            Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<DeflectedLaserExplosion>(), 60, 4f, Main.myPlayer);
        }

        public override void AI()
        {
            projectile.netUpdate = true;
            if(projectile.timeLeft >= 599)
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/GuardianLaserSpawn"));
            Lighting.AddLight(projectile.position, Color.Cyan.ToVector3());
            if (projectile.timeLeft <= 510)
                projectile.ai[1] *= 0.95f;
            else
                projectile.ai[1] += 0.1f;
            if (projectile.ai[1] <= 0.02f)
                projectile.Kill();

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Helpers.DrawLine(Owner.Center + new Vector2(6 * Owner.direction, 0), projectile.Center, Main.projectileTexture[projectile.type], Main.projectileTexture[projectile.type], spriteBatch, Color.DarkCyan * 0.2f, projectile.ai[1] * 2.2f);
            Helpers.DrawLine(Owner.Center + new Vector2(6 * Owner.direction, 0), projectile.Center, Main.projectileTexture[projectile.type], Main.projectileTexture[projectile.type], spriteBatch, Color.DarkCyan * 0.5f, projectile.ai[1] * 1.35f);
            Helpers.DrawLine(Owner.Center + new Vector2(6 * Owner.direction, 0), projectile.Center, Main.projectileTexture[projectile.type], Main.projectileTexture[projectile.type], spriteBatch, Color.White * 0.7f, projectile.ai[1] * 0.5f);
        }
    }
}
