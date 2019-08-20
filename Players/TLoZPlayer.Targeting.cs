using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TLoZ.Players
{
    public sealed partial class TLoZPlayer : ModPlayer
    {

        public void ResetTargetingEffects()
        {
            if (MyTarget != null)
            {
                if (Vector2.Distance(player.Center, MyTarget.Center) >= 35 * 16 || !MyTarget.active)
                    MyTarget = null;
            }

            if (TLoZMod.loZClientConfig.switchTarget)
                if (TLoZInput.zTarget.JustPressed)
                {
                    if (MyTarget == null)
                    {
                        bool setInitialTarget = false;
                        foreach (NPC npc in Main.npc)
                        {
                            if (!npc.active)
                                continue;

                            if (!setInitialTarget && Vector2.Distance(player.Center, npc.Center) < 35 * 16)
                            {
                                setInitialTarget = true;
                                MyTarget = npc;
                            }
                            else if (MyTarget != null && Vector2.Distance(player.Center, npc.Center) < Vector2.Distance(player.Center, MyTarget.Center))
                                MyTarget = npc;

                        }
                    }
                    else
                        MyTarget = null;
                }


            if (!TLoZMod.loZClientConfig.switchTarget)
                if (TLoZInput.zTarget.Current)
                {
                    bool setInitialTarget = false;
                    foreach (NPC npc in Main.npc)
                    {
                        if (!npc.active)
                            continue;

                        if (!setInitialTarget && Vector2.Distance(player.Center, npc.Center) < 35 * 16)
                        {
                            setInitialTarget = true;
                            MyTarget = npc;
                        }
                        else if (MyTarget != null && Vector2.Distance(player.Center, npc.Center) < Vector2.Distance(player.Center, MyTarget.Center))
                            MyTarget = npc;

                    }
                }
                else
                    MyTarget = null;


            if (MyTarget != null)
                TargetDirection = MyTarget.Center.X > player.Center.X ? 1 : -1;

            else if (TLoZInput.zTarget.Current)
                TargetDirection = player.direction;

            else
                TargetDirection = -99;

            player.direction = TargetDirection == -99 ? player.direction : TargetDirection;
        }

        public override void ModifyScreenPosition()
        {
            if(MyTarget != null)
            {
                Vector2 positionOffset = (MyTarget.Center - player.Center).SafeNormalize(-Vector2.UnitY) * Vector2.Distance(player.Center, MyTarget.Center) / 3;
                Main.screenPosition = player.Center + positionOffset - new Vector2(Main.screenWidth, Main.screenHeight) / 2;
            }
        }

        public int TargetDirection { get; set; }

        public NPC MyTarget { get; set; }
    }
}
