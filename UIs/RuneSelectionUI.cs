using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Microsoft.Xna.Framework;

namespace TLoZ.UIs
{
    public class RuneSelectionUI : UIState
    {
        public Texture2D[] RuneTextures;
        public string[] RuneNames;
        public override void OnInitialize()
        {
            RuneTextures = new[]
            {
                TLoZTextures.UI_StasisRune,
                TLoZTextures.UI_BombRune2,
                TLoZTextures.UI_BombRune1,
                TLoZTextures.UI_MagnesisRune,
                TLoZTextures.UI_CryonisRune
            };
            RuneNames = new[]
            {
                "Stasis",
                "Bomb( Round )",
                "Bomb( Square )",
                "Magnesis",
                "Cryonis"
            };
        }
        public override void OnDeactivate()
        {
            RuneTextures = null;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            TLoZPlayer zPlayer = TLoZPlayer.Get(Main.LocalPlayer);
            if(zPlayer.IsSelectingRune)
            {
                Vector2 position = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - 120);
                float decreasedScale = 0.75f;
                int indexMinusOne = zPlayer.SelectedRune - 1 < 0 ? 4 : zPlayer.SelectedRune - 1;
                int indexPlusOne = zPlayer.SelectedRune + 1 > 4 ? 0 : zPlayer.SelectedRune + 1;
                Utils.DrawBorderString(spriteBatch, RuneNames[zPlayer.SelectedRune], position + new Vector2(-30, 35), Color.White);
                spriteBatch.Draw(RuneTextures[zPlayer.SelectedRune], position, null, Color.White, 0f, new Vector2(32, 32), 1f, SpriteEffects.None, 1f);
                spriteBatch.Draw(RuneTextures[indexMinusOne], position - new Vector2(64, 0), null, Color.White, 0f, new Vector2(32, 32), decreasedScale, SpriteEffects.None, 1f);
                spriteBatch.Draw(RuneTextures[indexPlusOne], position + new Vector2(64, 0), null, Color.White, 0f, new Vector2(32, 32), decreasedScale, SpriteEffects.None, 1f);
            }
        }
    }
}