using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria_Manhunt.Common.Players;
using System.Linq;

namespace Terraria_Manhunt.Common.Commands
{
    public class Tracker : ModCommand
    {
        // Works on server in multiplayer
        public override CommandType Type
            => CommandType.Server;

        // Executed command name
        public override string Command
            => "tracker";

        // Command usage description
        public override string Usage
            => "/tracker hide/show/set/get/clear" +
            "\n hide - hides the tracker display" +
            "\n show - shows the tracker display" +
            "\n set <player> - the player (speedrunner) desired to be tracked" +
            "\n get - returns the currently tracked player" +
            "\n clear - clears the currently tracked player";

        // Command description
        public override string Description
            => "Tracker handler command";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                throw new UsageException("The tracker does not work on singleplayer!");
            }
            else if (args.Length == 0)
            {
                throw new UsageException("Type \"/tracker help\" for help!");
            }
            else if (args.Length > 1 && args[0] != "set")
            {
                throw new UsageException("Cannot take more than one parameter!");
            }

            TrackedPlayerSync player = caller.Player.GetModPlayer<TrackedPlayerSync>();
            bool found = false;

            switch (args[0])
            {
                case "hide":
                    player.showTracker = false;
                    player.SyncPlayer(caller.Player.whoAmI, 255, true);
                    caller.Reply("Hiding tracker!", Color.Yellow);
                    break;
                case "show":
                    player.showTracker = true;
                    player.SyncPlayer(caller.Player.whoAmI, 255, true);
                    caller.Reply("Displaying tracker!", Color.Yellow);
                    break;
                case "set":
                    if (args.Length < 2)
                    {
                        throw new UsageException("Please choose a player to track!");
                    }

                    string trackedName = string.Join(" ", args.Skip(1));

                    foreach (var plr in Main.ActivePlayers)
                    {
                        if (plr.name == trackedName)
                        {
                            player.trackedPlayer = plr.whoAmI;
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        foreach (var plr in Main.ActivePlayers)
                        {
                            TrackedPlayerSync modPlayer = plr.GetModPlayer<TrackedPlayerSync>();
                            modPlayer.trackedPlayer = player.trackedPlayer;
                            modPlayer.SyncPlayer(plr.whoAmI, caller.Player.whoAmI, true);
                        }
                        caller.Reply($"Set {Main.player[player.trackedPlayer].name} to the tracked player", Color.Yellow);
                    }
                    else
                    {
                        throw new UsageException("Player not found!");
                    }
                    break;
                case "get":
                    if (player.trackedPlayer < 255)
                    {
                        caller.Reply($"{Main.player[player.trackedPlayer].name} is the tracked player", Color.Yellow);
                    }
                    else
                    {
                        caller.Reply("There isn't a player being tracked!", Color.Yellow);
                    }
                    break;
                case "clear":
                    foreach (var plr in Main.ActivePlayers)
                    {
                        TrackedPlayerSync modPlayer = plr.GetModPlayer<TrackedPlayerSync>();
                        if (modPlayer.trackedPlayer < 255)
                        {
                            found = true;
                            modPlayer.trackedPlayer = 255;
                            modPlayer.SyncPlayer(plr.whoAmI, caller.Player.whoAmI, true);
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
                    break;
                case "help":
                    caller.Reply(Usage, Color.Red);
                    break;
                default:
                    throw new UsageException("Invalid subcommand ... Type \"/tracker help\" for help!");
            }
        }
    }
}