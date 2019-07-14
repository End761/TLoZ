using TLoZ.Runes;

namespace TLoZ.Projectiles.Runes
{
    public class BombSquare : BombBase
    {
        public override void AI()
        {
            base.AI();
            if (projectile.velocity.Y == 0)
                projectile.velocity.X = 0;
        }
        public override bool RequiredRune => TLoZPlayer.SelectedRune is BombSquareRune;
    }
}