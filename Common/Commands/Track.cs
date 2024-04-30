using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerrariaManhunt.Common.Players;

namespace TerrariaManhunt.Common.Commands
{
	public class Track : ModCommand
	{
		// Works on server in multiplayer
		public override CommandType Type
			=> CommandType.Server;

		// Executed command name
		public override string Command
			=> "track";

		// Command usage description
		public override string Usage
			=> "/track player" +
			"\n player - the player (speedrunner) desired to be tracked";

		// Command description
		public override string Description
			=> "Chooses a player to display tracking information to hunters";

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if (args.Length == 0)
			{
				throw new UsageException("Please specify which player to track!");
			}
			else if (args.Length > 1)
			{
				throw new UsageException("Please choose one player to be tracked!\n...Multiple speedrunners is not currently supported.");
			}

            TrackedPlayerSync myPlayer = Main.CurrentPlayer.GetModPlayer<TrackedPlayerSync>();

            bool found = false;
			foreach (var player in Main.ActivePlayers)
			{
				if (player.name == args[0])
				{
                    myPlayer.trackedPlayer = player.whoAmI;
					found = true;
					break;
				}
			}
			if (found)
			{
				foreach (var player in Main.ActivePlayers)
				{
                    TrackedPlayerSync modPlayer = player.GetModPlayer<TrackedPlayerSync>();
					modPlayer.trackedPlayer = myPlayer.trackedPlayer;
					modPlayer.SyncPlayer(player.whoAmI, Main.CurrentPlayer.whoAmI, true);
                }
				caller.Reply($"Set {Main.player[myPlayer.trackedPlayer].name} to the tracked player", Color.Yellow);
			}
			else
			{
				throw new UsageException("Player not found!");
			}
		}
	}
}