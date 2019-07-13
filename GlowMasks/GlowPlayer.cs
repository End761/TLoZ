using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;

namespace TLoZ
{
    public class GlowPlayer : ModPlayer
    {
        public static int ticks = 0;
        public static int Frame = 0;
        public static readonly PlayerLayer WeaponLayer = new PlayerLayer("WCore", "WeaponLayer", PlayerLayer.HeldItem, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            Player drawPlayer = drawInfo.drawPlayer;

            if (drawPlayer.dead || drawPlayer.frozen || drawPlayer.itemAnimation <= 0) // If the player can't use the item, don't draw it.
                return;

            var item = drawPlayer.HeldItem;
            if (!item.IsAir)
            {
                var m = item.GetGlobalItem<TLoZItems>().GMD;

                if (m != null) // Change WeaponName to your moditem's classname
                {
                    Mod mod = ModLoader.GetMod(m.Mod);
                    var sourceRect = new Rectangle(0, Frame * m.Width, m.Width, m.Height);
                    if (++ticks >= m.FrameSpeed)
                    {
                        if (Frame < m.FrameCount)
                            Frame += 1;
                        else
                            Frame = 0;
                        ticks = 0;
                    }
                    Texture2D weaponTexture = mod.GetTexture(m.TexturePath); // Change the file path to the path where the weapon's glowmask sprite is
                    Vector2 position = new Vector2((float)(drawInfo.itemLocation.X - Main.screenPosition.X), (float)(drawInfo.itemLocation.Y - Main.screenPosition.Y));
                    Vector2 origin = new Vector2(drawPlayer.direction == -1 ? weaponTexture.Width : 0, drawPlayer.gravDir == -1 ? 0 : weaponTexture.Height);
                    Color color = new Color(255, 255, 255, drawPlayer.HeldItem.alpha);
                    ItemSlot.GetItemLight(ref color, drawPlayer.HeldItem, false);
                    origin = new Vector2(drawPlayer.direction == -1 ? m.Width : 0, drawPlayer.gravDir == -1 ? 0 : m.Height);
                    DrawData drawData = new DrawData(weaponTexture, position, sourceRect, drawPlayer.HeldItem.GetAlpha(color), drawPlayer.itemRotation, origin, drawPlayer.HeldItem.scale, drawInfo.spriteEffects, 0);
                    if (drawPlayer.HeldItem.color != default(Color))
                        drawData = new DrawData(weaponTexture, position, sourceRect, drawPlayer.HeldItem.GetColor(color), drawPlayer.itemRotation, origin, drawPlayer.HeldItem.scale, drawInfo.spriteEffects, 0);
                    Main.playerDrawData.Add(drawData);
                }
            }
        });
        public override void PreUpdate()
        {
            if (player.releaseUseItem)
                ticks = 0;
        }
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].Name == "HeldItem")
                {
                    layers.Insert(i + 1, WeaponLayer);
                    break;
                }
            }
        }
    }
}
