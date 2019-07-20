using Terraria.ObjectData;
using Terraria;

using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TLoZ.Tiles.Misc
{
    public class MasterSwordPedestal : TLoZTile
    {
        private bool _pulledMasterSword;

        public MasterSwordPedestal() : base("Pedestal", new Color(200, 200, 200), false, true, true)
        {
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            _pulledMasterSword = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
            TileObjectData.newTile.CoordinateHeights = new[] { 18 };
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (!_pulledMasterSword && Main.tile[i, j].frameX == 0)
                spriteBatch.Draw(mod.GetTexture("Items/Weapons/MasterSword/MasterByLiz"), new Vector2(i * 16f, j * 16f)  + new Vector2(208, 210)- Main.screenPosition, null, Lighting.GetColor(i, j), -((float)Math.PI / 2 + (float)Math.PI / 4), Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 1f);
            return true;
        }
        public override void RightClick(int i, int j)
        {
            _pulledMasterSword = !_pulledMasterSword;
        }
    }
}
