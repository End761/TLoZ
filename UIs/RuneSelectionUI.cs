using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Microsoft.Xna.Framework;
using TLoZ.Extensions;
using TLoZ.Players;
using TLoZ.Runes;
using TLoZ.Items.Tools;
using Terraria.ModLoader;

namespace TLoZ.UIs
{
    public class RuneSelectionUI : UIState
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            TLoZPlayer tlozPlayer = TLoZPlayer.Get(Main.LocalPlayer);

            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<SheikahSlate>() && tlozPlayer.SelectedRune is StasisRune)
            {
                spriteBatch.Draw(Main.magicPixel, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Yellow * 0.17f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
            if (!tlozPlayer.IsSelectingRune) return;

            Vector2 position = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - 120);

            if (tlozPlayer.UnlockedRunes.Count == 0)
            {
                Utils.DrawBorderString(spriteBatch, "You haven't unlocked any runes!", position, Color.White);
                return;
            }

            float decreasedScale = 0.75f;

            Rune
                nextRune = tlozPlayer.UnlockedRunes.Next(tlozPlayer.SelectedRune),
                previousRune = tlozPlayer.UnlockedRunes.Previous(tlozPlayer.SelectedRune);

            Utils.DrawBorderString(spriteBatch, tlozPlayer.SelectedRune.DisplayName, position + new Vector2(-30, 35), Color.White);
            spriteBatch.Draw(tlozPlayer.SelectedRune.RuneTexture, position, null, Color.White, 0f, new Vector2(32, 32), 1f, SpriteEffects.None, 1f);

            if (tlozPlayer.UnlockedRunes.Count > 1)
            {
                if (!TLoZMod.loZClientConfig.disableTips)
                {
                    Helpers.DrawInputButtonMouse(spriteBatch, position + new Vector2(-20, 60), 4, "Next rune");
                    Helpers.DrawInputButtonMouse(spriteBatch, position + new Vector2(-20, 140), 5, "Previous rune");
                    Helpers.DrawInputButtonKeyboard("D", spriteBatch, position + new Vector2(-100, 85), " or ");
                    Helpers.DrawInputButtonKeyboard("A", spriteBatch, position + new Vector2(-100, 165), " or ");
                }

                spriteBatch.Draw(previousRune.RuneTexture, position - new Vector2(64, 0), null, Color.White, 0f, new Vector2(32, 32), decreasedScale, SpriteEffects.None, 1f);
                spriteBatch.Draw(nextRune.RuneTexture, position + new Vector2(64, 0), null, Color.White, 0f, new Vector2(32, 32), decreasedScale, SpriteEffects.None, 1f);
            }
        }
    }
}