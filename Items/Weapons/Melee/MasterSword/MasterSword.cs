using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TLoZ.GlowMasks;
using TLoZ.Players;

namespace TLoZ.Items.Weapons.Melee.MasterSword
{
    public class MasterSword : TLoZItem
    {
        private GlowMaskData _nearBossGlow;

        private bool _nearBoss, _nearEvil;

        public MasterSword() : base("Master Sword", "The Blade of Evil's Bane", 50, 50, 0, 0, 5)
        {

        }

        public override string Texture => "TLoZ/Items/Weapons/Melee/MasterSword/MasterByLiz";

        public override void SetDefaults()
        {
            base.SetDefaults();
            _nearBossGlow = new GlowMaskData("Items/Weapons/Melee/MasterSword/MasterGlowByLiz", "TLoZ", 48, 48);
            item.melee = true;
            item.knockBack = 3f;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.melee = true;
            item.autoReuse = true;
            item.useTurn = true;
            item.damage = 30;
        }

        public override void PostReforge()
        {
            item.prefix = 0;
            item.Prefix(0);
        }

        public override bool ReforgePrice(ref int reforgePrice, ref bool canApplyDiscount)
        {
            reforgePrice = 0;
            return base.ReforgePrice(ref reforgePrice, ref canApplyDiscount);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (_nearBoss || _nearEvil)
            {
                Lighting.AddLight(new Vector2(hitbox.X + hitbox.Width / 2, hitbox.Y + hitbox.Height / 2), .9f, .9f, 1.8f);
            }
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            if (_nearBoss || _nearEvil) mult = 2;
        }

        public override void UpdateInventory(Player player)
        {
            _nearBoss = false;
            _nearEvil = player.ZoneUnderworldHeight || player.ZoneCrimson || player.ZoneCorrupt;
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || (!npc.boss && npc.type != NPCID.Clothier && npc.type != NPCID.OldMan))
                    continue;
                if (Vector2.Distance(player.Center, npc.Center) <= 3000)
                    _nearBoss = true;
            }
            if (_nearBoss || _nearEvil)
                item.GetGlobalItem<TLoZGlobalItem>().gmd = _nearBossGlow;
            else
                item.GetGlobalItem<TLoZGlobalItem>().gmd = null;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            spriteBatch.Draw
                (
                    mod.GetTexture("Items/Weapons/Melee/MasterSword/MasterDroppedByLiz"),
                    item.Center - Main.screenPosition,
                    null,
                    lightColor,
                    rotation,
                    new Vector2(19, 19),
                    scale,
                    SpriteEffects.None,
                    1f
                );
        }
    }
}
