using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using TLoZ.Items.Weapons.NoGroup;

namespace TLoZ.Projectiles.Misc
{
    public class TwoHandedWeaponHitbox : TLoZProjectile
    {
        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.width = 100;
            projectile.height = 120;
            projectile.friendly = true;
            projectile.penetrate = -1;
        }
        public override string Texture => "TLoZ/Textures/Misc/EmptyPixel";
        public override void AI()
        {
            projectile.timeLeft = 2;
            if (!TLoZPlayer.isSwinging)
                projectile.Kill();
            Texture2D texture = Main.itemTexture[Owner.HeldItem.type];
            projectile.width = (int)(texture.Width * 2f);
            projectile.Center = Owner.Center + new Vector2(texture.Width * .5f * Owner.direction, -20);

            if(Owner.HeldItem.type == mod.ItemType<WoodcutterAxe>())
            {
                for(int i = 0; i < projectile.height; i++)
                {
                    for(int j = 0; j < projectile.width; j++)
                    {
                        Tile tile = Main.tile[(int)(projectile.position.X + j) / 16, (int)(projectile.position.Y + i) / 16];
                        if (tile.type == TileID.Trees || tile.type == TileID.PalmTree)
                        {
                            WorldGen.KillTile((int)(projectile.position.X + j) / 16, (int)(projectile.position.Y + i) / 16, false, false);
                        }
                    }
                }
            }
        }
    }
}
