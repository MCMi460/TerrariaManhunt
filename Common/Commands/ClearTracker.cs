using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerrariaManhunt.Common.Players;

namespace TerrariaManhunt.Common.Commands
{
    public class ClearTracker : ModCommand
    {
        // Works on server in multiplayer
        public override CommandType Type
            => CommandType.Server;

        // Executed command name
        public override string Command
            => "clearTracker";

        // Command usage description
        public override string Usage
            => "/clearTracker";

        // Command description
        public override string Description
            => "Clears the currently tracked player";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length > 0)
            {
                throw new UsageException("This command doesn't take any parameters!");
            }

            bool found = false;
            foreach (var player in Main.ActivePlayers)
            {
                TrackedPlayerSync modPlayer = player.GetModPlayer<TrackedPlayerSync>();
                if (modPlayer.trackedPlayer < 255)
                {
                    found = true;
                    modPlayer.trackedPlayer = 255;
                    modPlayer.SyncPlayer(player.whoAmI, Main.CurrentPlayer.whoAmI, true);
                }
            }
            if (found)
            {
                caller.Reply("Cleared the tracked player!", Color.Yellow);
            }
            else
            {
                caller.Reply("There isn't a player being tracked right now!", Color.Yellow);
            }
        }
    }
}