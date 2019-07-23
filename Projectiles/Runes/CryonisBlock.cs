using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace TLoZ.Projectiles.Runes
{
    public class CryonisBlock : TLoZProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (MouseOver)
            {
                Helpers.StartShader(spriteBatch);
                GameShaders.Armor.Apply(GameShaders.Armor.GetShaderIdFromItemId(ItemID.PixieDye), projectile);
            }
            return false;
        }

        public override bool PreAI()
        {
            if (_initialPosition == Vector2.Zero)
                _initialPosition = projectile.position;
            projectile.position = _initialPosition + new Vector2(0, 62 - (Height + 6));
            return base.PreAI();
        }

        public override void AI()
        {
            projectile.timeLeft = 2;

            if (Height < 56)
            {
                for (int i = 0; i < 5; i++)
                {
                    int dust = Dust.NewDust(projectile.position, 50, 0, DustID.Ice);
                    Main.dust[dust].velocity *= 0.2f;
                    Main.dust[dust].noGravity = true;
                }
                Height *= 1.1f;
            }

            if (Main.rand.Next(10) == 0)
            {
                int dust = Dust.NewDust(projectile.position, 50, 50, DustID.Ice);
                Main.dust[dust].velocity *= 0.2f;
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].noGravity = true;
            }

            projectile.height = (int)Height + 6;


            if (_existanceTimer < 15)
                _existanceTimer++;

            if (_existanceTimer >= 15 && Owner.controlUseTile && MouseOver)
            {
                projectile.Kill();
            }
        }
        public override bool PreKill(int timeLeft)
        {
            for (int i = 0; i < 50; i++)
            {
                int dust = Dust.NewDust(projectile.position, 50, 62, DustID.Ice);
                Main.dust[dust].velocity *= 0.4f;
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].noGravity = true;
            }
            Main.PlaySound(13);
            return base.PreKill(timeLeft);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position /*+ new Vector2(0, 56 - Height)*/- Main.screenPosition, new Rectangle(0, 56 - (int)Height, 50, (int)Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position /*+ new Vector2(0, 56 - Height)*/ - Main.screenPosition, new Rectangle(0, 0, 50, 4), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position + new Vector2(0, Height) - Main.screenPosition, new Rectangle(0, 58, 50, 4), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            Helpers.EndShader(spriteBatch);
        }

        public float Height
        {
            get { return projectile.ai[0]; }
            private set { projectile.ai[0] = value; if (projectile.ai[0] > 56) projectile.ai[0] = 56; }
        }

        public bool MouseOver => projectile.Hitbox.Contains(Main.MouseWorld.ToPoint());

        private Vector2 _initialPosition;

        private int _existanceTimer;
    }
}
