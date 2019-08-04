using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TLoZ.Enums;
using TLoZ.Items.Tools;
using TLoZ.Items.Weapons;
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
            InitializeStamina();
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

            if (postStasisLaunchTimer > 0.0f)
                postStasisLaunchTimer -= 0.1f;

            if (inputLag > 0)
                inputLag--;

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

            ResetTargetingEffects();
            ResetStaminaEffects();
            ResetTwoHandedEffects();
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            TLoZGlobalNPC tlozTarget = TLoZGlobalNPC.GetFor(npc);

            if (tlozTarget.stasised)
                return false;

            return base.CanBeHitByNPC(npc, ref cooldownSlot);
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            TLoZGlobalNPC tlozTarget = TLoZGlobalNPC.GetFor(npc);

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
            _sprinting = false;
            inputLag = 0;
        }
        public override void PostUpdateRunSpeeds()
        {
            UpdateStaminaRunSpeeds();
        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            TLoZGlobalNPC tlozTarget = TLoZGlobalNPC.GetFor(target);
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
            if (TLoZGlobalNPC.GetFor(target).stasised)
                return true;
            return base.CanHitNPC(item, target);
        }
        public override bool? CanHitNPCWithProj(Projectile proj, NPC target)
        {
            if (TLoZGlobalNPC.GetFor(target).stasised)
                return true;
            return base.CanHitNPCWithProj(proj, target);
        }
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int armIndex = layers.FindIndex(x => x.Name.Equals("Arms"));
            if (HasBomb)
                player.bodyFrame.Y = 5 * 56;

            if (HasMasterSword)
            {
                layers.Insert(armIndex, TLoZDrawLayers.Instance.masterSwordSheathBelt);
                layers.Insert(0, TLoZDrawLayers.Instance.masterSwordSheath);
            }

            if (usesParaglider)
            {
                _exhaustedTimer = 30;
                player.bodyFrame.Y = 2 * 56;
                layers.Insert(armIndex, TLoZDrawLayers.Instance.paragliderLayer);
            }
            ModifyTwoHandedLayers(layers);
            ModifyGlowPlayerDrawLayers(layers);
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            ProcessRuneSelectionTriggers(triggersSet);

            if (HasParaglider && TLoZInput.equipParaglider.JustPressed && !exhausted)
            {
                player.mount?.Dismount(player);
                usesParaglider = !usesParaglider;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket gliderPacket = mod.GetPacket();
                    gliderPacket.Write((int)MessageType.Paraglider);
                    gliderPacket.Write((int)player.whoAmI);
                    gliderPacket.Write((bool)usesParaglider);
                    gliderPacket.Send();
                }
            }
        }
        public override void PostUpdate()
        {
            if (myTarget != null)
                player.direction = myTarget.Center.X > player.Center.X ? 1 : -1;
        }
        public override void PreUpdate()
        {
            PreGlowMaskUpdate();
        }
        public override void SetControls()
        {
            if (postStasisLaunchTimer > 0.0f)
                BlockInputs();

            if (IsSelectingRune)
                BlockInputs(true, false, false, false);
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
            SaveStamina(tag);

            return tag;
        }

        public override void Load(TagCompound tag)
        {
            LoadRunes(tag);
            LoadStamina(tag);

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

        // Properties

        private bool HasMasterSword { get; set; }
        public bool UsingMasterSword { get; private set; }

        public bool HasBomb { get; set; }

        public NPC LastChatFromNPC { get; internal set; }

        private bool HasParaglider => player.HasItem(mod.ItemType<ParagliderItem>());

        public bool Holds(int type) => player.HeldItem.type == type;
    }
}
