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
    public class TLoZProjectiles : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public static TLoZProjectiles GetFor(Projectile projectile) => projectile.GetGlobalProjectile<TLoZProjectiles>();
        public int StasisTimer;
        public bool Stasised => StasisTimer > 0;
        public bool LastStasisedState;
        public Vector2 StasisLaunchDirection;
        public float StasisLaunchSpeed;
        public float PostStasisLaunchTimer;
        public Vector2[] RandomStasisPositions;
        public float StasisChainsOpacity;

        public int CantGetHitTimer;
        private bool _isHostile;
        public bool CanBeStasised; 
        public override void SetDefaults(Projectile projectile)
        {
            RandomStasisPositions = new[] { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero };
            CanBeStasised = projectile.type == ProjectileID.Boulder || projectile.type == mod.ProjectileType<BombRound>();
            _isHostile = projectile.hostile;
        }
        public override void PostAI(Projectile projectile)
        {
            if (StasisChainsOpacity > 0.0f) StasisChainsOpacity -= 0.02f;
            LastStasisedState = Stasised;
        }
        public override bool PreAI(Projectile projectile)
        {
            if(Stasised && !LastStasisedState)
            {
                StasisChainsOpacity = 2.0f;
                for (int i = 0; i < 4; i++)
                {
                    float x = Main.rand.Next(-60, 60);
                    float y = Main.rand.Next(-60, 60);
                    RandomStasisPositions[i] = projectile.Center + new Vector2(x, y);
                }
            }
            if (StasisTimer > 0)
                StasisTimer--;
            if(StasisTimer == 1)
            {
                Main.PlaySound(13);
            }
            if (CantGetHitTimer > 0) CantGetHitTimer--;
            if (PostStasisLaunchTimer > 0.0f) PostStasisLaunchTimer -= 0.1f; 
            if(Stasised)
            {
                projectile.timeLeft++;
                projectile.hostile = false;
                PostStasisLaunchTimer = 6.5f;
                foreach (Projectile proj in Main.projectile)
                {
                    if (!proj.active || proj == projectile)
                        continue;
                    if (projectile.Hitbox.Intersects(proj.Hitbox) && CantGetHitTimer <= 0)
                    {
                        CantGetHitTimer = 5;
                        if (proj.penetrate != -1)
                        {
                            CantGetHitTimer = 10;
                            proj.Kill();
                        }
                        Main.PlaySound(21);
                        StasisLaunchDirection = proj.velocity.SafeNormalize(-Vector2.UnitY);
                        StasisLaunchSpeed += proj.knockBack * 0.5f;
                    }
                }
                return false;
            } 
            if (projectile.hostile != _isHostile) projectile.hostile = _isHostile;
            if (StasisLaunchDirection * StasisLaunchSpeed != Vector2.Zero) projectile.velocity = StasisLaunchDirection * StasisLaunchSpeed;
            StasisLaunchDirection = Vector2.Zero;
            StasisLaunchSpeed = 0.0f;
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
            if (StasisChainsOpacity > 0.0f)
            {
                for (int i = 0; i < RandomStasisPositions.Length; i++)
                {
                    TLoZNpcs.DrawStasisChains(spriteBatch, projectile.Center, RandomStasisPositions[i], StasisChainsOpacity);
                }
            }
            if (Stasised)
            {
                // Draw the start
                float rotation = StasisLaunchDirection.ToRotation() - (float)Math.PI / 2;
                Color color = StasisLaunchSpeed > 14f ? Color.Red : StasisLaunchSpeed > 7f ? Color.Orange : Color.Yellow;
                lightColor = color;
                spriteBatch.Draw(TLoZTextures.Misc_StasisArrow, projectile.Center + (StasisLaunchDirection * StasisLaunchSpeed) - Main.screenPosition, new Rectangle(0, 0, 16, 10), color, rotation, new Vector2(8, 5), projectile.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.Misc_StasisArrowMiddle, projectile.Center + (StasisLaunchDirection) - Main.screenPosition, new Rectangle(0, 0, 16, (int)(2 * StasisLaunchSpeed * 5)), color, rotation, new Vector2(8, 5), projectile.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.Misc_StasisArrow, projectile.Center + (StasisLaunchDirection) + new Vector2(0, (2 * StasisLaunchSpeed * 4.95f) * projectile.scale).RotatedBy(rotation) - Main.screenPosition, new Rectangle(0, 8, 16, 12), color, rotation, new Vector2(8, 5), projectile.scale, SpriteEffects.None, 1f);
            }
            return true;
        }
    }
}
