using Terraria.ModLoader;
using TerrariaManhunt.Common.Players;
using Microsoft.Xna.Framework;

namespace TerrariaManhunt.Common.Commands
{
    public class GetTarget : ModCommand
    {
        // Executes for user
        public override CommandType Type
            => CommandType.Chat;

        // Executed command name
        public override string Command
            => "getTarget";

        // Command usage description
        public override string Usage
            => "/getTarget";

        // Command description
        public override string Description
            => "Gets the currently targeted user";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length > 0)
            {
                throw new UsageException("This command does not take any arguments!");
            }

            if (TerrariaManhunt.trackedPlayer != null)
            {
                caller.Reply($"{TerrariaManhunt.trackedPlayer.name} is the tracked player", Color.Yellow);
            } else
            {
                caller.Reply("There isn't a player being targeted!", Color.Yellow);
            }
        }
    }
}