using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TLoZ.Projectiles.MagnesisStates;

namespace TLoZ.Projectiles.Runes
{
    public class PickedUpTile : TLoZProjectile
    {

        public override string Texture => "TLoZ/Textures/Misc/EmptyPixel";

        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.width = 16;
            projectile.height = 16;
            tileFrame = new Vector2();
        }
        public override bool PreAI()
        {
            if(nextState == null)
            {
                if (projectile.ai[0] == 1)
                    nextState = new CheckNearestTiles();
                else
                    nextState = new PickTileState();
            }
            return base.PreAI();
        }
        public override void AI()
        {
            if (_existanceTimer < 15)
                _existanceTimer++;

            if(Owner.controlUseTile && Owner.itemTime == 0 && _existanceTimer >= 15)
            {
                projectile.Kill();
            }
            projectile.timeLeft = 2;
            nextState.Execute(projectile);
            projectile.velocity = Collision.TileCollision(projectile.position, projectile.velocity, 16, 16, false, false, 1);
            foreach(Projectile proj in Main.projectile)
            {
                if (!proj.active || proj.type != projectile.type)
                    continue;
                if (Collision.TileCollision(projectile.position, projectile.velocity, 16, 16, false, false, 1).X == 0)
                    proj.velocity.X = 0;
                if (Collision.TileCollision(projectile.position, projectile.velocity, 16, 16, false, false, 1).Y == 0)
                    proj.velocity.Y = 0;
            }

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void Kill(int timeLeft)
        {
            int x = (int)projectile.position.X;
            int y = (int)projectile.position.Y;
            WorldGen.PlaceTile(x / 16, y / 16, tileTextureID, true, true, -1, 0);
            Main.tile[x / 16, y / 16].frameX = (short)tileFrame.X;
            Main.tile[x / 16, y / 16].frameY = (short)tileFrame.Y;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(Main.tileTexture[tileTextureID], projectile.position - Main.screenPosition, new Rectangle((int)tileFrame.X, (int)tileFrame.Y, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        public int tileTextureID;
        public Vector2 tileFrame;
        private int _existanceTimer;

        public MagnesisState nextState;
    }
}
