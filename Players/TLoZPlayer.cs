using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TLoZ.Items.Weapons.MasterSword;
using TLoZ.NPCs;
using TLoZ.Projectiles;
using TLoZ.Projectiles.Runes;

namespace TLoZ.Players
{
    public sealed partial class TLoZPlayer : ModPlayer
    {
        public int responseIndex;

        public static TLoZPlayer Get(Player player) => player.GetModPlayer<TLoZPlayer>();

        public bool Holds(int type) => player.HeldItem.type == type;

        public Vector2 stasisLaunchVelocity;
        public float postStasisLaunchTimer;

        public bool usesParaglider;
        private int _paragliderNoFallDamageTimer;

        #region Rune Selection UI variables
        public float opacity;

        public bool IsSelectingRune => TLoZInput.changeRune.Current;
        public int inputLag;
        #endregion

        public int itemUseDelay;

        public bool isNearBomb;

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            if (TLoZ.loZClientConfig.spawnWithClothes)
            {
                Item hat = new Item();
                hat.SetDefaults(ItemID.HerosHat);
                Item shirt = new Item();
                shirt.SetDefaults(ItemID.HerosShirt);
                Item pants = new Item();
                pants.SetDefaults(ItemID.HerosPants);

                items.Add(hat);
                items.Add(shirt);
                items.Add(pants);
            }
        }

        public override void Initialize()
        {
            InitializeRunes();
        }

        public override void ResetEffects()
        {
            if (Main.gameMenu)
                HasBomb = false;

            if (itemUseDelay > 0)
                itemUseDelay--;

            if (player.ownedProjectileCounts[mod.ProjectileType<BombRound>()] <= 0)
                HasBomb = false;

            HasMasterSword = player.HasItem(mod.ItemType<MasterSword>());
            UsingMasterSword = !(player.itemAnimation <= 0 && Holds(mod.ItemType<MasterSword>()));

            if (LastChatFromNPC != null && LastChatFromNPC.active)
                HandleNPCChat(LastChatFromNPC);

            if (stasisLaunchVelocity != Vector2.Zero)
            {
                player.velocity = stasisLaunchVelocity * 2;
                player.gravity = 0;
                stasisLaunchVelocity = Vector2.Zero;
            }

            if (postStasisLaunchTimer > 0.0f) postStasisLaunchTimer -= 0.1f;
            if (inputLag > 0) inputLag--;

            if (_paragliderNoFallDamageTimer > 0)
            {
                player.noFallDmg = true;
                _paragliderNoFallDamageTimer--;
            }

            if (usesParaglider)
            {
                _paragliderNoFallDamageTimer = 60;
                player.maxFallSpeed *= 0.05f;

                if (player.velocity.Y != 0)
                {
                    player.moveSpeed *= 2.25f;
                    player.maxRunSpeed *= 2.25f;
                    player.runAcceleration *= 2.25f;
                }

                bool upDrafted = false;

                for (int i = 0; i < 10; i++)
                {
                    if (Main.tile[(int)player.position.X / 16, (int)player.position.Y / 16 + i].type == TileID.Campfire)
                        upDrafted = true;
                }

                if (upDrafted)
                    player.velocity.Y = -20f;
            }

            isNearBomb = false;
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            LoZnpCs tlozTarget = LoZnpCs.GetFor(npc);

            if (tlozTarget.stasised)
                return false;

            return base.CanBeHitByNPC(npc, ref cooldownSlot);
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            LoZnpCs tlozTarget = LoZnpCs.GetFor(npc);

            if (tlozTarget.postStasisFlyTimer > 0.0f)
            {
                postStasisLaunchTimer = 6.5f;
                stasisLaunchVelocity = npc.velocity;
            }
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            TLoZGlobalProjectile tlozProj = TLoZGlobalProjectile.GetFor(proj);
            if (tlozProj.postStasisLaunchTimer > 0.0f)
            {
                postStasisLaunchTimer = 6.5f;
                stasisLaunchVelocity = proj.velocity;
            }

        }

        public override void UpdateDead()
        {
            usesParaglider = false;
            HasBomb = false;
            stasisLaunchVelocity = Vector2.Zero;
            postStasisLaunchTimer = 0;
            inputLag = 0;
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            LoZnpCs tlozTarget = LoZnpCs.GetFor(target);
            if (tlozTarget.stasised)
            {
                crit = false;
                damage *= 0;
                target.life += 1;
                tlozTarget.stasisLaunchDirection = Helpers.DirectToMouse(target.Center, 1f);
                tlozTarget.stasisLaunchSpeed += item.knockBack * 0.5f;
            }
        }
        public override bool? CanHitNPC(Item item, NPC target)
        {
            if (item.type == mod.ItemType<MasterSword>() && (target.type == NPCID.Clothier || target.type == NPCID.OldMan))
                return true;
            if (LoZnpCs.GetFor(target).stasised)
                return true;
            return base.CanHitNPC(item, target);
        }
        public override bool? CanHitNPCWithProj(Projectile proj, NPC target)
        {
            if (LoZnpCs.GetFor(target).stasised)
                return true;
            return base.CanHitNPCWithProj(proj, target);
        }
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            if (HasBomb)
                player.bodyFrame.Y = 5 * 56;

            if (HasMasterSword)
            {
                layers.Insert(layers.FindIndex(x => x.Name.Equals("Arms")), MasterSword.masterSwordSheathBelt);
                layers.Insert(0, MasterSword.masterSwordSheath);
            }

            if (usesParaglider)
            {
                player.bodyFrame.Y = 2 * 56;
                layers.Insert(layers.FindIndex(x => x.Name.Equals("Arms")), paraglider);
            }

            ModifyGlowPlayerDrawLayers(layers);
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            ProcessRuneSelectionTriggers(triggersSet);

            if (TLoZInput.equipParaglider.JustPressed)
                usesParaglider = !usesParaglider;
        }
        public override void SetControls()
        {
            if (postStasisLaunchTimer > 0.0f) BlockInputs();
            if (IsSelectingRune) BlockInputs(true, false, false, false);
        }
        //Custom Methods:

        public void HandleNPCChat(NPC npc)
        {
            if (HasMasterSword) // Handle Master Sword reactions
            {
                if (npc.type == NPCID.Clothier)
                {
                    Main.npcChatText = TLoZDialogues.clothierMasterSwordReactions[responseIndex];
                }
            }
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();

            SaveRunes(tag);

            return tag;
        }

        public override void Load(TagCompound tag)
        {
            LoadRunes(tag);

            base.Load(tag);
        }

        private void BlockInputs(bool blockDirections = true, bool blockJumps = true, bool blockItemUse = true, bool blockOther = true)
        {
            if (blockDirections)
            {
                player.controlDown = false;
                player.controlUp = false;
                player.controlLeft = false;
                player.controlRight = false;
            }
            if (blockJumps)
                player.controlJump = false;

            if (blockItemUse)
            {
                player.controlUseItem = false;
                player.controlUseTile = false;
            }
            if (blockOther)
            {
                player.controlThrow = false;
                player.controlMount = false;
                player.controlHook = false;
            }
        }

        public static readonly PlayerLayer paraglider = new PlayerLayer("TLoZ", "Paraglider", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            Player drawPlayer = drawInfo.drawPlayer;

            if (drawPlayer.dead) // If the player can't use the item, don't draw it.
                return;
            Color color = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16);
            DrawData sheathData = new DrawData
            (
                ModContent.GetTexture("TLoZ/Items/Tools/ParaByLiz"),
                new Vector2((int)drawPlayer.MountedCenter.X, (int)drawPlayer.MountedCenter.Y - 22) - Main.screenPosition,
                null,
                color,
                0,
                new Vector2(19, 12),
                1f,
                drawPlayer.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                1
                );
            Main.playerDrawData.Add(sheathData);
        });

        // Properties

        private bool HasMasterSword { get; set; }
        public bool UsingMasterSword { get; private set; }

        public bool HasBomb { get; set; }

        public NPC LastChatFromNPC { get; internal set; }
    }
}
