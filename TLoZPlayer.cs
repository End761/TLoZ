using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using TLoZ.Items.Weapons.MasterSword;
using TLoZ.NPCs;
using TLoZ.Projectiles;
using TLoZ.Projectiles.Runes;

namespace TLoZ
{
    public class TLoZPlayer : ModPlayer
    {
        public bool NotUsingMasterSword;
        public bool HasMasterSword;
        public NPC LastNPCChat;
        public int ResponseIndex;
        public static TLoZPlayer Get(Player player) => player.GetModPlayer<TLoZPlayer>();
        
        public bool Holds(int type) => player.HeldItem.type == type;

        public Vector2 StasisLaunchVelocity;
        public float PostStasisLaunchTimer;

        public bool HasBomb;
        private int _selectedRune;
        public int SelectedRune
        {
            get { return _selectedRune; }
            set
            {
                _selectedRune = value;
                if (_selectedRune > 4) _selectedRune = 0;
                if (_selectedRune < 0) _selectedRune = 4;
            }
        }

        public bool UsesParaglider;
        private int _paragliderNoFallDamageTimer;

        #region Rune Selection UI variables
        public float Opacity;
        public bool IsSelectingRune => TLoZInput.ChangeRune.Current;
        public int InputLag;
        #endregion
        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            if (TLoZClientConfig.ShouldSpawnWithClothes)
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
        public override void ResetEffects()
        {
            if(Main.gameMenu)
                HasBomb = false;
            if(player.ownedProjectileCounts[mod.ProjectileType<BombRound>()] <= 0)
            {
                HasBomb = false;
            }
            HasMasterSword = player.HasItem(mod.ItemType<MasterSword>());
            NotUsingMasterSword = player.itemAnimation <= 0 && Holds(mod.ItemType<MasterSword>());
            if (LastNPCChat != null && LastNPCChat.active)
                HandleNPCChat(LastNPCChat);
            if (StasisLaunchVelocity != Vector2.Zero)
            {
                player.velocity = StasisLaunchVelocity * 2;
                player.gravity = 0;
                StasisLaunchVelocity = Vector2.Zero;
            }
            if (PostStasisLaunchTimer > 0.0f) PostStasisLaunchTimer -= 0.1f;
            if (InputLag > 0) InputLag--;
            if (_paragliderNoFallDamageTimer > 0)
            {
                player.noFallDmg = true;
                _paragliderNoFallDamageTimer--;
            }
            if (UsesParaglider)
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
                    {
                        upDrafted = true;
                    }
                }
                if(upDrafted)
                    player.velocity.Y = -20f;
            }
        }
        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            TLoZNpcs tlozTarget = TLoZNpcs.GetFor(npc);
            if (tlozTarget.Stasised)
            {
                return false;
            }
            return base.CanBeHitByNPC(npc, ref cooldownSlot);
        }
        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            TLoZNpcs tlozTarget = TLoZNpcs.GetFor(npc);
            if (tlozTarget.PostStasisFlyTimer > 0.0f)
            {
                 PostStasisLaunchTimer = 6.5f;
                 StasisLaunchVelocity = npc.velocity;
            }
        }
        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            TLoZProjectiles tlozProj = TLoZProjectiles.GetFor(proj);
            if(tlozProj.PostStasisLaunchTimer > 0.0f)
            {
                PostStasisLaunchTimer = 6.5f;
                StasisLaunchVelocity = proj.velocity;
            }

        }
        public override void UpdateDead()
        {
            UsesParaglider = false;
            HasBomb = false;
            StasisLaunchVelocity = Vector2.Zero;
            PostStasisLaunchTimer = 0;
            InputLag = 0;
        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            TLoZNpcs tlozTarget = TLoZNpcs.GetFor(target);
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
            if (TLoZNpcs.GetFor(target).Stasised)
                return true;
            return base.CanHitNPC(item, target);
        }
        public override bool? CanHitNPCWithProj(Projectile proj, NPC target)
        {
            if (TLoZNpcs.GetFor(target).Stasised)
                return true;
            return base.CanHitNPCWithProj(proj, target);
        }
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            if (HasBomb)
                player.bodyFrame.Y = 5 * 56;
            if (HasMasterSword)
            {
                layers.Insert(layers.FindIndex(x => x.Name == "Arms"), MasterSword.MasterSwordSheathBelt);
                layers.Insert(0, MasterSword.MasterSwordSheath);
            }
            if(UsesParaglider)
            {
                player.bodyFrame.Y = 5 * 56;
                layers.Add(Paraglider);
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if(IsSelectingRune)
            {
                var inputLag = 10;
                if (triggersSet.Left && InputLag <= 0)
                {
                    InputLag = inputLag;
                    SelectedRune -= 1;
                }
                if (triggersSet.Right && InputLag <= 0)
                {
                    InputLag = inputLag;
                    SelectedRune += 1;
                }
                if(PlayerInput.ScrollWheelDelta > 0 && InputLag <= 0)
                {
                    SelectedRune++;
                }
                if (PlayerInput.ScrollWheelDelta < 0 && InputLag <= 0)
                {
                    SelectedRune--;
                }
                PlayerInput.ScrollWheelDelta = 0;
            }
            if(TLoZInput.EquipParaglider.JustPressed)
            {
                UsesParaglider = !UsesParaglider;
            }
        }
        public override void SetControls()
        {
            if(PostStasisLaunchTimer > 0.0f) BlockInputs();
            if (IsSelectingRune) BlockInputs(true, false, false, false);
        }
        //Custom Methods:

        public void HandleNPCChat(NPC npc)
        {
            if (HasMasterSword) // Handle Master Sword reactions
            {
                if (npc.type == NPCID.Clothier)
                {
                    Main.npcChatText = TLoZDialogues.Clothier_MasterSwordReactions[ResponseIndex];
                }
            }
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

        public static readonly PlayerLayer Paraglider = new PlayerLayer("TLoZ", "Paraglider", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            Player drawPlayer = drawInfo.drawPlayer;

            if (drawPlayer.dead) // If the player can't use the item, don't draw it.
                return;
            Color color = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16);
            DrawData sheathData = new DrawData
            (
                ModContent.GetTexture("TLoZ/Items/Tools/Paraglider"),
                new Vector2((int)drawPlayer.MountedCenter.X, (int)drawPlayer.MountedCenter.Y - 24) - Main.screenPosition,
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
    }
}
