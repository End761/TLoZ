using Microsoft.Xna.Framework;
using Terraria;
using TLoZ.Players;
using TLoZ.Projectiles.Misc;

namespace TLoZ.Items.Weapons
{
    public abstract class TwoHandedWeapon : TLoZItem
    {
        protected TwoHandedWeapon(string displayName, string tooltip, int width, int height, int value = 0, int defense = 0, int rarity = 0) : base(displayName, tooltip, width, height, value, defense, rarity)
        {
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.noUseGraphic = true;
            item.useAnimation = 50;
            item.noMelee = true;
            item.knockBack = 4.5f;
        }
        public override bool CanUseItem(Player player)
        {
            TLoZPlayer.Get(player).isSwinging = TLoZPlayer.Get(player).swingRotation == 0.0f ? true : TLoZPlayer.Get(player).isSwinging;

            return base.CanUseItem(player);
        }

    }
}
