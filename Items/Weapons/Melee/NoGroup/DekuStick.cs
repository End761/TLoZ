﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using TLoZ.Players;

namespace TLoZ.Items.Weapons.Melee.NoGroup
{
    public class DekuStick : TwoHandedWeapon
    {
        public DekuStick() : base("Deku Stick", "Can be set on fire", 40, 40, 10000, 0, 4)
        {
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.melee = true;
            item.damage = 12;
            item.useStyle = 1;
            item.knockBack = 2.5f;
        }
        public override void OnHitEffects(NPC target, Player player)
        {
            if (TLoZPlayer.Get(player).HasIgnitedStick)
                target.AddBuff(BuffID.OnFire, 180);
        }
        public override void DrawEffects(Player player)
        {
            if (TLoZPlayer.Get(player).HasIgnitedStick)
            {
                float bonusOffset = (TLoZPlayer.Get(player).IsSlashReversed ? player.direction == 1 ? (float)MathHelper.Pi * 0.5f : (float)MathHelper.Pi * 1.5f : 0f) + player.fullRotation;
                int dust = Dust.NewDust(Helpers.PivotPoint(TLoZDrawLayers.Instance.TwoHanderVFX, 50 * player.direction * -1, -56, TLoZDrawLayers.Instance.TwoHanderRotation + bonusOffset), 0, 0, DustID.Fire);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].noGravity = true;
            }
        }
        public override void HitboxEffects(Projectile projectile, Player player)
        {
            for (int i = 0; i < projectile.height; i++)
                for (int j = 0; j < projectile.width; j++)
                {
                    Tile tile = Main.tile[(int)(projectile.position.X + j) / 16, (int)(projectile.position.Y + i) / 16];
                    if (tile.type == TileID.Campfire)
                        TLoZPlayer.Get(player).HasIgnitedStick = true;
                }
        }
    }
}
