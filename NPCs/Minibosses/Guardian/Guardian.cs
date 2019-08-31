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
        public Guardian() : base("Guardian", true, 1000, 100, 60, false, 10)
        {
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.aiStyle = -1;
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.knockBackResist = 0f;
            ScanTimer = 6.0f;
            npc.netAlways = true;
            HasResetAI3 = false;
            Died = false;
            IsGuardianActive = false;
        }
        
        public override void AI()
        {
            npc.netUpdate = true;

            npc.netUpdate2 = true;

            npc.TargetClosest();

            if (!Died)
            {
                if (Target.active && !Target.dead && Vector2.Distance(npc.Center, Target.Center) <= 400)
                    IsGuardianActive = true;

                else if (!Target.active || Target.dead || Vector2.Distance(npc.Center, Target.Center) >= 2048)
                    IsGuardianActive = false;

                if (IsGuardianActive)
                {
                    TimeSinceLastActivation = 0;

                    if (BootUpTimer < 1.4f)
                        BootUpTimer += 0.02f;

                    else if (EyeBootUpTimer < 3.0f)
                        EyeBootUpTimer += 0.02f;

                    if(EyeBootUpTimer >= 1.0f && EyeBootUpTimer < 1.02f)
                    {
                        Helpers.CircleDust(npc.Center - new Vector2(-1, 48), Vector2.Zero, DustID.AncientLight, 6, 6);
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Guardian_Boot"));
                    }


                    if (ScanTimer > 0.0f && EyeBootUpTimer >= 3.0f)
                        ScanTimer -= Main.bloodMoon ? 0.08f : 0.04f;
                }
                else
                {
                    TimeSinceLastActivation++;

                    if (EyeBootUpTimer > 0.0f)
                        EyeBootUpTimer -= 0.02f;

                    else if (BootUpTimer > 0.0f)
                        BootUpTimer -= 0.02f;

                    ShotPreparationTimer = 0;
                    ScanTimer = 9.0f;
                }

                if (ScanTimer <= 0.0f)
                {
                    if(ShotPreparationTimer > 50 && ShotPreparationTimer < 52)
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/GuardianShoot"));

                    if (++ShotPreparationTimer > 60f)
                    {
                        Vector2 laserPos = npc.Center - new Vector2(-1, 48).RotatedBy(npc.rotation);
                        Helpers.CircleDust(laserPos, Vector2.Zero, DustID.AncientLight, 12, 12);
                        int laser = Projectile.NewProjectile(laserPos, Helpers.DirectToPosition(laserPos, ShootPos, 16f), mod.ProjectileType<GuardianLaser>(), 60, 0f, Main.myPlayer);
                        Main.projectile[laser].ai[0] = npc.whoAmI;
                        Main.projectile[laser].ai[1] = 1f;
                        ScanTimer = 9.6f;
                        ShotPreparationTimer = 0.0f;
                        NoLaserTimer = 60f;
                        FocusTimer = 0;
                    }
                }

                if (ShotPreparationTimer < 45f)
                    ShootPos = Target.Center + Target.velocity / 8f - new Vector2(0, 6);

                if (NoLaserTimer > 0f)
                    NoLaserTimer -= 1f;
            }
            else
            {
                if (!HasResetAI3)
                {
                    ScanTimer = 0;
                    ShotPreparationTimer = 0f;
                    HasResetAI3 = true;
                }

                if(++ScanTimer > 60f)
                    ShotPreparationTimer += 0.01f;

                float[] chooseOne = new float[] { 0.001f, 0.001f, 0.5f, 0.5f };

                BootUpTimer = Main.rand.Next(chooseOne);

                if (ShotPreparationTimer >= 0.45f)
                {
                    npc.life = 0;
                    npc.checkDead();
                }
            }

            int timeRequired = ScanTimer <= 2.0f ? 14 : 4;

            if (NoLaserTimer <= 0 && ++FocusTimer >= timeRequired && EyeBootUpTimer >= 3.0f && ShotPreparationTimer < 45 && !Died)
            {
                if (ScanTimer <= 2.0f)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/GuardianScan2"));

                else
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/GuardianScan1"));

                FocusTimer = 0;
            }

            if (ScanTimer > 2.0f && ScanTimer < 2.1f)
                FocusTimer = 18;

            if (!Died)
                foreach (Projectile proj in Main.projectile)
                {
                    if (!proj.ranged || !proj.active || !proj.friendly)
                        continue;

                    for (int i = 0; i < 1 + proj.extraUpdates; i++)
                    {
                        if (Math.Abs(proj.velocity.X) > proj.velocity.Y && new Rectangle(proj.Hitbox.X + (int)(proj.velocity.X * (i + 1)), proj.Hitbox.Y + (int)(proj.velocity.Y * (i + 1)), proj.Hitbox.Width, proj.Hitbox.Height).Intersects(new Rectangle((int)(npc.Center.X - 8), (int)(npc.Center.Y - 60), 16, 16)))
                        {
                            if (!IsGuardianActive)
                            {
                                ShootPos = Target.Center + Target.velocity / 8f - new Vector2(0, 6);
                                Vector2 laserPos = npc.Center - new Vector2(-1, 48).RotatedBy(npc.rotation);
                                Helpers.CircleDust(laserPos, Vector2.Zero, DustID.AncientLight, 12, 12);
                                int laser = Projectile.NewProjectile(laserPos, Helpers.DirectToPosition(laserPos, ShootPos, 16f), mod.ProjectileType<GuardianLaser>(), 60, 0f, Main.myPlayer);
                                Main.projectile[laser].ai[0] = npc.whoAmI;
                                Main.projectile[laser].ai[1] = 1f;
                            }

                            IsGuardianActive = true;

                            if (NoStunTimer <= 0)
                            {
                                ScanTimer = 14.6f;
                                ShotPreparationTimer = 0.0f;
                                NoLaserTimer = 120f;
                                FocusTimer = 0;
                                NoStunTimer = 560;
                            }

                            npc.StrikeNPC((int)(proj.damage * 1.25f), 0f, 0, false, false, true);

                            Helpers.CircleDust(npc.Center - new Vector2(-1, 48), Vector2.Zero, DustID.AncientLight, 8, 8);
                            proj.Kill();
                        }
                    }
                }

            if (NoStunTimer > 0)
                NoStunTimer--;
        }

        public override bool CheckActive()
        {
            if (TimeSinceLastActivation > 18000)
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

            return farEnough && numberOfGuardians <= 8 && spawnInfo.player.ZoneOverworldHeight ? 0.1f : 0f ;
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            IsGuardianActive = true;
        }

        public override bool CheckDead()
        {
            if (!Died)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/GuardianDeath"));

                npc.life = npc.lifeMax;
                npc.dontTakeDamage = true;

                Died = true;

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

            Effect deathEffect = TLoZMod.Instance.GetEffect("Effects/GuardianDeath");

            deathEffect.Parameters["uColor"].SetValue(new Vector4(ShotPreparationTimer, ShotPreparationTimer, ShotPreparationTimer, 0.8f));

            if (Died)
                Helpers.StartShader(spriteBatch, deathEffect);

            spriteBatch.Draw(baseTexture, npc.Center - new Vector2(0, 30) - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(baseTexture.Width / 2, baseTexture.Height / 2), 1f, SpriteEffects.None, 1f);

            if (Died && HasResetAI3)
                deathEffect.CurrentTechnique.Passes[0].Apply();

            deathEffect.Parameters["uColor"].SetValue(new Vector4(ShotPreparationTimer, ShotPreparationTimer, ShotPreparationTimer, BootUpTimer));

            spriteBatch.Draw(glowTexture, npc.Center - new Vector2(0, 30) - Main.screenPosition, null, Color.White * BootUpTimer, npc.rotation, new Vector2(baseTexture.Width / 2, baseTexture.Height / 2), 1f, SpriteEffects.None, 1f);

            spriteBatch.Draw(eyeTexture, npc.Center - new Vector2(0, 30) - Main.screenPosition, null, Color.White * (EyeBootUpTimer >= 1.0f ? 1f : 0f), npc.rotation, new Vector2(baseTexture.Width / 2, baseTexture.Height / 2), 1f, SpriteEffects.None, 1f);

            if (Died && HasResetAI3)
                deathEffect.CurrentTechnique.Passes[0].Apply();

            Helpers.EndShader(spriteBatch);

            if (EyeBootUpTimer >= 3.0f && NoLaserTimer <= 0f && !Died && ShotPreparationTimer <= 45f)
            {
                float x = Main.rand.NextFloat(ScanTimer * -1, ScanTimer) * 4;
                float y = Main.rand.NextFloat(ScanTimer * -1, ScanTimer) * 4;

                Vector2 position = ShootPos - new Vector2(0, 6) + new Vector2(x, y);

                Helpers.DrawLine(npc.Center - new Vector2(-1, 48).RotatedBy(npc.rotation), position, mod.GetTexture("Textures/Misc/Laser"), mod.GetTexture("Textures/Misc/LaserEnd"), spriteBatch, ScanTimer <= 2.0f ? Color.Red : Color.Crimson, ScanTimer <= 2.0f ? 2 : 4);
            }
        }


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

        public bool IsGuardianActive { get; set; }

        public float BootUpTimer
        {
            get { return npc.ai[0]; }
            private set { npc.ai[0] = value; }
        }

        public float EyeBootUpTimer
        {
            get { return npc.ai[1]; }
            private set { npc.ai[1] = value; }
        }

        public float ScanTimer
        {
            get { return npc.ai[2]; }
            private set { npc.ai[2] = value; }
        }

        public float ShotPreparationTimer
        {
            get { return npc.ai[3]; }
            private set { npc.ai[3] = value; }
        }
        
        public float NoLaserTimer { get; private set; }

        public Vector2 ShootPos { get; private set; }

        public bool Died { get; private set; }
        public bool HasResetAI3 { get; private set; }

        public int TimeSinceLastActivation { get; private set; }
        public int NoStunTimer { get; private set; }
        public int FocusTimer { get; private set; }
    }
}
