using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TLoZ.Projectiles.Hostile;
using System;

namespace TLoZ.NPCs.Minibosses.Guardian
{
    public class Guardian : TLoZNpc
    {
        public Guardian() : base("Guardian", true, 1000, 100, 60, false)
        {
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.knockBackResist = -1f;
            npc.ai[2] = 6.0f;
            _hasResetAI3 = false;
            _died = false;
            isActive = false;
        }
        public override void AI()
        {
            if (!_died)
            {
                if (Target.active && !Target.dead && Vector2.Distance(npc.Center, Target.Center) <= 400)
                    isActive = true;
                else if (!Target.active || Target.dead || Vector2.Distance(npc.Center, Target.Center) >= 2048)
                    isActive = false;
                if (isActive)
                {
                    _timeSinceLastActivation = 0;
                    if (npc.ai[0] < 1.4f)
                        npc.ai[0] += 0.02f;
                    else if (npc.ai[1] < 3.0f)
                        npc.ai[1] += 0.02f;

                    if(npc.ai[1] >= 1.0f && npc.ai[1] < 1.02f)
                    {
                        Helpers.CircleDust(npc.Center - new Vector2(-1, 48), Vector2.Zero, DustID.AncientLight, 6, 6);
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Guardian_Boot"));
                    }


                    if (npc.ai[2] > 0.0f && npc.ai[1] >= 3.0f)
                    {
                        npc.ai[2] -= Main.bloodMoon ? 0.08f : 0.04f;
                    }
                }
                else
                {
                    _timeSinceLastActivation++;
                    if (npc.ai[1] > 0.0f)
                        npc.ai[1] -= 0.02f;
                    else if (npc.ai[0] > 0.0f)
                        npc.ai[0] -= 0.02f;
                    npc.ai[3] = 0;
                    npc.ai[2] = 9.0f;
                }
                if (npc.ai[2] <= 0.0f)
                {
                    if(npc.ai[3] > 50 && npc.ai[3] < 52)
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/GuardianShoot"));
                    }
                    if (++npc.ai[3] > 60f)
                    {
                        Vector2 laserPos = npc.Center - new Vector2(-1, 48).RotatedBy(npc.rotation);
                        Helpers.CircleDust(laserPos, Vector2.Zero, DustID.AncientLight, 12, 12);
                        int laser = Projectile.NewProjectile(laserPos, Helpers.DirectToPosition(laserPos, _shootPos, 16f), mod.ProjectileType<GuardianLaser>(), 60, 0f, Main.myPlayer);
                        Main.projectile[laser].ai[0] = npc.whoAmI;
                        Main.projectile[laser].ai[1] = 1f;
                        npc.ai[2] = 9.6f;
                        npc.ai[3] = 0.0f;
                        _noLaserTimer = 60f;
                        _focusTimer = 0;
                    }
                }
                if (npc.ai[3] < 45f)
                {
                    _shootPos = Target.Center + Target.velocity / 8f - new Vector2(0, 6);
                }
                if (_noLaserTimer > 0f)
                {
                    _noLaserTimer -= 1f;
                }
            }
            else
            {
                if (!_hasResetAI3)
                {
                    npc.ai[2] = 0;
                    npc.ai[3] = 0f;
                    _hasResetAI3 = true;
                }
                if(++npc.ai[2] > 60f)
                    npc.ai[3] += 0.01f;
                float[] chooseOne = new float[] { 0.001f, 0.001f, 0.5f, 0.5f };
                npc.ai[0] = Main.rand.Next(chooseOne);
                if (npc.ai[3] >= 0.45f)
                {
                    npc.life = 0;
                    npc.checkDead();
                }
            }
            int timeRequired = npc.ai[2] <= 2.0f ? 14 : 4;
            if (_noLaserTimer <= 0 && ++_focusTimer >= timeRequired && npc.ai[1] >= 3.0f && npc.ai[3] < 45 && !_died)
            {
                if (npc.ai[2] <= 2.0f)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/GuardianScan2"));
                }
                else
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/GuardianScan1"));
                }

                _focusTimer = 0;
            }
            if (npc.ai[2] > 2.0f && npc.ai[2] < 2.1f)
                _focusTimer = 18;
            if (!_died)
                foreach (Projectile proj in Main.projectile)
                {
                    if (!proj.ranged || !proj.active || !proj.friendly)
                        continue;
                    for (int i = 0; i < 1 + proj.extraUpdates; i++)
                    {
                        if (Math.Abs(proj.velocity.X) > proj.velocity.Y && new Rectangle(proj.Hitbox.X + (int)(proj.velocity.X * (i + 1)), proj.Hitbox.Y + (int)(proj.velocity.Y * (i + 1)), proj.Hitbox.Width, proj.Hitbox.Height).Intersects(new Rectangle((int)(npc.Center.X - 8), (int)(npc.Center.Y - 60), 16, 16)))
                        {
                            if (!isActive)
                            {

                                _shootPos = Target.Center + Target.velocity / 8f - new Vector2(0, 6);
                                Vector2 laserPos = npc.Center - new Vector2(-1, 48).RotatedBy(npc.rotation);
                                Helpers.CircleDust(laserPos, Vector2.Zero, DustID.AncientLight, 12, 12);
                                int laser = Projectile.NewProjectile(laserPos, Helpers.DirectToPosition(laserPos, _shootPos, 16f), mod.ProjectileType<GuardianLaser>(), 60, 0f, Main.myPlayer);
                                Main.projectile[laser].ai[0] = npc.whoAmI;
                                Main.projectile[laser].ai[1] = 1f;
                            }
                            isActive = true;
                            if (_noStunTimer <= 0)
                            {
                                npc.ai[2] = 14.6f;
                                npc.ai[3] = 0.0f;
                                _noLaserTimer = 120f;
                                _focusTimer = 0;
                                _noStunTimer = 560;
                            }
                            npc.StrikeNPC((int)(proj.damage * 1.25f), 0f, 0, false, false, true);

                            Helpers.CircleDust(npc.Center - new Vector2(-1, 48), Vector2.Zero, DustID.AncientLight, 8, 8);
                            proj.Kill();
                        }
                    }
                }
            if (_noStunTimer > 0)
                _noStunTimer--;
        }
        public override bool CheckActive()
        {
            if (_timeSinceLastActivation > 18000)
                return true;
            return false;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int numberOfGuardians = 0;
            bool farEnough = true;
            foreach(NPC npc in Main.npc)
            {
                if (!npc.active)
                    continue;
                if (npc.type == mod.NPCType<Guardian>())
                {
                    numberOfGuardians += 1;
                    if (Vector2.Distance(npc.position, new Vector2(spawnInfo.spawnTileX * 16, spawnInfo.spawnTileY * 16)) <= 1024)
                        farEnough = false;
                }
            }
            return farEnough && numberOfGuardians <= 8 && spawnInfo.player.ZoneOverworldHeight ? 0.25f : 0f ;
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            isActive = true;
        }
        public override bool CheckDead()
        {
            if (!_died)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/GuardianDeath"));
                npc.life = npc.lifeMax;
                npc.dontTakeDamage = true;
                _died = true;
                return false;
            }
            Main.NewText("DIEDED");
            Dusts();
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/SheikahExplosion"));
            return true;
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
            Effect deathEffect = TLoZ.Instance.GetEffect("Effects/GuardianDeath");
            deathEffect.Parameters["uColor"].SetValue(new Vector4(npc.ai[3], npc.ai[3], npc.ai[3], 0.8f));
            if (_died)
                Helpers.StartShader(spriteBatch, deathEffect);
            spriteBatch.Draw(baseTexture, npc.Center - new Vector2(0, 30) - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(baseTexture.Width / 2, baseTexture.Height / 2), 1f, SpriteEffects.None, 1f);
            if (_died && _hasResetAI3)
                deathEffect.CurrentTechnique.Passes[0].Apply();

            deathEffect.Parameters["uColor"].SetValue(new Vector4(npc.ai[3], npc.ai[3], npc.ai[3], npc.ai[0]));
            spriteBatch.Draw(glowTexture, npc.Center - new Vector2(0, 30) - Main.screenPosition, null, Color.White * npc.ai[0], npc.rotation, new Vector2(baseTexture.Width / 2, baseTexture.Height / 2), 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(eyeTexture, npc.Center - new Vector2(0, 30) - Main.screenPosition, null, Color.White * (npc.ai[1] >= 1.0f ? 1f : 0f), npc.rotation, new Vector2(baseTexture.Width / 2, baseTexture.Height / 2), 1f, SpriteEffects.None, 1f);
            if (_died && _hasResetAI3)
                deathEffect.CurrentTechnique.Passes[0].Apply();
            Helpers.EndShader(spriteBatch);
            if (npc.ai[1] >= 3.0f && _noLaserTimer <= 0f && !_died && npc.ai[3] <= 45f)
            {
                float x = Main.rand.NextFloat(npc.ai[2] * -1, npc.ai[2]) * 4;
                float y = Main.rand.NextFloat(npc.ai[2] * -1, npc.ai[2]) * 4;
                Vector2 position = _shootPos - new Vector2(0, 6) + new Vector2(x, y);
                Helpers.DrawLine(npc.Center - new Vector2(-1, 48).RotatedBy(npc.rotation), position, mod.GetTexture("Textures/Misc/Laser"), mod.GetTexture("Textures/Misc/LaserEnd"), spriteBatch, npc.ai[2] <= 2.0f ? Color.Red : Color.Crimson, npc.ai[2] <= 2.0f ? 2 : 4);
            }
        }

        private float _noLaserTimer;
        private Vector2 _shootPos;
        private bool _died;
        private bool _hasResetAI3;
        private int _focusTimer;
        public bool isActive;
        private int _timeSinceLastActivation;
        private int _noStunTimer;

        private void Dusts()
        {
            Helpers.CircleDust(npc.Center, Vector2.Zero, DustID.AncientLight, 140, 140, 2.5f, 60);
            for (int i = 0; i < 80; i++)
            {
                int dust = Dust.NewDust(npc.Center, 0, 0, DustID.AncientLight, 0, 0);
                Main.dust[dust].velocity *= 17.5f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 3.5f;
            }
            // Large Smoke Gore spawn
            for (int g = 0; g < 2; g++)
            {
                int goreIndex = Gore.NewGore(new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) - 24f), Vector2.Zero, Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 3.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 3.5f;

                goreIndex = Gore.NewGore(new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) - 24f), Vector2.Zero, Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 3.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 3.5f;

                goreIndex = Gore.NewGore(new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) - 24f), Vector2.Zero, Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 3.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 3.5f;

                goreIndex = Gore.NewGore(new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) - 24f), Vector2.Zero, Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 3.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 3.5f;
            }
        }
    }
}
