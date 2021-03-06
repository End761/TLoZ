﻿using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using TLoZ.Items.Shields;
using TLoZ.Items.Shields.Traveler;
using System.Linq;
using TLoZ.Projectiles.Hostile;

namespace TLoZ.Players
{
    public sealed partial class TLoZPlayer : ModPlayer
    {
        public void ResetParryEffects()
        {
            if (ParryVisualTime > 0)
                ParryVisualTime--;

            if (ParryRealTime > 0)
                ParryRealTime--;

            if (BackflipTime > 0)
            {
                player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                player.fullRotation = -BackflipRotation * player.direction;
                HasResetRotation = false;
                BackflipEnded = false;

                BackflipTime--;

                if (BackflipRotation < MathHelper.Pi * 2)
                    BackflipRotation += 0.24f;
            }

            else if (player.velocity.Y == 0 || BackflipTime <= 0)
                BackflipEnded = true;

            if (BackflipEnded)
            {
                if(!HasResetRotation)
                {
                    player.fullRotation = 0.0f;
                    HasResetRotation = true;
                }

                BackflipRotation = 0.0f;
            }

            if (HasJustBackflipped)
            {
                player.velocity = new Vector2(-5.5f * player.direction, -8.5f);
                BackflipTime = 45;
            }

            if (EquipedShield != null)
            {
                if (TargetDirection != -99 && player.controlUseTile && !HoldsTwoHander)
                {
                    player.velocity.X = 2.5f * player.direction;
                    ParryVisualTime = 40;
                    ParryRealTime = 20;
                }
            }

            EquipedShield = FindShield();
        }

        public void ModifyParryLayers(List<PlayerLayer> layers)
        {
            if (TargetDirection != -99 && !HoldsTwoHander && EquipedShield != null)
            {
                if(player.itemAnimation <= 0)
                    player.bodyFrame.Y = 3 * 56;

                layers.Add(TLoZDrawLayers.Instance.equipedShieldLayer);
            }
        }

        public void ParryProj(Projectile proj)
        {
            if (ParryRealTime > 0)
            {
                proj.hostile = false;
                proj.friendly = true;
                if(proj.type != ModContent.ProjectileType<GuardianLaser>())
                    proj.velocity = Helpers.DirectToMouse(proj.Center) * 16;
            }
        }

        public void ParryNPC(NPC npc)
        {
            if (ParryRealTime > 0)
            {
                npc.velocity.Y = -2.5f;
                npc.velocity.X = 8.5f * npc.knockBackResist * player.direction;
            }
        }

        public void PreParryHurt(out bool ignoreHit)
        {
            ignoreHit = false;

            if (ParryRealTime > 0)
                ignoreHit = true;
        }

        public void SetParryControls()
        {
            if (!BackflipEnded || ParryVisualTime > 0)
                BlockInputs(true, true, true, true);
        }

        private ShieldBase FindShield()
        {
            ShieldBase shield = null;

            foreach (Item item in player.inventory.Where(t => t.modItem != null && t.modItem is ShieldBase))
                shield = item.modItem as ShieldBase;
            
            return shield;
        }

        public ShieldBase EquipedShield { get; set; }

        public int ParryVisualTime { get; set; }

        public int ParryRealTime { get; set; }

        public bool HasResetRotation { get; private set; }

        private float _backflipRotation;
        public float BackflipRotation
        {
            get { return _backflipRotation; }
            private set
            {
                _backflipRotation = value;
                if (_backflipRotation > MathHelper.Pi * 2)
                    _backflipRotation = MathHelper.Pi * 2;
            }
        }

        public int BackflipTime { get; private set; }

        public bool BackflipEnded { get; private set; } 

        public bool HasJustBackflipped =>
            (TargetDirection == 1 && player.controlLeft && player.controlJump && !player.mount.Active && player.velocity.Y == 0) || 
            (TargetDirection == -1 && player.controlRight && player.controlJump && !player.mount.Active && player.velocity.Y == 0);
    }
}
