using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerrariaManhunt.Common.Players;

namespace TerrariaManhunt.Common.Commands
{
	public class Target : ModCommand
	{
		// Works on server in multiplayer
		public override CommandType Type
			=> CommandType.Server;

		// Executed command name
		public override string Command
			=> "target";

		// Command usage description
		public override string Usage
			=> "/target player" +
			"\n player - the player (speedrunner) desired to be tracked";

		// Command description
		public override string Description
			=> "Chooses a player to display tracking information to hunters";

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if (args.Length == 0)
			{
				throw new UsageException("Please specify which player to target!");
			}
			else if (args.Length > 1)
			{
				throw new UsageException("Please choose one player to be targeted!\n...Multiple speedrunners is not currently supported.");
			}

			bool found = false;
			foreach (var player in Main.player)
			{
				if (player.active && player.name == args[0])
				{
					TerrariaManhunt.trackedPlayer = player;
					Player current = Main.CurrentPlayer;
					current.GetModPlayer<TargetedPlayerSync>().SyncPlayer(current.whoAmI, current.whoAmI, false);
					found = true;
					break;
				}
			}
			if (found)
			{
				caller.Reply($"Set {TerrariaManhunt.trackedPlayer.name} to the tracked player", Color.Yellow);
			}
			else
			{
				throw new UsageException("Player not found!");
			}
		}
	}
}