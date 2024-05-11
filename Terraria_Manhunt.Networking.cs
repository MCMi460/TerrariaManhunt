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
            UpdateTargetedPlayer
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();

            switch (msgType)
            {
                case MessageType.UpdateTargetedPlayer:

                    byte playerNumber = reader.ReadByte();
                    TrackedPlayerSync player = Main.player[playerNumber].GetModPlayer<TrackedPlayerSync>();
                    player.ReceivePlayerSync(reader);

                    if (Main.netMode == NetmodeID.Server)
                    {
                        // Forward the changes to the other clients
                        player.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
                default:
                    Logger.WarnFormat("TerrariaManhunt: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}