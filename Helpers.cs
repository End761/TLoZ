using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using TLoZ.Dusts;

namespace TLoZ
{
    public static class Helpers
    {
        /// <summary>
        /// Gives you a Vector2 that represents a direction from start to mouse position.
        /// Useful for something like shooting projectiles towards mouse inside ModPlayer class;
        /// </summary>
        /// <param name="start">Position from which we need to get the direction</param>
        /// <param name="speed">Acts as "offset" or the speed at which Vector2 will move towards Mouse</param>
        /// <returns></returns>
        public static Vector2 DirectToMouse(Vector2 start, float speed = 1.0f) => (Main.MouseWorld - start).SafeNormalize(-Vector2.UnitY) * speed;

        public static Vector2 DirectToPosition(Vector2 start, Vector2 end, float speed = 1.0f) => (end - start).SafeNormalize(-Vector2.UnitY) * speed;

        public static float GetPercent(int currentValue, int maxValue, int output) => (currentValue * output) / maxValue;

        public static void CircleDust(Vector2 pos, Vector2 vel, int dustID, float width = 2, float height = 8, float scale = 1.55f, float count = 25.0f)
        {
            for (int k = 0; (double)k < (double)count; k++)
            {
                Vector2 vector2 = (Vector2.UnitX * 0.0f + -Vector2.UnitY.RotatedBy((double)k * (6.22 / (double)count), new Vector2()) * new Vector2(width, height)).RotatedBy((double)vel.ToRotation(), new Vector2());
                int dust = Dust.NewDust(pos - new Vector2(0f, 4f), 1, 1, dustID, 0f, 0f, 200, Scale: scale);
                Main.dust[dust].scale = scale;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].position = pos + vector2;
                Main.dust[dust].velocity = vel * 0.0f + vector2.SafeNormalize(Vector2.UnitY) * 1.0f;
            }
        }

        public static void StartShader(SpriteBatch spriteBatch, Effect shader = null)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, shader, Main.GameViewMatrix.ZoomMatrix);
        }

        public static void EndShader(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static void DrawInputButtonKeyboard(string buttonName, SpriteBatch spriteBatch, Vector2 position, string description = "")
        {
            spriteBatch.Draw(TLoZTextures.UIKeyboardInput, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            Utils.DrawBorderString(spriteBatch, buttonName, position + new Vector2(16, 6), Color.Yellow, 1.5f);
            Utils.DrawBorderString(spriteBatch, description, position + new Vector2(50, 10), Color.Yellow);
        }
        public static void DrawInputButtonMouse(SpriteBatch spriteBatch, Vector2 position, int frame = 0, string description = "")
        {
            spriteBatch.Draw(TLoZTextures.UIMouseInput, position, new Rectangle(0, 72 * frame, 48, 72), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            Utils.DrawBorderString(spriteBatch, description, position + new Vector2(50, 36), Color.Yellow);
        }

        public static void CreateGeneralUseDust(int count, Vector2 position, Color color = new Color())
        {
            for (int i = 0; i < count + 1; i++)
            {
                int dust = Dust.NewDust(position, 0, 0, ModContent.DustType<GeneralUseDust>());
                Main.dust[dust].color = color == new Color() ? Color.White : color;
            }
        }

        public static Vector2 PivotPoint(Vector2 position, float offset_x, float offset_y, float angle)
        {
            float new_x = position.X + (float)Math.Cos(angle) * offset_x + (float)Math.Cos(angle + (float)Math.PI / 2) * offset_y;
            float new_y = position.Y + (float)Math.Sin(angle) * offset_x + (float)Math.Sin(angle + (float)Math.PI / 2) * offset_y;
            return new Vector2(new_x, new_y);
        }

        public static void DrawLine(Vector2 A, Vector2 B, Texture2D middle, Texture2D end, SpriteBatch spriteBatch, Color color, float lineThickness = 4)
        {
            Vector2 tangent = B - A;
            float rotation = (float)Math.Atan2(tangent.Y, tangent.X);

            Vector2 capOrigin = new Vector2(end.Width / 2, end.Height / 2f);
            Vector2 middleOrigin = new Vector2(0, middle.Height / 2f);
            Vector2 middleScale = new Vector2(tangent.Length(), lineThickness);
            spriteBatch.Draw(middle, A - Main.screenPosition, null, color, rotation, middleOrigin, middleScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(end, B - Main.screenPosition, null, color, rotation + MathHelper.Pi, capOrigin, 1f, SpriteEffects.None, 0f);
        }
    }
}
