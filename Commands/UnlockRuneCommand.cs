using Terraria;
using Terraria.ModLoader;
using TLoZ.Players;
using TLoZ.Runes;

namespace TLoZ.Commands
{
    public class UnlockRuneCommand : ModCommand
    {
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
                return;

            TLoZPlayer.Get(Main.LocalPlayer).UnlockRune(RuneManager.Instance[args[0]]);
        }

        public override string Command => "unlockRune";
        public override CommandType Type => CommandType.World;
    }
}