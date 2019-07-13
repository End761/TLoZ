using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TLoZ.Buffs;

namespace TLoZ.NPCs
{
    public class TLoZNpcs : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public static TLoZNpcs GetFor(NPC npc) => npc.GetGlobalNPC<TLoZNpcs>();
        #region Rune variables

        #region Stasis variables
        public bool Stasised;
        public Vector2 PreStasisPosition;
        public Vector2 StasisLaunchDirection;
        public float StasisLaunchSpeed;
        public float PostStasisFlyTimer;
        public bool NoGravityDefault;
        public Vector2[] RandomStasisPositions;
        public float StasisChainsOpacity;
        public Color PreStasisColor;
        #endregion

        #endregion
        public override void SetDefaults(NPC npc)
        {
            PreStasisColor = npc.color;
            RandomStasisPositions = new[] { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero };
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
            if (StasisLaunchDirection * StasisLaunchSpeed != Vector2.Zero) npc.velocity = StasisLaunchDirection * StasisLaunchSpeed;
            StasisLaunchSpeed = 0.0f;
            StasisLaunchDirection = Vector2.Zero;
            if (PostStasisFlyTimer > 0.0f) npc.noGravity = true;
            else if (npc.noGravity != NoGravityDefault) npc.noGravity = NoGravityDefault;
            PreStasisPosition = npc.Center;

            if (PostStasisFlyTimer > 0.0f)
                return false;
            return base.PreAI(npc);
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Stasised)
            {
                damage *= 0;
                npc.life += 1;
                crit = false;
                StasisLaunchDirection = projectile.velocity.SafeNormalize(-Vector2.UnitY);
                StasisLaunchSpeed += 0.72f;
            }
        }
        public override void ResetEffects(NPC npc)
        {
            if (PostStasisFlyTimer > 0.0f) PostStasisFlyTimer -= 0.1f;
            if (StasisChainsOpacity > 0.0f) StasisChainsOpacity -= 0.02f;
            if (Stasised && !npc.HasBuff(mod.BuffType<StasisDebuff>()))
            {
                npc.color = PreStasisColor;
                Main.PlaySound(13);
            }
            if (!Stasised && npc.HasBuff(mod.BuffType<StasisDebuff>()))
            {
                StasisChainsOpacity = 2.0f;
                for (int i = 0; i < 4; i++)
                {
                    int multiplier = Main.rand.NextBool() ? -1 : 1;
                    float x = Main.rand.Next(40, 60) * multiplier;
                    multiplier = Main.rand.NextBool() ? -1 : 1;
                    float y = Main.rand.Next(40, 60) * multiplier;
                    RandomStasisPositions[i] = npc.Center + new Vector2(x, y);
                }
            }
            Stasised = false;
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            if (StasisChainsOpacity > 0.0f)
            {
                for (int i = 0; i < RandomStasisPositions.Length; i++)
                {
                    DrawStasisChains(spriteBatch, npc.Center, RandomStasisPositions[i], StasisChainsOpacity);
                }
            }
            if (Stasised)
            {
                // Draw the start
                float rotation = StasisLaunchDirection.ToRotation() - (float)Math.PI / 2;
                Color color = StasisLaunchSpeed > 14f ? Color.Red : StasisLaunchSpeed > 7f ? Color.Orange : Color.Yellow;
                spriteBatch.Draw(TLoZTextures.Misc_StasisArrow, npc.Center + (StasisLaunchDirection * StasisLaunchSpeed) - Main.screenPosition, new Rectangle(0, 0, 16, 10), color, rotation, new Vector2(8, 5), npc.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.Misc_StasisArrowMiddle, npc.Center + (StasisLaunchDirection) - Main.screenPosition, new Rectangle(0, 0, 16, (int)(2 * StasisLaunchSpeed * 5)), color, rotation, new Vector2(8, 5), npc.scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(TLoZTextures.Misc_StasisArrow, npc.Center + (StasisLaunchDirection) + new Vector2(0, (2 * StasisLaunchSpeed * 4.95f) * npc.scale).RotatedBy(rotation) - Main.screenPosition, new Rectangle(0, 8, 16, 12), color, rotation, new Vector2(8, 5), npc.scale, SpriteEffects.None, 1f);
            }
            return base.PreDraw(npc, spriteBatch, drawColor);
        }

        public static void DrawStasisChains(SpriteBatch spriteBatch, Vector2 StartPosition, Vector2 EndPosition, float _opacity = 1f)
        {
            float rotation = (EndPosition - StartPosition).ToRotation() - (float)Math.PI / 2;
            float _length = (EndPosition - StartPosition).Length();
            //spriteBatch.Draw(TLoZTextures.Misc_StasisChain, StartPosition - Main.screenPosition, new Rectangle(0, 0, 16, 10), Color.White * _opacity, rotation, new Vector2(8, 5), 1f, SpriteEffects.FlipVertically, 1f);
            for (float i = 0.0f; i < _length; i += 1.0f)
            {
                float multiplier = i >= _length - 1 ? 19.8f : 20;
                Vector2 offset = (EndPosition - StartPosition).SafeNormalize(-Vector2.UnitY) * i * multiplier;
                spriteBatch.Draw(TLoZTextures.Misc_StasisChain, StartPosition + offset - Main.screenPosition, i >= _length - 2 ? new Rectangle(0, 0, 16, 10) : new Rectangle(0, 12, 16, 20), Color.White * _opacity, rotation, new Vector2(8, 10), 1f, i >= _length - 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 1f);
            }
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            TLoZPlayer mPlayer = TLoZPlayer.Get(Main.LocalPlayer);
            mPlayer.LastNPCChat = npc;
            if(npc.type == NPCID.Clothier)
            {
                mPlayer.ResponseIndex = Main.rand.Next(TLoZDialogues.Clothier_MasterSwordReactions.Length);
            }
        }
        public override bool PreChatButtonClicked(NPC npc, bool firstButton)
        {
            TLoZPlayer mPlayer = TLoZPlayer.Get(Main.LocalPlayer);
            mPlayer.LastNPCChat = null;
            return base.PreChatButtonClicked(npc, firstButton);
        }
    }
}
