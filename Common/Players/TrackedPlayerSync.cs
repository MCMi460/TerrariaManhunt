using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace Terraria_Manhunt.Common.Players
{
    // This syncs the client with the server and vice versa
    public class TrackedPlayerSync : ModPlayer
    {
        public bool showTracker = true;
        public int trackedPlayer = 255;

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)Terraria_Manhunt.MessageType.UpdateTargetedPlayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)trackedPlayer);
            packet.Write((byte)(showTracker ? 1 : 0));
            packet.Send(toWho, fromWho);
        }

        public void ReceivePlayerSync(BinaryReader reader)
        {
            byte whoAmI = reader.ReadByte();
            trackedPlayer = whoAmI;
            bool show = (int)reader.ReadByte() == 1;
            showTracker = show;
        }

        public override void OnEnterWorld()
        {
            if (ModContent.GetInstance<TerrariaManhuntSettings>().AnnounceAchievements)
            {
                Main.Achievements.ClearAll();
            }
        }
    }
}