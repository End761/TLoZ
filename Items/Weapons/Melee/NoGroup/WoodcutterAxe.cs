using Terraria;
using Terraria.ID;

namespace TLoZ.Items.Weapons.Melee.NoGroup
{
    public class WoodcutterAxe : TwoHandedWeapon
    {
        public WoodcutterAxe() : base("Woodcutter's Axe", "", 40, 40, 10000, 0, 4)
        {
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.melee = true;
            item.damage = 24;
            item.useStyle = 1;
        }
        public override void HitboxEffects(Projectile projectile, Player player)
        {
            for (int i = 0; i < projectile.height; i++)
                for (int j = 0; j < projectile.width; j++)
                {
                    Tile tile = Main.tile[(int)(projectile.position.X + j) / 16, (int)(projectile.position.Y + i) / 16];
                    if (tile.type == TileID.Trees || tile.type == TileID.PalmTree)
                        WorldGen.KillTile((int)(projectile.position.X + j) / 16, (int)(projectile.position.Y + i) / 16, false, false);
                }
        }
    }
}
