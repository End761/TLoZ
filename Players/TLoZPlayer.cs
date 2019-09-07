using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TLoZ.Items.Tools;
using TLoZ.Items.Weapons.Melee.MasterSword;
using TLoZ.Network;
using TLoZ.Network.Packets;
using TLoZ.NPCs;
using TLoZ.Projectiles;
using TLoZ.Projectiles.Runes;

namespace TLoZ.Players
{
    public sealed partial class TLoZPlayer : ModPlayer
    {

        public static TLoZPlayer Get(Player player) => player.GetModPlayer<TLoZPlayer>();

        private bool _paragliding;
        private int _paragliderNoFallDamageTimer;

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            if (TLoZMod.loZClientConfig.spawnWithClothes)
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
            InitializeOcarina();
        }

        public override void ResetEffects()
        {
            if (Main.gameMenu)
                HasBomb = false;

            if (ItemUseDelay > 0)
                ItemUseDelay--;

            if (player.ownedProjectileCounts[mod.ProjectileType<BombRound>()] <= 0)
                HasBomb = false;

            HasMasterSword = player.HasItem(mod.ItemType<MasterSword>());

            UsingMasterSword = !(player.itemAnimation <= 0 && Holds(mod.ItemType<MasterSword>()));

            if (LastChatFromNPC != null && LastChatFromNPC.active)
                HandleNPCChat(LastChatFromNPC);

            if (StasisLaunchVelocity != Vector2.Zero)
            {
                player.velocity = StasisLaunchVelocity * 2;
                player.gravity = 0;
                StasisLaunchVelocity = Vector2.Zero;
            }

            if (PostStasisLaunchTimer > 0.0f)
                PostStasisLaunchTimer -= 0.1f;

            if (InputDelay > 0)
                InputDelay--;

            if (_paragliderNoFallDamageTimer > 0)
            {
                player.noFallDmg = true;
                _paragliderNoFallDamageTimer--;
            }

            if (Paragliding)
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

            IsNearBomb = false;

            if (!Main.dedServ)
            {
                ResetTargetingEffects();
                ResetStaminaEffects();
                ResetTwoHandedEffects();
            }

            ResetParryEffects();
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            TLoZGlobalNPC tlozTarget = TLoZGlobalNPC.GetFor(npc);

            if (tlozTarget.Stasised)
                return false;

            return base.CanBeHitByNPC(npc, ref cooldownSlot);
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            TLoZGlobalNPC tlozTarget = TLoZGlobalNPC.GetFor(npc);

            if (tlozTarget.PostStasisFlyTimer > 0.0f)
            {
                PostStasisLaunchTimer = 6.5f;
                StasisLaunchVelocity = npc.velocity;
            }
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            TLoZGlobalProjectile tlozProj = TLoZGlobalProjectile.GetFor(proj);
            if (tlozProj.PostStasisLaunchTimer > 0.0f)
            {
                PostStasisLaunchTimer = 6.5f;
                StasisLaunchVelocity = proj.velocity;
            }
            ParryProj(proj);
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            PreParryHurt(out bool ignoreHit);

            return !ignoreHit;
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            ParryNPC(npc);
        }

        public override void UpdateDead()
        {
            Paragliding = false;
            HasBomb = false;

            StasisLaunchVelocity = Vector2.Zero;
            PostStasisLaunchTimer = 0;

            _sprinting = false;
            InputDelay = 0;
        }

        public override void PostUpdateRunSpeeds()
        {
            PostUpdateTHWRunSpeeds();
            UpdateStaminaRunSpeeds();
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            TLoZGlobalNPC tlozTarget = TLoZGlobalNPC.GetFor(target);
            if (tlozTarget.Stasised)
            {
                crit = false;
                damage *= 0;
                target.life += 1;
                tlozTarget.StasisLaunchDirection = Helpers.DirectToMouse(target.Center, 1f);
                tlozTarget.StasisLaunchSpeed += item.knockBack * 0.5f;
            }
        }

        public override bool? CanHitNPC(Item item, NPC target)
        {
            if (item.type == mod.ItemType<MasterSword>() && (target.type == NPCID.Clothier || target.type == NPCID.OldMan))
                return true;

            if (TLoZGlobalNPC.GetFor(target).Stasised)
                return true;

            return base.CanHitNPC(item, target);
        }

        public override bool? CanHitNPCWithProj(Projectile proj, NPC target)
        {
            if (TLoZGlobalNPC.GetFor(target).Stasised)
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

            if (Paragliding)
            {
                _exhaustedTimer = 30;
                player.bodyFrame.Y = 2 * 56;
                layers.Insert(armIndex, TLoZDrawLayers.Instance.paragliderLayer);
            }

            ModifyTwoHandedLayers(layers);
            ModifyParryLayers(layers);
            ModifyGlowPlayerDrawLayers(layers);
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            ProcessRuneSelectionTriggers(triggersSet);

            ProcessOcarinaTriggers(triggersSet);

            if (HasParaglider && TLoZInput.EquipParaglider.JustPressed && !Exhausted)
            {
                player.mount?.Dismount(player);
                Paragliding = !Paragliding;
            }

            TLoZInput.Update();
        }

        public override void PostUpdate()
        {
            if (TargetDirection != -99)
                player.direction = TargetDirection;

            PostUpdateOcarina();
        }

        public override void PreUpdate()
        {
            PreGlowMaskUpdate();
        }

        public override void SetControls()
        {
            if (PostStasisLaunchTimer > 0.0f)
                BlockInputs();

            if (IsSelectingRune)
                BlockInputs(true, false, false, false);

            SetParryControls();

            SetOcarinaControls();
        }
        //Custom Methods:

        public void HandleNPCChat(NPC npc)
        {
            if (HasMasterSword) // Handle Master Sword reactions
            {
                if (npc.type == NPCID.Clothier)
                {
                    Main.npcChatText = TLoZDialogues.clothierMasterSwordReactions[ResponseIndex];
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

        public bool Paragliding
        {
            get => _paragliding;
            set
            {
                if (_paragliding == value) return;

                _paragliding = value;
                NetworkPacketManager.Instance.SendPacketToServerIfLocal<PlayerParagliderStatePacket>(this.player, player.whoAmI, _paragliding);
            }
        }

        public bool Holds(int type) => player.HeldItem.type == type;

        public bool IsSelectingRune => TLoZInput.ChangeRune.Current;

        public float Opacity { get; set; }

        public int InputDelay { get; set; }

        public Vector2 StasisLaunchVelocity { get; set; }
        public float PostStasisLaunchTimer { get; set; }

        public bool IsNearBomb { get; set; }
        public int ResponseIndex { get; set; }

        public int ItemUseDelay { get; set; }
    }
}
