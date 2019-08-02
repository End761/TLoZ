namespace TLoZ.Items.Weapons.NoGroup
{
    public class BiggoronKek : TwoHandedWeapon
    {
        public BiggoronKek() : base("Biggoron's Sword", "This one doesn't break, now does it?", 40, 40, 10000, 0, 4)
        {
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.melee = true;
            item.damage = 90;
            item.useStyle = 1;
        }
    }
}
