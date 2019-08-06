using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using TLoZ.NPCs.Minibosses.Guardian;

namespace TLoZ.Projectiles.Hostile
{
    public class GuardianLaser : TLoZProjectile
    {
        public override void SetDefaults()
        {
            projectile.hostile = true;
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
            if(projectile.damage > 0)
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType<GuardianLaserExplosion>(), 60, 4f, Main.myPlayer);

            projectile.damage = 0;

            return false;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage *= 4;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.damage = 0;
            projectile.velocity = Vector2.Zero;
            Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType<GuardianLaserExplosion>(), 60, 4f, Main.myPlayer);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if(target.type != mod.NPCType<Guardian>())
            {
                projectile.damage = 0;
                projectile.velocity = Vector2.Zero;
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType<GuardianLaserExplosion>(), 60, 4f, Main.myPlayer);
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            return target.type != mod.NPCType<Guardian>();
        }
        public override void AI()
        {
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
            Helpers.DrawLine(Main.npc[(int)projectile.ai[0]].Center - new Vector2(0, 48), projectile.Center, Main.projectileTexture[projectile.type], Main.projectileTexture[projectile.type], spriteBatch, Color.Cyan, projectile.ai[1]);
        }
    }
}
