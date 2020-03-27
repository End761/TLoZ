using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TLoZ.Items.Weapons;
using TLoZ.Items.Weapons.Melee;
using TLoZ.Projectiles.Misc;

namespace TLoZ.Players
{
    public partial class TLoZPlayer : ModPlayer
    {
        public void PostUpdateTHWRunSpeeds()
        {
            if(TwoHanderChargeAttack)
            {
                player.moveSpeed *= (LeftClickTimer * 0.01f - 0.75f);
                player.maxRunSpeed *= (LeftClickTimer * 0.01f - 0.75f);
                player.runAcceleration *= (LeftClickTimer * 0.01f - 0.75f);
            }
        }
        public void ResetTwoHandedEffects()
        {
            if (player.controlUseItem)
            {
                if (LeftClickTimer < 240)
                    LeftClickTimer++;
            }
            else
            {
                TwoHanderChargeAttack = false;
                LeftClickTimer = 0;
            }
            if (IsSwinging && LeftClickTimer >= 20 && !WindedBack)
            {
                WindedBack = false;
                TwoHanderChargeAttack = true;
                IsSwinging = false;
                DownwardsSlash = false;
            }
            if (IsSwinging && SwingRotation < 4.5f)
            {
                if (SwingRotation > 4.5f)
                    SwingRotation = 4.5f;
                if (!WindedBack)
                {
                    if (SwingRotation > -0.4f)
                        SwingRotation -= 0.02f;
                    else
                        WindedBack = true;
                }
                else if (SwingRotation < 0.2f)
                {
                    SwingRotation += 0.05f;
                }
                else
                {
                    SwingRotation *= 1.25f;
                }
                if (SwingRotation > 3.0f)
                {
                    Projectile.NewProjectile(player.Center, new Vector2(0.5f * player.direction, -0.5f), ModContent.ProjectileType<TwoHandedWeaponHitbox>(), player.HeldItem.damage, player.HeldItem.knockBack * 2f, player.whoAmI);
                    Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 71);
                }
            }
            else if (DownwardsSlash && SwingRotation > 2.0f)
            {
                IsSlashReversed = true;
                WaitTimer = 2.4f;
                if (!WindedBack)
                {
                    if (SwingRotation < 4.68f)
                        SwingRotation += 0.01f;
                    else
                        WindedBack = true;
                }
                else
                {
                    SwingRotation *= 0.875f;
                    if (SwingRotation <= 2.0f)
                    {
                        Projectile.NewProjectile(player.Center, new Vector2(1 * player.direction, -0.5f), ModContent.ProjectileType<TwoHandedWeaponHitbox>(), player.HeldItem.damage, player.HeldItem.knockBack * 1.5f, player.whoAmI);
                        Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 71);
                    }
                }
            }
            else if (WaitTimer <= 0.0f && !TwoHanderChargeAttack)
            {
                IsSlashReversed = false;
                DownwardsSlash = false;
                WindedBack = false;
                IsSwinging = false;
                if (SwingRotation > 0.2f)
                {
                    if (SwingRotation > 4.45f)
                        SwingRotation *= 0.999f;
                    else
                        SwingRotation *= 0.95f;
                }
                else
                    SwingRotation = 0.0f;
            }
            if (!HoldsTwoHander)
            {
                HasIgnitedStick = false;
                IsSlashReversed = false;
                DownwardsSlash = false;
                WindedBack = false;
                IsSwinging = false;
                SwingRotation = 0.0f;
            }
            if (WaitTimer > 0.0f)
            {
                if (WaitTimer < 2.4f)
                    DownwardsSlash = false;
                WaitTimer -= 0.1f;
            }
            if(HoldsTwoHander)
            {
                TwoHandedWeapon weapon = player.HeldItem.modItem as TwoHandedWeapon;

                if(Main.myPlayer == player.whoAmI)
                    weapon?.DrawEffects(player);
            }
            if (TwoHanderChargeAttack)
            {
                if(LeftClickTimer == 20)
                    Projectile.NewProjectile(player.Center, new Vector2(1 * player.direction, -0.5f), ModContent.ProjectileType<TwoHandedWeaponHitbox>(), player.HeldItem.damage, player.HeldItem.knockBack * 1.5f, Main.myPlayer);
                if (SwingRotation <= -6.28)
                    SwingRotation = 0.0f;
                SwingRotation -= LeftClickTimer * 0.001f;
                _exhaustedTimer = 30;
                SpendRate += 0.18f;
                IsSlashReversed = true;
            }
            if (Exhausted)
                TwoHanderChargeAttack = false;
        }
        public void ModifyTwoHandedLayers(List<PlayerLayer> layers)
        {
            int armIndex = layers.FindIndex(x => x.Name.Equals("Arms"));
            if (HoldsTwoHander)
            {
                layers.Insert(armIndex, TLoZDrawLayers.Instance.twoHandedWeaponLayer);
                if (TwoHanderChargeAttack)
                {
                    player.hairFrame.Y =
                        player.headFrame.Y =
                        player.bodyFrame.Y = 3 * 56;
                }
                else
                {
                    if (SwingRotation < 0.7f)
                    {
                        player.hairFrame.Y =
                            player.headFrame.Y =
                                player.bodyFrame.Y = 4 * 56;
                    }
                    else if (SwingRotation < 2.4f)
                    {
                        player.hairFrame.Y =
                            player.headFrame.Y =
                            player.bodyFrame.Y = 3 * 56;
                    }
                    else if (SwingRotation < 4.0f)
                    {
                        player.hairFrame.Y =
                            player.headFrame.Y =
                        player.bodyFrame.Y = 2 * 56;
                    }
                    else
                    {
                        player.hairFrame.Y =
                            player.headFrame.Y =
                        player.bodyFrame.Y = 1 * 56;
                    }
                }
            }
        }

        public bool HoldsTwoHander
        {
            get
            {
                if (player.HeldItem.modItem == null || Paragliding || HasBomb)
                    return false;

                return player.HeldItem.modItem.GetType().IsSubclassOf(typeof(TwoHandedWeapon));
            }
        }

        public float SwingRotation { get;  set; }

        public bool IsSwinging { get; set; }

        public bool WindedBack { get; set; }

        public bool DownwardsSlash { get; set; }

        public float WaitTimer { get; private set; }

        public bool IsSlashReversed { get; set; }

        public bool HasIgnitedStick { get; set; }

        public bool TwoHanderChargeAttack { get; set; }

        public int LeftClickTimer { get; private set; }
    }
}
