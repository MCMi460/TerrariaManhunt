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
                    
                    
                    
                    /*
                    Logger.WarnFormat("before " + reader.BaseStream.Position);
                    foreach (var user in Main.ActivePlayers)
                    {
                        reader.BaseStream.Position = 5;
                        user.GetModPlayer<TargetedPlayerSync>().ReceivePlayerSync(reader);
                    }*/


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