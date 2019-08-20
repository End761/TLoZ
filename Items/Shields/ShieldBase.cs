using Microsoft.Xna.Framework.Graphics;

namespace TLoZ.Items.Shields
{
    public abstract class ShieldBase : TLoZItem
    {
        protected ShieldBase(string displayName, string tooltip, int width, int height, Texture2D equipTexture, int value = 0, int defense = 0, int rarity = 0) : base(displayName, tooltip, width, height, value, defense, rarity)
        {
            EquipedTexture = equipTexture;
        }

        public Texture2D EquipedTexture { get; }
    }   
}
