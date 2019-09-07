using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TLoZ.Tiles
{
    public abstract class TLoZTile : ModTile
    {
		private readonly string _name;

        private readonly Color _color;

        private readonly bool _tileSolidTop, _tileFrameImportant, _tileNoAttach, _tileLavaDeath;

        protected TLoZTile(string tileName, Color color, bool solidTop = false, bool frameImportant = false, bool noAttach = false, bool lavaDeath = false)
        {
			_name = tileName;

            _color = color;

            _tileSolidTop = solidTop;
            _tileNoAttach = noAttach;
            _tileLavaDeath = lavaDeath;
            _tileFrameImportant = frameImportant;
        }

		public override void SetDefaults()
		{
            Main.tileSolidTop[Type] = _tileSolidTop;
            Main.tileFrameImportant[Type] = _tileFrameImportant;
            Main.tileNoAttach[Type] = _tileNoAttach;
            Main.tileLavaDeath[Type] = _tileLavaDeath;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault(_name);
            AddMapEntry(_color, name);
		}
    }
}
