using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.UI;
using TLoZ.Players;

namespace TLoZ.UIs
{
    public class StaminaUIState : UIState
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            TLoZPlayer tlozPlayer = TLoZPlayer.Get(player);

            if (tlozPlayer.BonusStamina != LastBonusStamina)
                Opacity = 10.0f;

            if ((tlozPlayer.Stamina < tlozPlayer.MaxStamina || HaltRegen || Rate > 0) && Opacity < 4.0f)
                Opacity += 0.2f;

            else if (Opacity > 0.0f && !HaltRegen)
                Opacity -= 0.1f;

            if (!FullLerp)
            {
                LerpAmount += 0.02f;

                if (LerpAmount >= 0.8f)
                    FullLerp = true;
            }
            else
            {
                LerpAmount -= 0.02f;

                if (LerpAmount <= 0.0f)
                    FullLerp = false;
            }

            LerpedColor = Color.Lerp(Color.Orange, Color.Red, LerpAmount);

            Vector2 position = new Vector2((int)player.Center.X, (int)player.Center.Y + player.gfxOffY)- Main.screenPosition - new Vector2(60, 60);
            Effect mainBar = TLoZMod.Instance.GetEffect("Effects/ProgressBar");

            Effect secondBar = TLoZMod.Instance.GetEffect("Effects/ProgressBar");

            Effect thirdBar = TLoZMod.Instance.GetEffect("Effects/ProgressBar");

            Texture2D mainWheel = TLoZTextures.UIStaminaWheel;
            Texture2D outerWheel = TLoZTextures.UIStaminaOuterWheel;

            SpendRate = MathHelper.Lerp(SpendRate, (float)Rate, 0.1f);

            DrawBar(spriteBatch, mainWheel, (float)(tlozPlayer.Stamina / 50), position, tlozPlayer.Exhausted ? LerpedColor : Color.Green * Opacity, 0.6f, mainBar, 1f, tlozPlayer.BonusStamina <= 0);

            DrawBar(spriteBatch, outerWheel, (float)(tlozPlayer.Stamina / 50 - 1), position - new Vector2(6f, 6f) / Main.GameZoomTarget, tlozPlayer.Exhausted ? LerpedColor : Color.Green * Opacity, 0.6f, mainBar, (float)(tlozPlayer.MaxStamina / 50 - 1), tlozPlayer.BonusStamina <= 0);

            if (tlozPlayer.BonusStamina > 0)
                DrawBar(spriteBatch, mainWheel, (float)(tlozPlayer.BonusStamina / tlozPlayer.MaxStamina), position -  new Vector2(40, -3), tlozPlayer.Exhausted ? LerpedColor : Color.Yellow * Opacity, 0.5f, secondBar, 1f, tlozPlayer.BonusStamina <= tlozPlayer.MaxStamina);

            if(tlozPlayer.BonusStamina > tlozPlayer.MaxStamina)
                DrawBar(spriteBatch, mainWheel, (float)(tlozPlayer.BonusStamina / tlozPlayer.MaxStamina - 1), position - new Vector2(80, -3), tlozPlayer.Exhausted ? LerpedColor : Color.Yellow * Opacity, 0.5f, thirdBar);

            LastBonusStamina = tlozPlayer.BonusStamina;
        }


        private void DrawBar(SpriteBatch spriteBatch, Texture2D texture, float amount, Vector2 position, Color color, float scale, Effect shader, float bgLength = 1, bool canDeplete = true)
        {
            Helpers.StartShader(spriteBatch, shader);

            shader.Parameters["uSourceRect"].SetValue(new Vector4(0, 0, 64, 64));
            shader.Parameters["uImageSize0"].SetValue(new Vector2(64, 64));
            shader.Parameters["uRotation"].SetValue(3f);
            shader.Parameters["uDirection"].SetValue(-1f);

            shader.Parameters["uSaturation"].SetValue(bgLength);
            shader.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(texture, position, null, Color.Black * Opacity, 0f, Vector2.Zero, scale / Main.GameZoomTarget, SpriteEffects.None, 1f);

            if (canDeplete)
            {
                shader.Parameters["uSaturation"].SetValue(amount);
                shader.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(texture, position, null, Rate > 0.0f ? LerpedColor * 0.95f : Color.Red * Opacity, 0f, Vector2.Zero, scale / Main.GameZoomTarget, SpriteEffects.None, 1f);
            }

            shader.Parameters["uSaturation"].SetValue(amount - SpendRate);
            shader.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, scale / Main.GameZoomTarget, SpriteEffects.None, 1f);

            Helpers.EndShader(spriteBatch);
        }


        public float Opacity { get; private set; }
        public bool HaltRegen { get; set; }
        public double Rate { get; set; }

        public float LerpAmount { get; private set; }
        public bool FullLerp { get; private set; }
        public Color LerpedColor { get; private set; }

        public double LastBonusStamina { get; private set; }
        public float SpendRate { get; private set; }
    }
}
