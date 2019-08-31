using Microsoft.Xna.Framework.Graphics;

namespace TLoZ.Items.Shields
{
    public abstract class ShieldBase : TLoZItem
    {
        protected ShieldBase(string displayName, string tooltip, int width, int height, string equipTexture, int value = 0, int defense = 0, int rarity = 0) : base(displayName, tooltip, width, height, value, defense, rarity)
        {
            EquipedTexturePath = equipTexture;
        }

        public string EquipedTexturePath { get; }
    }   
}
