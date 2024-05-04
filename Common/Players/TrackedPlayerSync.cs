using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaManhunt.Common.Players
{
    // This syncs the client with the server and vice versa
    public class TrackedPlayerSync : ModPlayer
    {
        public int trackedPlayer = 255;
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)TerrariaManhunt.MessageType.UpdateTargetedPlayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)trackedPlayer);
            packet.Send(toWho, fromWho);
        }

        public void ReceivePlayerSync(BinaryReader reader)
        {
            byte whoAmI = reader.ReadByte();
            trackedPlayer = whoAmI;
        }
    }
}