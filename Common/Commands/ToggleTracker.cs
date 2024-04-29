using Terraria.ModLoader;
using TerrariaManhunt.Common.Players;
using Microsoft.Xna.Framework;

namespace TerrariaManhunt.Common.Commands
{
    public class ToggleTracker : ModCommand
    {
        // Executes for user
        public override CommandType Type
            => CommandType.Chat;

        // Executed command name
        public override string Command
            => "toggleTracker";

        // Command usage description
        public override string Usage
            => "/toggleTracker";

        // Command description
        public override string Description
            => "Toggles the compass tracker for the user";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length > 0)
            {
                throw new UsageException("This command does not take any arguments!");
            }

            // This sets the player's DisplayInfo to include the tracker
            DisplayTrackerPlayer player = caller.Player.GetModPlayer<DisplayTrackerPlayer>();
            player.showTracker = !player.showTracker;

            caller.Reply($"{(player.showTracker ? "Displaying" : "Hiding")} tracker!", Color.Yellow);
        }
    }
}