using Microsoft.Xna.Framework;
using Terraria;
using TLoZ.Players;

namespace TLoZ.Items.Weapons.Melee.Ancient
{
    public class AncientBladesaw : TwoHandedWeapon
    {
        public AncientBladesaw() : base("Ancient Bladesaw", "Deals massive damage to guardians", 40, 40, 10000, 0, 4)
        {
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.melee = true;
            item.damage = 65;
            item.useStyle = 1;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (target.type == mod.NPCType("Guardian"))
                damage *= 2;
        }
        public override void DrawEffects(Player player)
        {
            float bonusOffset = (TLoZPlayer.Get(player).IsSlashReversed ? player.direction == 1 ? (float)MathHelper.Pi * 0.5f : (float)MathHelper.Pi * 1.5f : 0f) + player.fullRotation;
            Lighting.AddLight(Helpers.PivotPoint(TLoZDrawLayers.Instance.TwoHanderVFX, 25 * player.direction * -1, -31, TLoZDrawLayers.Instance.TwoHanderRotation + bonusOffset), Color.Cyan.ToVector3() * 2);
        }
    }
}
