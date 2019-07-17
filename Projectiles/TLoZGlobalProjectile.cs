﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TLoZ.Items.Tools;
using TLoZ.NPCs;
using TLoZ.Players;
using TLoZ.Projectiles.Runes;
using TLoZ.Runes;

namespace TLoZ.Projectiles
{
    public class TLoZGlobalProjectile : GlobalProjectile
    {
        public int stasisTimer;
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
            canBeStasised = StasisableProjectiles.SuitableProjectiles.Contains(projectile.type);
            _isHostile = projectile.hostile;
        }
        public override void PostAI(Projectile projectile)
        {
            if (stasisChainsOpacity > 0.0f)
                stasisChainsOpacity -= 0.02f;

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
            if (projectile.hostile != _isHostile) projectile.hostile = _isHostile;
            if (stasisLaunchDirection * stasisLaunchSpeed != Vector2.Zero) projectile.velocity = stasisLaunchDirection * stasisLaunchSpeed;
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
            if (canBeStasised && projectile.active && Main.LocalPlayer.HeldItem.type == mod.ItemType<SheikahSlate>() && TLoZPlayer.Get(Main.LocalPlayer).SelectedRune is StasisRune)
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
            if (stasisChainsOpacity > 0.0f)
            {
                for (int i = 0; i < randomStasisPositions.Length; i++)
                {
                    LoZnpCs.DrawStasisChains(spriteBatch, projectile.Center, randomStasisPositions[i], stasisChainsOpacity);
                }
            }
            if (Stasised)
            {
                Helpers.StartShader(spriteBatch);

                int shaderID = stasisLaunchSpeed > 14f ? GameShaders.Armor.GetShaderIdFromItemId(ItemID.InfernalWispDye) : stasisLaunchSpeed > 7f ? GameShaders.Armor.GetShaderIdFromItemId(ItemID.UnicornWispDye) : GameShaders.Armor.GetShaderIdFromItemId(ItemID.PixieDye);
                GameShaders.Armor.Apply(shaderID, projectile);
                // Draw the start
                float rotation = stasisLaunchDirection.ToRotation() - (float)Math.PI / 2;
                Color color = stasisLaunchSpeed > 14f ? Color.Red : stasisLaunchSpeed > 7f ? Color.Orange : Color.Yellow;
                spriteBatch.Draw(TLoZTexxtures.MiscStasisArrow, projectile.Center + (stasisLaunchDirection * stasisLaunchSpeed) - Main.screenPosition, new Rectangle(0, 0, 16, 10), color, rotation, new Vector2(8, 5), projectile.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTexxtures.MiscStasisArrowMiddle, projectile.Center + (stasisLaunchDirection) - Main.screenPosition, new Rectangle(0, 0, 16, (int)(2 * stasisLaunchSpeed * 5)), color, rotation, new Vector2(8, 5), projectile.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTexxtures.MiscStasisArrow, projectile.Center + (stasisLaunchDirection) + new Vector2(0, (2 * stasisLaunchSpeed * 4.95f) * projectile.scale).RotatedBy(rotation) - Main.screenPosition, new Rectangle(0, 8, 16, 12), color, rotation, new Vector2(8, 5), projectile.scale, SpriteEffects.None, 1f);
            }
            return true;
        }
        public override void PostDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
        {
            Helpers.EndShader(spriteBatch);
        }
        public bool Stasised => stasisTimer > 0;
        public override bool InstancePerEntity => true;
        public static TLoZGlobalProjectile GetFor(Projectile projectile) => projectile.GetGlobalProjectile<TLoZGlobalProjectile>();
    }
}
