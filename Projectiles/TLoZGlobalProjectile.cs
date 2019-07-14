using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TLoZ.NPCs;
using TLoZ.Projectiles.Runes;

namespace TLoZ.Projectiles
{
    public class TLoZGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public static TLoZGlobalProjectile GetFor(Projectile projectile) => projectile.GetGlobalProjectile<TLoZGlobalProjectile>();
        public int stasisTimer;
        public bool Stasised => stasisTimer > 0;
        public bool lastStasisedState;
        public Vector2 stasisLaunchDirection;
        public float stasisLaunchSpeed;
        public float postStasisLaunchTimer;
        public Vector2[] randomStasisPositions;
        public float stasisChainsOpacity;

        public int cantGetHitTimer;
        private bool _isHostile;
        public bool canBeStasised; 
        public override void SetDefaults(Projectile projectile)
        {
            randomStasisPositions = new[] { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero };
            canBeStasised = projectile.type == ProjectileID.Boulder || projectile.type == mod.ProjectileType<BombRound>();
            _isHostile = projectile.hostile;
        }
        public override void PostAI(Projectile projectile)
        {
            if (stasisChainsOpacity > 0.0f) stasisChainsOpacity -= 0.02f;
            lastStasisedState = Stasised;
        }
        public override bool PreAI(Projectile projectile)
        {
            if(Stasised && !lastStasisedState)
            {
                stasisChainsOpacity = 2.0f;
                for (int i = 0; i < 4; i++)
                {
                    float x = Main.rand.Next(-60, 60);
                    float y = Main.rand.Next(-60, 60);
                    randomStasisPositions[i] = projectile.Center + new Vector2(x, y);
                }
            }
            if (stasisTimer > 0)
                stasisTimer--;
            if(stasisTimer == 1)
            {
                Main.PlaySound(13);
            }
            if (cantGetHitTimer > 0)
                cantGetHitTimer--;

            if (postStasisLaunchTimer > 0.0f)
                postStasisLaunchTimer -= 0.1f; 

            if(Stasised)
            {
                projectile.timeLeft++;
                projectile.hostile = false;
                postStasisLaunchTimer = 6.5f;
                foreach (Projectile proj in Main.projectile)
                {
                    if (!proj.active || proj == projectile)
                        continue;
                    if (projectile.Hitbox.Intersects(proj.Hitbox) && cantGetHitTimer <= 0)
                    {
                        cantGetHitTimer = 5;
                        if (proj.penetrate != -1)
                        {
                            cantGetHitTimer = 10;
                            proj.Kill();
                        }
                        Main.PlaySound(21);
                        stasisLaunchDirection = proj.velocity.SafeNormalize(-Vector2.UnitY);
                        stasisLaunchSpeed += proj.knockBack * 0.5f;
                    }
                }
                return false;
            } 
            if (projectile.hostile != _isHostile)
                projectile.hostile = _isHostile;

            if (stasisLaunchDirection * stasisLaunchSpeed != Vector2.Zero)
                projectile.velocity = stasisLaunchDirection * stasisLaunchSpeed;

            stasisLaunchDirection = Vector2.Zero;
            stasisLaunchSpeed = 0.0f;
            return base.PreAI(projectile);
        }
        public override bool ShouldUpdatePosition(Projectile projectile)
        {
            if (Stasised)
                return false;
            return base.ShouldUpdatePosition(projectile);
        }
        public override bool PreDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
        {
            if (!projectile.active)
                return false;
            if (stasisChainsOpacity > 0.0f)
            {
                for (int i = 0; i < randomStasisPositions.Length; i++)
                {
                    TLoZnpCs.DrawStasisChains(spriteBatch, projectile.Center, randomStasisPositions[i], stasisChainsOpacity);
                }
            }
            if (Stasised)
            {
                // Draw the start
                float rotation = stasisLaunchDirection.ToRotation() - (float)Math.PI / 2;
                Color color = stasisLaunchSpeed > 14f ? Color.Red : stasisLaunchSpeed > 7f ? Color.Orange : Color.Yellow;
                lightColor = color;
                spriteBatch.Draw(TLoZTextures.MiscStasisArrow, projectile.Center + (stasisLaunchDirection * stasisLaunchSpeed) - Main.screenPosition, new Rectangle(0, 0, 16, 10), color, rotation, new Vector2(8, 5), projectile.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.MiscStasisArrowMiddle, projectile.Center + (stasisLaunchDirection) - Main.screenPosition, new Rectangle(0, 0, 16, (int)(2 * stasisLaunchSpeed * 5)), color, rotation, new Vector2(8, 5), projectile.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.MiscStasisArrow, projectile.Center + (stasisLaunchDirection) + new Vector2(0, (2 * stasisLaunchSpeed * 4.95f) * projectile.scale).RotatedBy(rotation) - Main.screenPosition, new Rectangle(0, 8, 16, 12), color, rotation, new Vector2(8, 5), projectile.scale, SpriteEffects.None, 1f);
            }
            return true;
        }
    }
}
