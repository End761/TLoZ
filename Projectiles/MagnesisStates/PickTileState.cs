using Microsoft.Xna.Framework;
using Terraria;
using TLoZ.Projectiles.Runes;

namespace TLoZ.Projectiles.MagnesisStates
{
    public class PickTileState : MagnesisState
    {
        public override void Execute(Projectile projectile)
        {

            int x = (int)(projectile.Center.X / 16);
            int y = (int)(projectile.Center.Y / 16);

            PickedUpTile upTile = (PickedUpTile)projectile.modProjectile;

            Tile tile = Main.tile[x, y];

            upTile.tileTextureID = tile.type;
            upTile.tileFrame = new Vector2((int)tile.frameX, (int)tile.frameY);

            WorldGen.KillTile(x, y, false, false, true);

            upTile.nextState = new MoveToCursorState();
        }
    }
}
