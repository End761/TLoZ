using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace TLoZ.Projectiles.Runes
{
    public class PickedUpTile : TLoZProjectile
    {
        public override string Texture => "TLoZ/Textures/Misc/EmptyPixel";
        public override void SetDefaults()
        {
            projectile.ignoreWater = true;
            tileIDs = new int[totalTileCount];
            for(int i = 0; i < totalTileCount; i++)
            {
                tileIDs[i] = -1;
            }
            tilePositions = new Vector2[totalTileCount];
            tileFrames = new Vector2[totalTileCount];
            projectile.tileCollide = false;
            projectile.width = 0;
            projectile.height = 0;
            projectile.hostile = true;
            projectile.friendly = true;
        }
        public override void AI()
        {
            if (_existanceTimer < 15)
                _existanceTimer++;
            if (_existanceTimer >= 15 && Owner.controlUseTile)
            {
                projectile.Kill();
            }
            projectile.timeLeft = 2;
            projectile.velocity = Helpers.DirectToMouse(projectile.position + mousePosOffset, 2);
            for (int i = 0; i < totalTileCount; i++)
            {
                if (tileIDs != null)
                {
                    if (tileIDs[i] != -1)
                        projectile.velocity = Collision.TileCollision(projectile.position + tilePositions[i], projectile.velocity, 15, 15, false, false, 1);
                }
            }
        }
        public override bool CanHitPlayer(Player target)
        {
            for(int i = 0; i < totalTileCount; i++)
            {
                if (tileIDs != null)
                {
                    bool hadCollision = false;
                    if (Collision.CheckAABBvAABBCollision(target.position + target.velocity, target.Hitbox.Size(), Tilesize(i), new Vector2(16)))
                    {
                        hadCollision = true;
                        target.velocity = Vector2.Zero;
                    }
                    if (Collision.CheckAABBvAABBCollision(Tilesize(i) + projectile.velocity, new Vector2(16), target.position, target.Hitbox.Size()))
                    {
                        hadCollision = true;
                        target.velocity = projectile.velocity * 2;
                    }
                    if (hadCollision) return false;
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < totalTileCount; i++)
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
            Helpers.StartShader(spriteBatch);
            GameShaders.Armor.Apply(GameShaders.Armor.GetShaderIdFromItemId(ItemID.PixieDye), projectile);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (!projectile.active || projectile.timeLeft <= 0)
                return;

            if (tileIDs != null && tilePositions != null && tileFrames != null)
            {
                for (int i = 0; i < totalTileCount; i++)
                {
                    if(tileIDs[i] != -1)
                        spriteBatch.Draw(Main.tileTexture[tileIDs[i]], projectile.position + tilePositions[i] - Main.screenPosition, new Rectangle((int)tileFrames[i].X, (int)tileFrames[i].Y, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }
            }
            Helpers.EndShader(spriteBatch);
        }

        private Vector2 Tilesize(int index) => projectile.position + tilePositions[index];

        public int[] tileIDs;
        public Vector2[] tilePositions;
        public Vector2[] tileFrames;
        public Vector2 mousePosOffset;
        private int _existanceTimer;

        public const int totalTileCount = 25;
    }
}
