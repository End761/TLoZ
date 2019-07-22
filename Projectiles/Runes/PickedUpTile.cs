using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TLoZ.Projectiles.Runes
{
    public class PickedUpTile : TLoZProjectile
    {
        public override string Texture => "TLoZ/Textures/Misc/EmptyPixel";
        public override void SetDefaults()
        {
            tileIDs = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            tilePositions = new Vector2[16];
            tileFrames = new Vector2[16];
            projectile.tileCollide = false;
            projectile.width = 16;
            projectile.height = 16;
        }
        public override void AI()
        {
            if (_existanceTimer < 15)
                _existanceTimer++;
            if(_existanceTimer >= 15 && Owner.controlUseTile)
            {
                projectile.Kill();
            }
            if(Owner.Hitbox.Bottom + 1 > projectile.Hitbox.Top && Math.Abs(Owner.Hitbox.X - projectile.Hitbox.X - projectile.width / 4) <= projectile.width /2 && projectile.position.Y > Owner.position.Y)
            {
                Owner.velocity.Y = 0;
                Owner.position.Y = projectile.Hitbox.Top - Owner.height;
            }
            projectile.timeLeft = 2;
            projectile.velocity = Helpers.DirectToMouse(projectile.position);
            projectile.velocity = Collision.TileCollision(projectile.position, projectile.velocity, projectile.width - 1, projectile.height - 1, false, false, 1);
        }
        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 16; i++)
            {
                if(tileIDs[i] != -1)
                WorldGen.PlaceTile((int)(projectile.position.X + tilePositions[i].X) / 16, (int)(projectile.position.Y + tilePositions[i].Y) / 16, tileIDs[i], true, true, -1, 0);
            }
            tileIDs = null;
            tilePositions = null;
            tileFrames = null;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (!projectile.active || projectile.timeLeft <= 0)
                return;

            if (tileIDs != null && tilePositions != null && tileFrames != null)
            {
                for (int i = 0; i < 16; i++)
                {
                    if(tileIDs[i] != -1)
                        spriteBatch.Draw(Main.tileTexture[tileIDs[i]], projectile.position + tilePositions[i] - Main.screenPosition, new Rectangle((int)tileFrames[i].X, (int)tileFrames[i].Y, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }
            }
        }

        public int[] tileIDs;
        public Vector2[] tilePositions;
        public Vector2[] tileFrames;
        private int _existanceTimer;
    }
}
