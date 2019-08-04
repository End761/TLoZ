using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TLoZ.NPCs.Minibosses.Guardian
{
    public class Guardian : TLoZNpc
    {
        public Guardian() : base("Guardian", true, 1000, 134, 108, false)
        {
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.knockBackResist = -1f;
            npc.ai[2] = 14.0f;
        }
        public override void AI()
        {
            if (Target.active && !Target.dead && Vector2.Distance(npc.Center, Main.player[npc.target].Center) < 512)
            {
                if (npc.ai[0] < 1.4f)
                    npc.ai[0] += 0.02f;
                else if (npc.ai[1] < 1.0f)
                    npc.ai[1] += 0.02f;

                if (npc.ai[2] > 0.0f)
                    npc.ai[2] -= 0.04f;
            }
            else
            {
                if (npc.ai[1] > 0.0f)
                    npc.ai[1] -= 0.02f;
                else if (npc.ai[0] > 0.0f)
                    npc.ai[0] -= 0.02f;
                npc.ai[3] = 0;
                npc.ai[2] = 14.0f;
            }
            if(npc.ai[2] <= 0.0f)
            {
                if(++npc.ai[3] > 60f)
                {
                    Vector2 laserPos = npc.Center - new Vector2(0, 16);
                    Projectile.NewProjectile(laserPos, Helpers.DirectToPosition(laserPos, _shootPos, 12f), ProjectileID.DeathLaser, 60, 0f);
                    npc.ai[2] = 14.0f;
                    npc.ai[3] = 0.0f;
                    _noLaserTimer = 60f;
                }
            }
            if (npc.ai[3] < 50f)
                _shootPos = Target.Center - new Vector2(0, 6);
            if(_noLaserTimer > 0f)
            {
                _noLaserTimer -= 1f;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D baseTexture = Main.npcTexture[npc.type];
            Texture2D glowTexture = TLoZTextures.NPCFXGuardianGlow;
            Texture2D eyeTexture = TLoZTextures.NPCFXGuardianEye;
            spriteBatch.Draw(baseTexture, npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(baseTexture.Width / 2, baseTexture.Height / 2), 1f, SpriteEffects.None, 1f);

            spriteBatch.Draw(glowTexture, npc.Center - Main.screenPosition, null, Color.White * npc.ai[0], npc.rotation, new Vector2(baseTexture.Width / 2, baseTexture.Height / 2), 1f, SpriteEffects.None, 1f);

            spriteBatch.Draw(eyeTexture, npc.Center - Main.screenPosition, null, Color.White * npc.ai[1], npc.rotation, new Vector2(baseTexture.Width / 2, baseTexture.Height / 2), 1f, SpriteEffects.None, 1f);

            if (npc.ai[1] >= 1.0f && _noLaserTimer <= 0f)
            {
                float x = Main.rand.NextFloat(npc.ai[2] * -1, npc.ai[2]) * 2;
                float y = Main.rand.NextFloat(npc.ai[2] * -1, npc.ai[2]) * 2;
                Vector2 position = _shootPos - new Vector2(0, 6) + new Vector2(x, y);
                Helpers.DrawLine(npc.Center - new Vector2(0, 16), position, mod.GetTexture("Textures/Misc/Laser"), mod.GetTexture("Textures/Misc/LaserEnd"), spriteBatch, Color.Red);
            }
        }

        private float _noLaserTimer;
        private Vector2 _shootPos;
    }
}
