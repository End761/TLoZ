using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;
using TLoZ.GlowMasks;
using TLoZ.Items;

namespace TLoZ.Players
{
    public sealed partial class TLoZPlayer : ModPlayer
    {
        public static int ticks = 0;
        public static int frame = 0;

        public static readonly PlayerLayer weaponLayer = new PlayerLayer("WCore", "WeaponLayer", PlayerLayer.HeldItem, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            Player drawPlayer = drawInfo.drawPlayer;

            if (drawPlayer.dead || drawPlayer.frozen || drawPlayer.itemAnimation <= 0) // If the player can't use the item, don't draw it.
                return;

            Item item = drawPlayer.HeldItem;
            if (!item.IsAir)
            {
                GlowMaskData m = item.GetGlobalItem<TLoZGlobalItem>().gmd;

                if (m != null)
                {
                    Mod mod = ModLoader.GetMod(m.mod);
                    Rectangle sourceRect = new Rectangle(0, frame * m.width, m.width, m.height);
                    if (++ticks >= m.frameSpeed)
                    {
                        if (frame < m.frameCount)
                            frame += 1;
                        else
                            frame = 0;
                        ticks = 0;
                    }
                    Texture2D weaponTexture = mod.GetTexture(m.texturePath); // Change the file path to the path where the weapon's glowmask sprite is
                    Vector2 position = new Vector2((float)(drawInfo.itemLocation.X - Main.screenPosition.X), (float)(drawInfo.itemLocation.Y - Main.screenPosition.Y));
                    Vector2 origin = new Vector2(drawPlayer.direction == -1 ? weaponTexture.Width : 0, drawPlayer.gravDir == -1 ? 0 : weaponTexture.Height);
                    Color color = new Color(255, 255, 255, drawPlayer.HeldItem.alpha);
                    ItemSlot.GetItemLight(ref color, drawPlayer.HeldItem, false);
                    origin = new Vector2(drawPlayer.direction == -1 ? m.width : 0, drawPlayer.gravDir == -1 ? 0 : m.height);
                    DrawData drawData = new DrawData(weaponTexture, position, sourceRect, drawPlayer.HeldItem.GetAlpha(color), drawPlayer.itemRotation, origin, drawPlayer.HeldItem.scale, drawInfo.spriteEffects, 0);
                    if (drawPlayer.HeldItem.color != default)
                        drawData = new DrawData(weaponTexture, position, sourceRect, drawPlayer.HeldItem.GetColor(color), drawPlayer.itemRotation, origin, drawPlayer.HeldItem.scale, drawInfo.spriteEffects, 0);
                    Main.playerDrawData.Add(drawData);
                }
            }
        });

        public void PreGlowMaskUpdate()
        {
            if (player.releaseUseItem)
                ticks = 0;
        }

        private void ModifyGlowPlayerDrawLayers(List<PlayerLayer> layers)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].Name == "HeldItem")
                {
                    layers.Insert(i + 1, weaponLayer);
                    break;
                }
            }
        }
    }
}
