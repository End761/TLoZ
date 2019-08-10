using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TLoZ.Items.Weapons;
using TLoZ.Projectiles.Misc;

namespace TLoZ.Players
{
    public partial class TLoZPlayer : ModPlayer
    {
        public float swingRotation;
        public bool isSwinging;
        public bool windedBack;
        public bool downwardsSlash;
        private float _waitTimer;
        public bool isSlashReversed;
        public bool hasIgnitedStick;
        public bool twoHanderChargeAttack;
        public int leftClickTimer;
        public void PostUpdateTHWRunSpeeds()
        {
            if(twoHanderChargeAttack)
            {
                player.moveSpeed *= (leftClickTimer * 0.01f - 0.75f);
                player.maxRunSpeed *= (leftClickTimer * 0.01f - 0.75f);
                player.runAcceleration *= (leftClickTimer * 0.01f - 0.75f);
            }
        }
        public void ResetTwoHandedEffects()
        {
            if (player.controlUseItem)
            {
                if (leftClickTimer < 240)
                    leftClickTimer++;
            }
            else
            {
                twoHanderChargeAttack = false;
                leftClickTimer = 0;
            }
            if (isSwinging && leftClickTimer >= 20 && !windedBack)
            {
                windedBack = false;
                twoHanderChargeAttack = true;
                isSwinging = false;
                downwardsSlash = false;
            }
            if (isSwinging && swingRotation < 4.5f)
            {
                if (swingRotation > 4.5f)
                    swingRotation = 4.5f;
                if (!windedBack)
                {
                    if (swingRotation > -0.4f)
                        swingRotation -= 0.02f;
                    else
                        windedBack = true;
                }
                else if (swingRotation < 0.2f)
                {
                    swingRotation += 0.05f;
                }
                else
                {
                    swingRotation *= 1.25f;
                }
                if (swingRotation > 3.0f)
                {
                    Projectile.NewProjectile(player.Center, new Vector2(0.5f * player.direction, -0.5f), mod.ProjectileType<TwoHandedWeaponHitbox>(), player.HeldItem.damage, player.HeldItem.knockBack * 2f, player.whoAmI);
                    Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 71);
                }
            }
            else if (downwardsSlash && swingRotation > 2.0f)
            {
                isSlashReversed = true;
                _waitTimer = 2.4f;
                if (!windedBack)
                {
                    if (swingRotation < 4.68f)
                        swingRotation += 0.01f;
                    else
                        windedBack = true;
                }
                else
                {
                    swingRotation *= 0.875f;
                    if (swingRotation <= 2.0f)
                    {
                        Projectile.NewProjectile(player.Center, new Vector2(1 * player.direction, -0.5f), mod.ProjectileType<TwoHandedWeaponHitbox>(), player.HeldItem.damage, player.HeldItem.knockBack * 1.5f, player.whoAmI);
                        Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 71);
                    }
                }
            }
            else if (_waitTimer <= 0.0f && !twoHanderChargeAttack)
            {
                isSlashReversed = false;
                downwardsSlash = false;
                windedBack = false;
                isSwinging = false;
                if (swingRotation > 0.2f)
                {
                    if (swingRotation > 4.45f)
                        swingRotation *= 0.999f;
                    else
                        swingRotation *= 0.95f;
                }
                else
                    swingRotation = 0.0f;
            }
            if (!HoldsTwoHander)
            {
                hasIgnitedStick = false;
                isSlashReversed = false;
                downwardsSlash = false;
                windedBack = false;
                isSwinging = false;
                swingRotation = 0.0f;
            }
            if (_waitTimer > 0.0f)
            {
                if (_waitTimer < 2.4f)
                    downwardsSlash = false;
                _waitTimer -= 0.1f;
            }
            if(HoldsTwoHander)
            {
                TwoHandedWeapon weapon = player.HeldItem.modItem as TwoHandedWeapon;

                if(Main.myPlayer == player.whoAmI)
                    weapon?.DrawEffects(player);
            }
            if (twoHanderChargeAttack)
            {
                if(leftClickTimer == 20)
                    Projectile.NewProjectile(player.Center, new Vector2(1 * player.direction, -0.5f), mod.ProjectileType<TwoHandedWeaponHitbox>(), player.HeldItem.damage, player.HeldItem.knockBack * 1.5f, Main.myPlayer);
                if (swingRotation <= -6.28)
                    swingRotation = 0.0f;
                swingRotation -= leftClickTimer * 0.001f;
                _exhaustedTimer = 30;
                spendRate += 0.18f;
                isSlashReversed = true;
            }
            if (exhausted)
                twoHanderChargeAttack = false;
        }
        public void ModifyTwoHandedLayers(List<PlayerLayer> layers)
        {
            int armIndex = layers.FindIndex(x => x.Name.Equals("Arms"));
            if (HoldsTwoHander)
            {
                layers.Insert(armIndex, TLoZDrawLayers.Instance.twoHandedWeaponLayer);
                if (twoHanderChargeAttack)
                {
                    player.hairFrame.Y =
                        player.headFrame.Y =
                        player.bodyFrame.Y = 3 * 56;
                }
                else
                {
                    if (swingRotation < 0.7f)
                    {
                        player.hairFrame.Y =
                            player.headFrame.Y =
                                player.bodyFrame.Y = 4 * 56;
                    }
                    else if (swingRotation < 2.4f)
                    {
                        player.hairFrame.Y =
                            player.headFrame.Y =
                            player.bodyFrame.Y = 3 * 56;
                    }
                    else if (swingRotation < 4.0f)
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
    }
}
