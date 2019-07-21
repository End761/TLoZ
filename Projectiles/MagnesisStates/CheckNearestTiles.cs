using Microsoft.Xna.Framework;
using Terraria;
using TLoZ.Projectiles.Runes;

namespace TLoZ.Projectiles.MagnesisStates
{
    public class CheckNearestTiles : MagnesisState
    {
        public override void Execute(Projectile projectile)
        {
            int x = (int)(projectile.Center.X);
            int y = (int)(projectile.Center.Y);
            Player player = Main.player[projectile.owner];
            int proj = TLoZ.Instance.ProjectileType<PickedUpTile>();

            int val = 16;
            if (WorldGen.SolidTile(Main.tile[x / 16 + 1, y / 16]) && player.ownedProjectileCounts[proj] <= 0)
                Projectile.NewProjectile(new Vector2(x + val, y), Vector2.Zero, proj, 0, 0, player.whoAmI);

            if (WorldGen.SolidTile(Main.tile[x / 16 - 1, y / 16]) && player.ownedProjectileCounts[proj] <= 0)
                Projectile.NewProjectile(new Vector2(x - val, y), Vector2.Zero, proj, 0, 0, player.whoAmI);

            if (WorldGen.SolidTile(Main.tile[x / 16, y / 16 + 1]) && player.ownedProjectileCounts[proj] <= 0)
                Projectile.NewProjectile(new Vector2(x, y - val), Vector2.Zero, proj, 0, 0, player.whoAmI);

            if (WorldGen.SolidTile(Main.tile[x / 16, y / 16 + 1]) && player.ownedProjectileCounts[proj] <= 0)
                Projectile.NewProjectile(new Vector2(x, y + val), Vector2.Zero, proj, 0, 0, player.whoAmI);


            PickedUpTile upTile = (PickedUpTile)projectile.modProjectile;
            upTile.nextState = new PickTileState();
        }
    }
}
