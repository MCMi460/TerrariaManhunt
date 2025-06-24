using Terraria_Manhunt.Common.Players;
using System.IO;
using Terraria;
using Terraria.ID;

namespace Terraria_Manhunt
{
    partial class Terraria_Manhunt
    {
        internal enum MessageType : byte
        {
            SyncPlayer
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();

            switch (msgType)
            {
                case MessageType.SyncPlayer:
                    byte playerNumber = reader.ReadByte();
                    ManhuntPlayer player = Main.player[playerNumber].GetModPlayer<ManhuntPlayer>();
                    bool newPlayer = (int)reader.ReadByte() == 1;
                    player.ReceivePlayerSync(reader);

                    bool updateNewPlayer = false;
                    if (newPlayer)
                    {
                        foreach (var plr in Main.ActivePlayers)
                        {
                            ManhuntPlayer modPlayer = plr.GetModPlayer<ManhuntPlayer>();
                            if (modPlayer.trackedPlayer < 255)
                            {
                                player.trackedPlayer = modPlayer.trackedPlayer;
                                updateNewPlayer = true;
                                break;
                            }
                        }
                    }

                    if (Main.netMode == NetmodeID.Server)
                    {
                        // Forward the changes to the other clients
                        player.SyncPlayer(-1, whoAmI, false);
                        if (updateNewPlayer)
                            player.SyncPlayer(playerNumber, whoAmI, false);
                    }
                    break;
                default:
                    Logger.WarnFormat("TerrariaManhunt: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}