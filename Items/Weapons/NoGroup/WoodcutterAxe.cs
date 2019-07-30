namespace TLoZ.Items.Weapons.NoGroup
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
    }
}
