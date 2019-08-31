using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TLoZ.Dusts;
using TLoZ.Items.Tools;
using TLoZ.NPCs;
using TLoZ.Players;
using TLoZ.Projectiles.Runes;
using TLoZ.Runes;

namespace TLoZ.Projectiles
{
    public class TLoZGlobalProjectile : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            StasisChainsPositions = new[] { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero };
            CanBeStasised = StasisableProjectiles.SuitableProjectiles.Contains(projectile.type);
            IsHostile = projectile.hostile;
        }
        public override void PostAI(Projectile projectile)
        {
            if (StasisChainsOpacity > 0.0f)
                StasisChainsOpacity -= 0.02f;

            LastStasisedState = Stasised;
        }
        public override bool PreAI(Projectile projectile)
        {
            if (Stasised && !LastStasisedState)
            {
                StasisChainsOpacity = 2.0f;

                for (int i = 0; i < 4; i++)
                {
                    float x = Main.rand.Next(-60, 60);
                    float y = Main.rand.Next(-60, 60);
                    StasisChainsPositions[i] = projectile.Center + new Vector2(x, y);
                }
            }
            if (StasisTimer > 0)
                StasisTimer--;

            if(StasisTimer == 1)
            {
                Main.PlaySound(13);
            }

            if (CannottGetHitTimer > 0)
                CannottGetHitTimer--;

            if (PostStasisLaunchTimer > 0.0f)
            {
                PostStasisLaunchTimer -= 0.1f;
            }

            if(Stasised)
            {
                projectile.timeLeft++;
                projectile.hostile = false;
                PostStasisLaunchTimer = 6.5f;

                foreach (Projectile proj in Main.projectile)
                {
                    if (!proj.active || proj == projectile)
                        continue;

                    if (projectile.Hitbox.Intersects(proj.Hitbox) && CannottGetHitTimer <= 0)
                    {
                        CannottGetHitTimer = 5;

                        if (proj.penetrate != -1)
                        {
                            CannottGetHitTimer = 10;
                            proj.Kill();
                        }

                        Main.PlaySound(21);
                        StasisLaunchDirection = proj.velocity.SafeNormalize(-Vector2.UnitY);
                        StasisLaunchSpeed += proj.knockBack * 0.5f;
                    }
                }

                return false;
            } 

            if (projectile.hostile != IsHostile)
                projectile.hostile = IsHostile;

            if (StasisLaunchDirection * StasisLaunchSpeed != Vector2.Zero)
            {
                StasisDustColor = StasisLaunchSpeed > 14f ? Color.Red : StasisLaunchSpeed > 7f ? Color.Orange : Color.Yellow;
                StasisDustTimer = 15f;
                projectile.velocity = StasisLaunchDirection * StasisLaunchSpeed;
            }

            if (StasisDustTimer > 0.0f)
            {
                for (int i = 1; i < 5; i++)
                {
                    Helpers.CreateGeneralUseDust(2, projectile.Center + projectile.velocity / i, Color.Red);
                }

                StasisDustTimer -= 0.1f;
            }

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
            if (CanBeStasised && projectile.active && Main.LocalPlayer.HeldItem.type == mod.ItemType<SheikahSlate>() && TLoZPlayer.Get(Main.LocalPlayer).SelectedRune is StasisRune)
            {
                Helpers.StartShader(spriteBatch);
                GameShaders.Armor.Apply(GameShaders.Armor.GetShaderIdFromItemId(ItemID.PixieDye), projectile);
            }
            else
            {
                Helpers.EndShader(spriteBatch);
            }

            if (!projectile.active)
                return false;

            if (StasisChainsOpacity > 0.0f)
            {
                for (int i = 0; i < StasisChainsPositions.Length; i++)
                {
                    TLoZGlobalNPC.DrawStasisChains(spriteBatch, projectile.Center, StasisChainsPositions[i], StasisChainsOpacity);
                }
            }

            if (Stasised)
            {
                Helpers.StartShader(spriteBatch);

                int shaderID = StasisLaunchSpeed > 14f ? GameShaders.Armor.GetShaderIdFromItemId(ItemID.InfernalWispDye) : StasisLaunchSpeed > 7f ? GameShaders.Armor.GetShaderIdFromItemId(ItemID.UnicornWispDye) : GameShaders.Armor.GetShaderIdFromItemId(ItemID.PixieDye);
                GameShaders.Armor.Apply(shaderID, projectile);
                // Draw the start
                float rotation = StasisLaunchDirection.ToRotation() - (float)Math.PI / 2;
                Color color = StasisLaunchSpeed > 14f ? Color.Red : StasisLaunchSpeed > 7f ? Color.Orange : Color.Yellow;
                spriteBatch.Draw(TLoZTextures.MiscStasisArrow, projectile.Center + (StasisLaunchDirection * StasisLaunchSpeed) - Main.screenPosition, new Rectangle(0, 0, 16, 10), color, rotation, new Vector2(8, 5), projectile.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.MiscStasisArrowMiddle, projectile.Center + (StasisLaunchDirection) - Main.screenPosition, new Rectangle(0, 0, 16, (int)(2 * StasisLaunchSpeed * 5)), color, rotation, new Vector2(8, 5), projectile.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.MiscStasisArrow, projectile.Center + (StasisLaunchDirection) + new Vector2(0, (2 * StasisLaunchSpeed * 4.95f) * projectile.scale).RotatedBy(rotation) - Main.screenPosition, new Rectangle(0, 8, 16, 12), color, rotation, new Vector2(8, 5), projectile.scale, SpriteEffects.None, 1f);
            }

            return true;
        }

        public override void PostDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
        {
            Helpers.EndShader(spriteBatch);
        }

        public bool Stasised => StasisTimer > 0;

        public override bool InstancePerEntity => true;

        public static TLoZGlobalProjectile GetFor(Projectile projectile) => projectile.GetGlobalProjectile<TLoZGlobalProjectile>();

        public int StasisTimer { get; set; }

        public bool LastStasisedState { get; set; }

        public Vector2 StasisLaunchDirection { get; set; }

        public float StasisLaunchSpeed { get; set; }

        public float PostStasisLaunchTimer { get; set; }

        public Vector2[] StasisChainsPositions { get; set; }

        public float StasisChainsOpacity { get; set; }

        public float StasisDustTimer { get; set; }

        public Color StasisDustColor { get; set; }

        public int CannottGetHitTimer { get; set; }

        public bool IsHostile { get; private set; }

        public bool CanBeStasised { get; set; }
    }
}
