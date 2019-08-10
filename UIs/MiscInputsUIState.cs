using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using TLoZ.Players;
using TLoZ.Projectiles.Runes;
using TLoZ.Runes;

namespace TLoZ.UIs
{
    public class MiscInputsUIState : UIState
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            TLoZPlayer loZPlayer = TLoZPlayer.Get(player);

            if (TLoZMod.loZClientConfig.disableTips)
                return;

            if (loZPlayer.IsSelectingRune)
                return;

            Vector2 position = new Vector2(Main.screenWidth / 2 - 90, Main.screenHeight - 300);

            if(player.HeldItem.type == TLoZMod.Instance.ItemType("SheikahSlate") && loZPlayer.SelectedRune is StasisRune)
            {
                Helpers.DrawInputButtonMouse(spriteBatch, position, 1, "Put to stasis. (Hover mouse over target)");
            }

            if (loZPlayer.SelectedRune is BombSquareRune || loZPlayer.SelectedRune is BombRoundRune)
            {
                if (loZPlayer.HasBomb)
                {
                    Helpers.DrawInputButtonKeyboard("S", spriteBatch, position - new Vector2(80, -100), " + ");
                    Helpers.DrawInputButtonMouse(spriteBatch, position + new Vector2(0, 80), 1, " to put Bomb down.");
                    Helpers.DrawInputButtonMouse(spriteBatch, position, 1, "Throw Bomb.");
                }

                if (!loZPlayer.HasBomb )
                {
                    if (!loZPlayer.isNearBomb && loZPlayer.SelectedRune is BombRoundRune && player.ownedProjectileCounts[TLoZMod.Instance.ProjectileType<BombRound>()] > 0)
                        Helpers.DrawInputButtonMouse(spriteBatch, position, 2, "Detonate Bomb( Round ).");

                    else if(!loZPlayer.isNearBomb && loZPlayer.SelectedRune is BombSquareRune && player.ownedProjectileCounts[TLoZMod.Instance.ProjectileType<BombSquare>()] > 0)
                        Helpers.DrawInputButtonMouse(spriteBatch, position, 2, "Detonate Bomb( Square ).");

                    else if(loZPlayer.isNearBomb)
                        Helpers.DrawInputButtonMouse(spriteBatch, position, 2, "Pick up Bomb(Hover over it with mouse)");
                }
            }
        } 
    }
}
