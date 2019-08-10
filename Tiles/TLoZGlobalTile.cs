using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TLoZ.Players;
using TLoZ.Runes;

namespace TLoZ.Tiles
{
    public class TLoZGlobalTile : GlobalTile
    {
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            TLoZPlayer tloZPlayer = TLoZPlayer.Get(Main.LocalPlayer);

            if (tloZPlayer.Holds(TLoZMod.Instance.ItemType("SheikahSlate")) && tloZPlayer.SelectedRune is MagnesisRune && MagnesisRune.magnesisWhiteList.Contains(type))
            {
                spriteBatch.Draw(Main.tileTexture[type], new Vector2(i, j) * 16f + new Vector2(192) - Main.screenPosition, new Rectangle(Main.tile[i, j].frameX, Main.tile[i, j].frameY, 16, 16), Color.Yellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
        }
    }
}
