using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TLoZ.Dusts
{
    public class GeneralUseDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 32, 32);
            dust.scale = 0.4f;
            dust.position.X -= 32f * (dust.scale - 0.1f);
            dust.position.Y -= 32f * (dust.scale - 0.1f);
            dust.noGravity = true;
            dust.alpha = 100;
        }

        public override bool Update(Dust dust)
        {
            dust.scale *= 0.955f;
            dust.alpha += 25;
            dust.color = new Color(Vector3.Lerp(dust.color.ToVector3(), Color.Black.ToVector3(), 0.005f));
            if (dust.scale < 0.25f || dust.alpha >= 255)
            {
                dust.active = false;
            }
            return false;
        }
    }
}
