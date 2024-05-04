using Terraria;
using Terraria.ModLoader;
using TerrariaManhunt.Common.Players;
using Microsoft.Xna.Framework;

namespace TerrariaManhunt.Common.Commands
{
    public class GetTracked : ModCommand
    {
        // Executes for user
        public override CommandType Type
            => CommandType.Chat;

        // Executed command name
        public override string Command
            => "getTracked";

        // Command usage description
        public override string Usage
            => "/getTracked";

        // Command description
        public override string Description
            => "Gets the currently tracked user";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length > 0)
            {
                throw new UsageException("This command does not take any arguments!");
            }

            TrackedPlayerSync modPlayer = Main.CurrentPlayer.GetModPlayer<TrackedPlayerSync>();

            if (modPlayer.trackedPlayer < 255)
            {
                caller.Reply($"{Main.player[modPlayer.trackedPlayer].name} is the tracked player", Color.Yellow);
            }
            else
            {
                caller.Reply("There isn't a player being tracked!", Color.Yellow);
            }
        }
    }
}