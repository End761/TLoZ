namespace TLoZ.Items.Weapons.Melee.TravelerWeapons
{
    public class TravelerClaymore : TwoHandedWeapon
    {
        public TravelerClaymore() : base("Traveler's Claymore", "", 40, 40, 10000, 0, 4)
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
