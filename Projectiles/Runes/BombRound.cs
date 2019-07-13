namespace TLoZ.Projectiles.Runes
{
    public class BombRound : BombBase
    {
        public override void PostAI()
        {
            if (projectile.ai[1] >= 1)
            {
                projectile.rotation += (projectile.position.X - projectile.oldPosition.X) / 16;
                projectile.velocity.X *= 0.99f;
                if (projectile.velocity.Y == 0)
                    projectile.ai[1] = 2;
                if(projectile.velocity.Y > projectile.oldVelocity.Y && projectile.ai[1] == 2)
                {
                    projectile.velocity.X *= 1.04f;
                }
            }
        }
    }
}
