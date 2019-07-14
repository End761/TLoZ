using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using TLoZ.Players;
using TLoZ.Runes;

namespace TLoZ.Projectiles.Runes
{
    public abstract class BombBase : TLoZProjectile
    {
        private int _existanceTimer;
        public override void SetDefaults()
        {
            _existanceTimer = 0;
            projectile.hostile = true;
            projectile.friendly = true;
            projectile.width = 20;
            projectile.height = 20;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }
        public override bool PreAI()
        {
            if (projectile.ai[0] == 0)
                projectile.timeLeft = 4;

            return true;
        }
        public override void AI()
        {
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;

            if (_existanceTimer < 15)
                _existanceTimer++;

            int dusto = Dust.NewDust(projectile.Center - new Vector2(2, 18).RotatedBy(projectile.rotation), 0, 0, DustID.AncientLight, 0, 0, 0, default(Color), 2f);

            Main.dust[dusto].noGravity = true;
            Main.dust[dusto].velocity *= 0;

            if ((!Owner.active || Owner.dead) && projectile.ai[0] == 0)
                projectile.Kill();

            if (projectile.ai[1] == 0 && Owner.whoAmI == Main.myPlayer)
            {
                Owner.heldProj = projectile.whoAmI;
                TLoZPlayer.HasBomb = true;
                TLoZPlayer.itemUseDelay = 15;
                projectile.Center = new Vector2((int)Owner.position.X, (int)Owner.position.Y) + new Vector2(10, -8);

                if (Owner.controlUseItem && Owner.itemAnimation == 0 && _existanceTimer >= 15)
                {
                    projectile.velocity = Owner.velocity.X != 0 && !Owner.controlDown? new Vector2(8 * Owner.direction, -8) : Vector2.Zero;
                    projectile.ai[1] = 1;

                    TLoZPlayer.HasBomb = false;
                }
            }
            if (projectile.ai[1] >= 1)
                projectile.velocity.Y += 0.3f;

            if (projectile.timeLeft == 2)
            {
                Dusts();
                projectile.width = 200;
                projectile.height = 200;
                projectile.Center -= new Vector2(100, 100);
                Main.PlaySound(SoundID.Item14);
            }

            if (Main.myPlayer == Owner.whoAmI && Owner.controlUseTile)
            {
                if (projectile.Hitbox.Contains(Main.MouseWorld.ToPoint()) && Vector2.Distance(projectile.Center, Owner.Center) <= 16 * 5)
                {
                    projectile.ai[0] = 0;
                    projectile.ai[1] = 0;
                }
                else if(Vector2.Distance(projectile.Center, Owner.Center) > 16 * 5 && RequiredRune)
                {
                    projectile.damage = 200;
                    projectile.ai[0] = 1;
                    projectile.ai[1] = 2;
                }
            }

            Lighting.AddLight(projectile.Center, new Vector3(0.0f, 1.4f, 1.4f));
            projectile.velocity = Collision.TileCollision(projectile.position, projectile.velocity, 20, 20, false, false, 1);
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            damage = (int)(damage * 0.25f);
        }

        private void Dusts()
        {
            Helpers.CircleDust(projectile.Center, Vector2.Zero, DustID.AncientLight, 140, 140, 2.5f, 60);
            for(int i = 0; i < 80; i++)
            {
                int dust = Dust.NewDust(projectile.Center, 0, 0, DustID.AncientLight, 0, 0);
                Main.dust[dust].velocity *= 17.5f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 3.5f;
            }
            // Large Smoke Gore spawn
            for (int g = 0; g < 2; g++)
            {
                int goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 3.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 3.5f;

                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 3.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 3.5f;

                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 3.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 3.5f;

                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 3.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 3.5f;
            }
        }

        // I can't bother to deal w/ bullshit that is vanilla drawing so I do my own
        // Comes at a bonus of not being affected by light;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, new Vector2(12, 22), 1f, SpriteEffects.None, 1f);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return projectile.ai[0] == 1 && Vector2.Distance(projHitbox.Center(), targetHitbox.Center()) <= 16 * 9.5f;
        }

        public Player Owner => Main.player[projectile.owner];
        public TLoZPlayer TLoZPlayer => TLoZPlayer.Get(Owner);
        public virtual bool RequiredRune => TLoZPlayer.SelectedRune is BombRoundRune;
    }
}
