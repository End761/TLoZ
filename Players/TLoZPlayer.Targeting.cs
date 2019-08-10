using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TLoZ.Players
{
    public sealed partial class TLoZPlayer : ModPlayer
    {
        public NPC myTarget;

        public void ResetTargetingEffects()
        {
            if (myTarget != null)
            {
                if (Vector2.Distance(player.Center, myTarget.Center) >= 35 * 16 || !myTarget.active)
                    myTarget = null;
            }
            if (TLoZMod.loZClientConfig.switchTarget)
                if (TLoZInput.zTarget.JustPressed)
                {
                    if (myTarget == null)
                    {
                        bool setInitialTarget = false;
                        foreach (NPC npc in Main.npc)
                        {
                            if (!npc.active)
                                continue;

                            if (!setInitialTarget && Vector2.Distance(player.Center, npc.Center) < 35 * 16)
                            {
                                setInitialTarget = true;
                                myTarget = npc;
                            }
                            else if (myTarget != null && Vector2.Distance(player.Center, npc.Center) < Vector2.Distance(player.Center, myTarget.Center))
                                myTarget = npc;

                        }
                    }
                    else
                        myTarget = null;
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
                            myTarget = npc;
                        }
                        else if (myTarget != null && Vector2.Distance(player.Center, npc.Center) < Vector2.Distance(player.Center, myTarget.Center))
                            myTarget = npc;

                    }
                }
                else
                    myTarget = null;
        }
        public override void ModifyScreenPosition()
        {
            if(myTarget != null)
            {
                Vector2 positionOffset = (myTarget.Center - player.Center).SafeNormalize(-Vector2.UnitY) * Vector2.Distance(player.Center, myTarget.Center) / 3;
                Main.screenPosition = player.Center + positionOffset - new Vector2(Main.screenWidth, Main.screenHeight) / 2;
            }
        }
    }
}
