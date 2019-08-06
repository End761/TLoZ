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
        private float _opacity;
        public bool haltRegen;
        public double rate;

        private float _lerpAmount;
        private bool _fullLerp;
        private Color _lerpedColor;

        private double _lastBonusStamina;
        private float _spendRate;
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            TLoZPlayer tlozPlayer = TLoZPlayer.Get(player);

            if (tlozPlayer.bonusStamina != _lastBonusStamina)
                _opacity = 10.0f;
            if ((tlozPlayer.Stamina < tlozPlayer.maxStamina || haltRegen || rate > 0) && _opacity < 4.0f)
                _opacity += 0.2f;
            else if (_opacity > 0.0f && !haltRegen)
                _opacity -= 0.1f;

            if (!_fullLerp)
            {
                _lerpAmount += 0.02f;
                if (_lerpAmount >= 0.8f)
                    _fullLerp = true;
            }
            else
            {
                _lerpAmount -= 0.02f;
                if (_lerpAmount <= 0.0f)
                    _fullLerp = false;
            }
            _lerpedColor = Color.Lerp(Color.Orange, Color.Red, _lerpAmount);

            Vector2 position = new Vector2((int)player.Center.X, (int)player.Center.Y + player.gfxOffY)- Main.screenPosition - new Vector2(60, 60);
            Effect mainBar = TLoZ.Instance.GetEffect("Effects/ProgressBar");
            Effect secondBar = TLoZ.Instance.GetEffect("Effects/ProgressBar");
            Effect thirdBar = TLoZ.Instance.GetEffect("Effects/ProgressBar");
            Texture2D mainWheel = TLoZTextures.UIStaminaWheel;
            Texture2D outerWheel = TLoZTextures.UIStaminaOuterWheel;
            _spendRate = MathHelper.Lerp(_spendRate, (float)rate, 0.1f);
            DrawBar(spriteBatch, mainWheel, (float)(tlozPlayer.Stamina / 50), position, tlozPlayer.exhausted ? _lerpedColor : Color.Green * _opacity, 0.6f, mainBar, 1f, tlozPlayer.BonusStamina <= 0);

            DrawBar(spriteBatch, outerWheel, (float)(tlozPlayer.Stamina / 50 - 1), position - new Vector2(6f, 6f) / Main.GameZoomTarget, tlozPlayer.exhausted ? _lerpedColor : Color.Green * _opacity, 0.6f, mainBar, (float)(tlozPlayer.maxStamina / 50 - 1), tlozPlayer.BonusStamina <= 0);
            if (tlozPlayer.BonusStamina > 0)
                DrawBar(spriteBatch, mainWheel, (float)(tlozPlayer.BonusStamina / tlozPlayer.maxStamina), position -  new Vector2(40, -3), tlozPlayer.exhausted ? _lerpedColor : Color.Yellow * _opacity, 0.5f, secondBar, 1f, tlozPlayer.BonusStamina <= tlozPlayer.maxStamina);

            if(tlozPlayer.BonusStamina > tlozPlayer.maxStamina)
                DrawBar(spriteBatch, mainWheel, (float)(tlozPlayer.BonusStamina / tlozPlayer.maxStamina - 1), position - new Vector2(80, -3), tlozPlayer.exhausted ? _lerpedColor : Color.Yellow * _opacity, 0.5f, thirdBar);

            _lastBonusStamina = tlozPlayer.BonusStamina;
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
            spriteBatch.Draw(texture, position, null, Color.Black * _opacity, 0f, Vector2.Zero, scale / Main.GameZoomTarget, SpriteEffects.None, 1f);
            if (canDeplete)
            {
                shader.Parameters["uSaturation"].SetValue(amount);
                shader.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(texture, position, null, rate > 0.0f ? _lerpedColor * 0.95f : Color.Red * _opacity, 0f, Vector2.Zero, scale / Main.GameZoomTarget, SpriteEffects.None, 1f);
            }
            shader.Parameters["uSaturation"].SetValue(amount - _spendRate);
            shader.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, scale / Main.GameZoomTarget, SpriteEffects.None, 1f);

            Helpers.EndShader(spriteBatch);
        }
    }
}
