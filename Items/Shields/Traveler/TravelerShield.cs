using Microsoft.Xna.Framework.Graphics;

namespace TLoZ.Items.Shields.Traveler
{
    public class TravelerShield : ShieldBase
    {
        public TravelerShield() : base("Traveler's Shield", "", 32, 32, TLoZMod.Instance.GetTexture("Items/Shields/Traveler/TravelerShieldEquiped"), 0, 0, 4)
        {
        }
    }
}
