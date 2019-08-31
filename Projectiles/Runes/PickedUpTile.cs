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
            TileIDs = new int[TOTAL_TILE_COUNT];

            for(int i = 0; i < TOTAL_TILE_COUNT; i++)
            {
                TileIDs[i] = -1;
            }

            TilePositions = new Vector2[TOTAL_TILE_COUNT];
            TileFrames = new Vector2[TOTAL_TILE_COUNT];
            projectile.tileCollide = false;
            projectile.width = 0;
            projectile.height = 0;
            projectile.hostile = true;
            projectile.friendly = true;
        }

        public override void AI()
        {
            if (ExistanceTimer < 15)
                ExistanceTimer++;

            if (ExistanceTimer >= 15 && Owner.controlUseTile)
            {
                projectile.Kill();
            }

            projectile.timeLeft = 2;
            projectile.velocity = Helpers.DirectToMouse(projectile.position + MousePosOffset, 2);

            for (int i = 0; i < TOTAL_TILE_COUNT; i++)
            {
                if (TileIDs != null)
                {
                    if (TileIDs[i] != -1)
                        projectile.velocity = Collision.TileCollision(projectile.position + TilePositions[i], projectile.velocity, 15, 15, false, false, 1);
                }
            }
        }
        public override bool CanHitPlayer(Player target)
        {
            for(int i = 0; i < TOTAL_TILE_COUNT; i++)
            {
                if (TileIDs != null)
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
            for(int i = 0; i < TOTAL_TILE_COUNT; i++)
            {
                if(TileIDs[i] != -1)
                WorldGen.PlaceTile((int)(projectile.position.X + TilePositions[i].X) / 16, (int)(projectile.position.Y + TilePositions[i].Y) / 16, TileIDs[i], true, true, -1, 0);
            }

            TileIDs = null;
            TilePositions = null;
            TileFrames = null;
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

            if (TileIDs != null && TilePositions != null && TileFrames != null)
            {
                for (int i = 0; i < TOTAL_TILE_COUNT; i++)
                {
                    if(TileIDs[i] != -1)
                        spriteBatch.Draw(Main.tileTexture[TileIDs[i]], projectile.position + TilePositions[i] - Main.screenPosition, new Rectangle((int)TileFrames[i].X, (int)TileFrames[i].Y, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }
            }

            Helpers.EndShader(spriteBatch);
        }

        private Vector2 Tilesize(int index) => projectile.position + TilePositions[index];

        public int[] TileIDs { get; private set; }
        public Vector2[] TilePositions { get; set; }
        public Vector2[] TileFrames { get; set; }
        public Vector2 MousePosOffset { get; set; }
        public int ExistanceTimer { get; private set; }

        public const int TOTAL_TILE_COUNT = 25;
    }
}
