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

        public override void SetDefaults(NPC npc)
        {
            PreStasisColor = npc.color;
            StasisChainsPositions = new[] { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero };

            NoGravityDefault = npc.noGravity;
            PreStasisPosition = npc.Center;
        }

        public override bool PreAI(NPC npc)
        {
            // Handle Stasis Rune
            if (Stasised)
            {
                Color color = StasisLaunchSpeed > 14f ? Color.Red : StasisLaunchSpeed > 7f ? Color.Orange : Color.Yellow;

                npc.color = color;
                npc.frameCounter = 0;
                npc.noGravity = true;
                npc.Center = PreStasisPosition;

                return false;
            }

            PreStasisColor = npc.color;

            if (StasisLaunchDirection * StasisLaunchSpeed != Vector2.Zero)
            {
                StasisDustTimer = 15f;
                StasisDustColor = StasisLaunchSpeed > 14f ? Color.Red : StasisLaunchSpeed > 7f ? Color.Orange : Color.Yellow;
                npc.velocity = StasisLaunchDirection * StasisLaunchSpeed;
            }

            StasisLaunchSpeed = 0.0f;
            StasisLaunchDirection = Vector2.Zero;

            if (PostStasisFlyTimer > 0.0f)
                npc.noGravity = true;

            else if (npc.noGravity != NoGravityDefault)
                npc.noGravity = NoGravityDefault;

            PreStasisPosition = npc.Center;

            if (PostStasisFlyTimer > 0.0f)
                return false;

            return base.PreAI(npc);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Stasised)
            {
                damage = 0;
                npc.life += 1;

                crit = false;

                StasisLaunchDirection = projectile.velocity.SafeNormalize(-Vector2.UnitY);
                StasisLaunchSpeed += 0.72f * knockback;
            }
        }

        public override void ResetEffects(NPC npc)
        {
            if (StasisDustTimer > 0.0f)
            {
                for (int i = 1; i < 5; i++)
                {
                    Helpers.CreateGeneralUseDust(2, npc.Center + npc.velocity / i, StasisDustColor);
                }

                StasisDustTimer -= 0.1f;
            }

            if (PostStasisFlyTimer > 0.0f)
                PostStasisFlyTimer -= 0.1f;

            if (StasisChainsOpacity > 0.0f)
                StasisChainsOpacity -= 0.02f;

            if (Stasised && !npc.HasBuff(ModContent.BuffType<StasisDebuff>()))
            {
                npc.color = PreStasisColor;
                Main.PlaySound(13);
            }

            if (!Stasised && npc.HasBuff(ModContent.BuffType<StasisDebuff>()))
            {
                StasisChainsOpacity = 2.0f;

                for (int i = 0; i < 4; i++)
                {
                    float x = Main.rand.Next(40, 60) * (Main.rand.NextBool() ? -1 : 1);
                    float y = Main.rand.Next(40, 60) * (Main.rand.NextBool() ? -1 : 1);

                    StasisChainsPositions[i] = npc.Center + new Vector2(x, y);
                }
            }

            Stasised = false;
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            if (!npc.boss && npc.active && Main.LocalPlayer.HeldItem.type == ModContent.ItemType<SheikahSlate>() && TLoZPlayer.Get(Main.LocalPlayer).SelectedRune is StasisRune)
            {
                Helpers.StartShader(spriteBatch);
                GameShaders.Armor.Apply(GameShaders.Armor.GetShaderIdFromItemId(ItemID.PixieDye), npc);
            }
            else
            {
                Helpers.EndShader(spriteBatch);
            }

            if (StasisChainsOpacity > 0.0f)
            {
                for (int i = 0; i < StasisChainsPositions.Length; i++)
                    DrawStasisChains(spriteBatch, npc.Center, StasisChainsPositions[i], StasisChainsOpacity);
            }

            if (Stasised)
            {
                Helpers.StartShader(spriteBatch);
                int shaderID = StasisLaunchSpeed > 14f ? GameShaders.Armor.GetShaderIdFromItemId(ItemID.InfernalWispDye) : StasisLaunchSpeed > 7f ? GameShaders.Armor.GetShaderIdFromItemId(ItemID.UnicornWispDye) : GameShaders.Armor.GetShaderIdFromItemId(ItemID.PixieDye);
                GameShaders.Armor.Apply(shaderID, npc);
                // Draw the start
                float rotation = StasisLaunchDirection.ToRotation() - (float)Math.PI / 2;
                Color color = StasisLaunchSpeed > 14f ? Color.Red : StasisLaunchSpeed > 7f ? Color.Orange : Color.Yellow;

                spriteBatch.Draw(TLoZTextures.MiscStasisArrow, npc.Center + (StasisLaunchDirection * StasisLaunchSpeed) - Main.screenPosition, new Rectangle(0, 0, 16, 10), color, rotation, new Vector2(8, 5), npc.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.MiscStasisArrowMiddle, npc.Center + (StasisLaunchDirection) - Main.screenPosition, new Rectangle(0, 0, 16, (int)(2 * StasisLaunchSpeed * 5)), color, rotation, new Vector2(8, 5), npc.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.MiscStasisArrow, npc.Center + (StasisLaunchDirection) + new Vector2(0, (2 * StasisLaunchSpeed * 4.95f) * npc.scale).RotatedBy(rotation) - Main.screenPosition, new Rectangle(0, 8, 16, 12), color, rotation, new Vector2(8, 5), npc.scale, SpriteEffects.None, 1f);

            }

            return base.PreDraw(npc, spriteBatch, drawColor);
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            Helpers.EndShader(spriteBatch);

            TLoZPlayer tlozPlayer = TLoZPlayer.Get(Main.LocalPlayer);

            if(tlozPlayer.MyTarget != null && tlozPlayer.MyTarget == npc)
            {
                spriteBatch.Draw(TLoZTextures.UITargeting, npc.Center - new Vector2(0, npc.height + 18) - Main.screenPosition, new Rectangle(0, 0, 16, 36), TLoZMod.loZClientConfig.targetBorderColor, 0f, new Vector2(8, 16), 1f, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.UITargeting, npc.Center - new Vector2(0, npc.height + 18) - Main.screenPosition, new Rectangle(0, 36, 16, 36), TLoZMod.loZClientConfig.targetCoreColor, 0f, new Vector2(8, 16), 1f, SpriteEffects.None, 1f);
            }
        }

        public static void DrawStasisChains(SpriteBatch spriteBatch, Vector2 startPosition, Vector2 endPosition, float opacity = 1f)
        {
            float rotation = (endPosition - startPosition).ToRotation() - (float)Math.PI / 2;
            float length = (endPosition - startPosition).Length();

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
                tlozPlayer.ResponseIndex = Main.rand.Next(TLoZDialogues.clothierMasterSwordReactions.Length);
            }
        }

        public override bool PreChatButtonClicked(NPC npc, bool firstButton)
        {
            TLoZPlayer.Get(Main.LocalPlayer).LastChatFromNPC = null;

            return base.PreChatButtonClicked(npc, firstButton);
        }

        public override bool InstancePerEntity => true;
        
        public bool Stasised { get; set; }

        public Vector2 PreStasisPosition { get; set; }

        public Vector2 StasisLaunchDirection { get; set; }

        public float StasisLaunchSpeed { get; set; }

        public float PostStasisFlyTimer { get; set; }

        public bool NoGravityDefault { get; private set; }

        public Vector2[] StasisChainsPositions { get; private set; }

        public float StasisChainsOpacity { get; private set; }

        public Color PreStasisColor { get; private set; }

        public float StasisDustTimer { get; private set; }

        public Color StasisDustColor { get; private set; }
    }
}
