using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaManhunt.Common.Players
{
    // This syncs the client with the server and vice versa
    public class TargetedPlayerSync : ModPlayer
    {
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)TerrariaManhunt.MessageType.UpdateTargetedPlayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)TerrariaManhunt.trackedPlayer.whoAmI);
            packet.Send(toWho, fromWho);
        }

        public void ReceivePlayerSync(BinaryReader reader)
        {
            int whoAmI = reader.ReadByte();
            foreach (var player in Main.player)
            {
                if (player.whoAmI == whoAmI)
                {
                    TerrariaManhunt.trackedPlayer = player;
                }
            }
        }
    }
}