using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TLoZ.Items.Weapons.Melee.MasterSword;
using TLoZ.Players;

namespace TLoZ
{
    public class TLoZDrawLayers
    {
        private static TLoZDrawLayers _instance;

        public static TLoZDrawLayers Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TLoZDrawLayers();
                return _instance;
            }
        }

        public readonly PlayerLayer twoHandedWeaponLayer = new PlayerLayer("TLoZ", "twoHandedWeaponLayer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            Player drawPlayer = drawInfo.drawPlayer;
            TLoZPlayer tlozPlayer = TLoZPlayer.Get(drawPlayer);

            if (drawPlayer.dead || !tlozPlayer.HoldsTwoHander) // If the player can't use the item, don't draw it.
                return;
            int itemType = drawPlayer.HeldItem.type;
            Texture2D itemTexture = Main.itemTexture[itemType];
            int dir = drawPlayer.direction;

            Color color = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16);
            Instance.TwoHanderRotation = (tlozPlayer.IsSlashReversed ? MathHelper.Pi : dir == -1 ? (MathHelper.Pi / 3) : MathHelper.Pi + MathHelper.Pi * .66f) - (tlozPlayer.SwingRotation) * dir;

            Vector2 posOffset = tlozPlayer.SwingRotation > 0.0f ? new Vector2(8 * dir, 2) * tlozPlayer.SwingRotation : Vector2.Zero;
            posOffset = tlozPlayer.TwoHanderChargeAttack ? new Vector2(12 * drawPlayer.direction, 0) : posOffset;
            SpriteEffects spriteEffects = tlozPlayer.IsSlashReversed ? dir == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally : dir == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 origin = tlozPlayer.IsSlashReversed ? new Vector2(dir == 1 ? 0 : itemTexture.Width, itemTexture.Height) : new Vector2(dir == -1 ? 0 : itemTexture.Width, itemTexture.Height);
            Instance.TwoHanderVFX = new Vector2((int)drawPlayer.MountedCenter.X, (int)drawPlayer.MountedCenter.Y + drawPlayer.gfxOffY) + new Vector2(22 * dir, 6) - posOffset;
            DrawData weaponData = new DrawData
            (
                itemTexture,
                Instance.TwoHanderVFX - Main.screenPosition,
                null,
                color,
                Instance.TwoHanderRotation,
                origin,
                1.4f,
                spriteEffects,
                1
                );

            Main.playerDrawData.Add(weaponData);
        });


        public readonly PlayerLayer paragliderLayer = new PlayerLayer("TLoZ", "paragliderLayer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            Player drawPlayer = drawInfo.drawPlayer;

            if (drawPlayer.dead) // If the player can't use the item, don't draw it.
                return;
            Color color = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16);
            DrawData sheathData = new DrawData
            (
                ModContent.GetTexture("TLoZ/Items/Tools/ParaByLiz"),
                new Vector2((int)drawPlayer.MountedCenter.X, (int)drawPlayer.MountedCenter.Y - 22 + drawPlayer.gfxOffY) - Main.screenPosition,
                null,
                color,
                0,
                new Vector2(19, 12),
                1f,
                drawPlayer.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                1
                );
            Main.playerDrawData.Add(sheathData);
        });



        public readonly PlayerLayer masterSwordSheath = new PlayerLayer("TLoZ", "SheathLayer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            Player drawPlayer = drawInfo.drawPlayer;

            if (drawPlayer.dead) // If the player can't use the item, don't draw it.
                return;

            int offsetY = 6;
            int offsetX = 6 * drawPlayer.direction;

            Color color = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16);
            DrawData sheathData = new DrawData
            (
                ModContent.GetTexture("TLoZ/Items/Weapons/Melee/MasterSword/MasterSwordSheath"),
                new Vector2((int)drawPlayer.MountedCenter.X - offsetX, (int)drawPlayer.MountedCenter.Y + offsetY + drawPlayer.gfxOffY) - Main.screenPosition,
                null,
                color,
                0,
                new Vector2(17, 17),
                1f,
                drawPlayer.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                1
                );

            DrawData swordData = new DrawData
            (
                ModContent.GetTexture("TLoZ/Items/Weapons/Melee/MasterSword/MasterSheathedByLiz"),
                new Vector2((int)drawPlayer.MountedCenter.X + 2 * drawPlayer.direction, (int)drawPlayer.MountedCenter.Y - 2 + drawPlayer.gfxOffY) - Main.screenPosition,
                null,
                color,
                0,
                new Vector2(24, 24),
                1f,
                drawPlayer.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                1
                );

            if (drawPlayer.HeldItem.type != TLoZMod.Instance.ItemType<MasterSword>() || !TLoZPlayer.Get(drawPlayer).UsingMasterSword)
                Main.playerDrawData.Add(swordData);

            Main.playerDrawData.Add(sheathData);
        });

        public readonly PlayerLayer masterSwordSheathBelt = new PlayerLayer("TLoZ", "SheathBeltLayer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            Player drawPlayer = drawInfo.drawPlayer;

            if (drawPlayer.dead) // If the player can't use the item, don't draw it.
                return;
            Color color = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16);
            DrawData sheathData = new DrawData
            (
                ModContent.GetTexture("TLoZ/Items/Weapons/Melee/MasterSword/Buckle_Body"),
                new Vector2((int)drawPlayer.MountedCenter.X, (int)drawPlayer.MountedCenter.Y - 3 + drawPlayer.gfxOffY) - Main.screenPosition,
                drawPlayer.bodyFrame,
                color,
                0,
                new Vector2(20, 28),
                1f,
                drawPlayer.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                1
                );
            Main.playerDrawData.Add(sheathData);
        });

        public readonly PlayerLayer equipedShieldLayer = new PlayerLayer("TLoZ", "EquipedShieldLayer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            Player drawPlayer = drawInfo.drawPlayer;
            TLoZPlayer zPlayer = TLoZPlayer.Get(drawPlayer);

            if (drawPlayer.dead) // If the player can't use the item, don't draw it.
                return;

            if (zPlayer.EquipedShield == null || drawPlayer.itemAnimation > 0)
                return;

            Color color = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16);

            Texture2D texture = TLoZMod.Instance.GetTexture(zPlayer.EquipedShield.EquipedTexturePath);

            DrawData shieldData = new DrawData
            (
                texture,
                new Vector2((int)drawPlayer.MountedCenter.X, (int)drawPlayer.MountedCenter.Y + drawPlayer.gfxOffY) - Main.screenPosition,
                new Rectangle(0, zPlayer.ParryVisualTime > 0 ? texture.Height / 2 : 0, texture.Width, texture.Height / 2),
                color,
                0,
                new Vector2(texture.Width / 2, texture.Height / 4),
                1f,
                drawPlayer.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                1
                );
            Main.playerDrawData.Add(shieldData);
        });

        public float TwoHanderRotation { get; private set; }
        public Vector2 TwoHanderVFX { get; private set; }
    }
}
