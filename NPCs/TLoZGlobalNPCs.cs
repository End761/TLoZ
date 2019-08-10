using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TLoZ.Buffs;
using TLoZ.Items.Tools;
using TLoZ.Players;
using TLoZ.Runes;

namespace TLoZ.NPCs
{
    public class TLoZGlobalNPC : GlobalNPC
    {
        public static TLoZGlobalNPC GetFor(NPC npc) => npc.GetGlobalNPC<TLoZGlobalNPC>();

        #region Rune variables

        #region Stasis variables
        public bool stasised;
        public Vector2 preStasisPosition;
        public Vector2 stasisLaunchDirection;
        public float stasisLaunchSpeed;
        public float postStasisFlyTimer;
        public bool noGravityDefault;
        public Vector2[] randomStasisPositions;
        public float stasisChainsOpacity;
        public Color preStasisColor;
        public float stasisDustTimer;
        public Color stasisDustColor;
        #endregion

        #endregion

        public override void SetDefaults(NPC npc)
        {
            preStasisColor = npc.color;
            randomStasisPositions = new[] { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero };

            noGravityDefault = npc.noGravity;
            preStasisPosition = npc.Center;
        }

        public override bool PreAI(NPC npc)
        {
            // Handle Stasis Rune
            if (stasised)
            {
                Color color = stasisLaunchSpeed > 14f ? Color.Red : stasisLaunchSpeed > 7f ? Color.Orange : Color.Yellow;

                npc.color = color;
                npc.frameCounter = 0;
                npc.noGravity = true;
                npc.Center = preStasisPosition;

                return false;
            }

            preStasisColor = npc.color;

            if (stasisLaunchDirection * stasisLaunchSpeed != Vector2.Zero)
            {
                stasisDustTimer = 15f;
                stasisDustColor = stasisLaunchSpeed > 14f ? Color.Red : stasisLaunchSpeed > 7f ? Color.Orange : Color.Yellow;
                npc.velocity = stasisLaunchDirection * stasisLaunchSpeed;
            }
            stasisLaunchSpeed = 0.0f;
            stasisLaunchDirection = Vector2.Zero;

            if (postStasisFlyTimer > 0.0f)
                npc.noGravity = true;

            else if (npc.noGravity != noGravityDefault)
                npc.noGravity = noGravityDefault;

            preStasisPosition = npc.Center;

            if (postStasisFlyTimer > 0.0f)
                return false;

            return base.PreAI(npc);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (stasised)
            {
                damage = 0;
                npc.life += 1;

                crit = false;

                stasisLaunchDirection = projectile.velocity.SafeNormalize(-Vector2.UnitY);
                stasisLaunchSpeed += 0.72f * knockback;
            }
        }

        public override void ResetEffects(NPC npc)
        {
            if (stasisDustTimer > 0.0f)
            {
                for (int i = 1; i < 5; i++)
                {
                    Helpers.CreateGeneralUseDust(2, npc.Center + npc.velocity / i, stasisDustColor);
                }
                stasisDustTimer -= 0.1f;
            }

            if (postStasisFlyTimer > 0.0f)
                postStasisFlyTimer -= 0.1f;

            if (stasisChainsOpacity > 0.0f)
                stasisChainsOpacity -= 0.02f;

            if (stasised && !npc.HasBuff(mod.BuffType<StasisDebuff>()))
            {
                npc.color = preStasisColor;
                Main.PlaySound(13);
            }

            if (!stasised && npc.HasBuff(mod.BuffType<StasisDebuff>()))
            {
                stasisChainsOpacity = 2.0f;

                for (int i = 0; i < 4; i++)
                {
                    float x = Main.rand.Next(40, 60) * (Main.rand.NextBool() ? -1 : 1);
                    float y = Main.rand.Next(40, 60) * (Main.rand.NextBool() ? -1 : 1);

                    randomStasisPositions[i] = npc.Center + new Vector2(x, y);
                }
            }

            stasised = false;
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            if (!npc.boss && npc.active && Main.LocalPlayer.HeldItem.type == mod.ItemType<SheikahSlate>() && TLoZPlayer.Get(Main.LocalPlayer).SelectedRune is StasisRune)
            {
                Helpers.StartShader(spriteBatch);
                GameShaders.Armor.Apply(GameShaders.Armor.GetShaderIdFromItemId(ItemID.PixieDye), npc);
            }
            else
            {
                Helpers.EndShader(spriteBatch);
            }
            if (stasisChainsOpacity > 0.0f)
            {
                for (int i = 0; i < randomStasisPositions.Length; i++)
                    DrawStasisChains(spriteBatch, npc.Center, randomStasisPositions[i], stasisChainsOpacity);
            }
            if (stasised)
            {
                Helpers.StartShader(spriteBatch);
                int shaderID = stasisLaunchSpeed > 14f ? GameShaders.Armor.GetShaderIdFromItemId(ItemID.InfernalWispDye) : stasisLaunchSpeed > 7f ? GameShaders.Armor.GetShaderIdFromItemId(ItemID.UnicornWispDye) : GameShaders.Armor.GetShaderIdFromItemId(ItemID.PixieDye);
                GameShaders.Armor.Apply(shaderID, npc);
                // Draw the start
                float rotation = stasisLaunchDirection.ToRotation() - (float)Math.PI / 2;
                Color color = stasisLaunchSpeed > 14f ? Color.Red : stasisLaunchSpeed > 7f ? Color.Orange : Color.Yellow;

                spriteBatch.Draw(TLoZTextures.MiscStasisArrow, npc.Center + (stasisLaunchDirection * stasisLaunchSpeed) - Main.screenPosition, new Rectangle(0, 0, 16, 10), color, rotation, new Vector2(8, 5), npc.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.MiscStasisArrowMiddle, npc.Center + (stasisLaunchDirection) - Main.screenPosition, new Rectangle(0, 0, 16, (int)(2 * stasisLaunchSpeed * 5)), color, rotation, new Vector2(8, 5), npc.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.MiscStasisArrow, npc.Center + (stasisLaunchDirection) + new Vector2(0, (2 * stasisLaunchSpeed * 4.95f) * npc.scale).RotatedBy(rotation) - Main.screenPosition, new Rectangle(0, 8, 16, 12), color, rotation, new Vector2(8, 5), npc.scale, SpriteEffects.None, 1f);

            }
            return base.PreDraw(npc, spriteBatch, drawColor);
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            Helpers.EndShader(spriteBatch);

            TLoZPlayer tlozPlayer = TLoZPlayer.Get(Main.LocalPlayer);

            if(tlozPlayer.myTarget != null && tlozPlayer.myTarget == npc)
            {
                spriteBatch.Draw(TLoZTextures.UITargeting, npc.Center - new Vector2(0, npc.height + 18) - Main.screenPosition, new Rectangle(0, 0, 16, 36), TLoZMod.loZClientConfig.targetBorderColor, 0f, new Vector2(8, 16), 1f, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.UITargeting, npc.Center - new Vector2(0, npc.height + 18) - Main.screenPosition, new Rectangle(0, 36, 16, 36), TLoZMod.loZClientConfig.targetCoreColor, 0f, new Vector2(8, 16), 1f, SpriteEffects.None, 1f);
            }
        }

        public static void DrawStasisChains(SpriteBatch spriteBatch, Vector2 startPosition, Vector2 endPosition, float opacity = 1f)
        {
            float rotation = (endPosition - startPosition).ToRotation() - (float)Math.PI / 2;
            float length = (endPosition - startPosition).Length();
            //spriteBatch.Draw(TLoZTextures.Misc_StasisChain, StartPosition - Main.screenPosition, new Rectangle(0, 0, 16, 10), Color.White * _opacity, rotation, new Vector2(8, 5), 1f, SpriteEffects.FlipVertically, 1f);

            for (float i = 0.0f; i < length; i += 1.0f)
            {
                float multiplier = i >= length - 1 ? 19.8f : 20;
                Vector2 offset = (endPosition - startPosition).SafeNormalize(-Vector2.UnitY) * i * multiplier;

                spriteBatch.Draw(TLoZTextures.MiscStasisChain, startPosition + offset - Main.screenPosition, i >= length - 2 ? new Rectangle(0, 0, 16, 10) : new Rectangle(0, 12, 16, 20), Color.White * opacity, rotation, new Vector2(8, 10), 1f, i >= length - 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 1f);
            }
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            TLoZPlayer tlozPlayer = TLoZPlayer.Get(Main.LocalPlayer);
            tlozPlayer.LastChatFromNPC = npc;

            if(npc.type == NPCID.Clothier)
            {
                tlozPlayer.responseIndex = Main.rand.Next(TLoZDialogues.clothierMasterSwordReactions.Length);
            }
        }
        public override bool PreChatButtonClicked(NPC npc, bool firstButton)
        {
            TLoZPlayer.Get(Main.LocalPlayer).LastChatFromNPC = null;

            return base.PreChatButtonClicked(npc, firstButton);
        }

        public override bool InstancePerEntity => true;
    }
}
