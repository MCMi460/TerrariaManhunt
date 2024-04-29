using TerrariaManhunt.Common.Players;
using System.IO;
using Terraria;
using Terraria.ID;

namespace TerrariaManhunt
{
    partial class TerrariaManhunt
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
                    TargetedPlayerSync player = Main.player[playerNumber].GetModPlayer<TargetedPlayerSync>();
                    player.ReceivePlayerSync(reader);

                    if (Main.netMode == NetmodeID.Server)
                    {
                        // Forward the changes to the other clients
                        player.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
                default:
                    Logger.WarnFormat("ExampleMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}