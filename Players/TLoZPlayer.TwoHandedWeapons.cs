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
        public void ResetTwoHandedEffects()
        {
            if (swingRotation > 4.5f)
                swingRotation = 4.5f;
            if (isSwinging && swingRotation < 4.5f)
            {
                if (!windedBack)
                {
                    if (swingRotation > -0.4f)
                        swingRotation -= 0.02f;
                    else
                        windedBack = true;
                }
                else if(swingRotation < 0.2f)
                {
                    swingRotation += 0.05f;
                }
                else
                {
                    swingRotation *= 1.25f;
                }
                if (swingRotation > 3.0f)
                {
                    Projectile.NewProjectile(player.Center, Helpers.DirectToMouse(player.Center), mod.ProjectileType<TwoHandedWeaponHitbox>(), player.HeldItem.damage, player.HeldItem.knockBack * 2f, player.whoAmI);
                    Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 71);
                }
            }
            else
            {
                windedBack = false;
                isSwinging = false;
                if (swingRotation > 0.2f)
                {
                    if (swingRotation > 4.35f)
                        swingRotation *= 0.999f;
                    else
                        swingRotation *= 0.95f;
                }
                else
                    swingRotation = 0.0f;
            }
            if(!HoldsTwoHander)
            {
                windedBack = false;
                isSwinging = false;
                swingRotation = 0.0f;
            }
        }

        public void ModifyTwoHandedLayers(List<PlayerLayer> layers)
        {
            int armIndex = layers.FindIndex(x => x.Name.Equals("Arms"));
            if (HoldsTwoHander)
            {
                layers.Insert(armIndex, TLoZDrawLayers.Instance.twoHandedWeaponLayer);
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

        public bool HoldsTwoHander
        {
            get
            {
                if (player.HeldItem.modItem == null || usesParaglider || HasBomb)
                    return false;

                return player.HeldItem.modItem.GetType().IsSubclassOf(typeof(TwoHandedWeapon));
            }
        }
    }
}
