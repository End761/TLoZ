using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using TLoZ.Players;
using TLoZ.Runes;

namespace TLoZ.UIs
{
    public class RuneSelectionUI : UIState
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            TLoZPlayer tlozPlayer = TLoZPlayer.Get(Main.LocalPlayer);

            if (!tlozPlayer.IsSelectingRune) return;

            Vector2 position = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - 120);

            if (tlozPlayer.UnlockedRunes.Count == 0)
            {
                Utils.DrawBorderString(spriteBatch, "You haven't unlocked any runes!", position, Color.White);
                return;
            }

            float decreasedScale = 0.75f;

            Rune
                nextRune = RuneManager.Instance.GetNext(tlozPlayer.SelectedRune),
                previousRune = RuneManager.Instance.GetPrevious(tlozPlayer.SelectedRune);

            Utils.DrawBorderString(spriteBatch, tlozPlayer.SelectedRune.DisplayName, position + new Vector2(-30, 35), Color.White);

            spriteBatch.Draw(tlozPlayer.SelectedRune.RuneTexture, position, null, Color.White, 0f, new Vector2(32, 32), 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(previousRune.RuneTexture, position - new Vector2(64, 0), null, Color.White, 0f, new Vector2(32, 32), decreasedScale, SpriteEffects.None, 1f);
            spriteBatch.Draw(nextRune.RuneTexture, position + new Vector2(64, 0), null, Color.White, 0f, new Vector2(32, 32), decreasedScale, SpriteEffects.None, 1f);
        }
    }
}