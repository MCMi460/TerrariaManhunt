using System.IO;
using Terraria;
using System.Runtime.Serialization.Formatters.Binary;
using log4net.Repository.Hierarchy;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrariaManhunt.Common.Players
{
    // This syncs the client with the server and vice versa
    public class TrackedPlayerSync : ModPlayer
    {
        public int trackedPlayer = -1;

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
            Main.NewText("first tracked player: " + trackedPlayer, Color.White);
            trackedPlayer = whoAmI;
            Main.NewText("final tracked player: " + Main.player[trackedPlayer].name, Color.White);
        }
    }
}