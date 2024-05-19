using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
            TerrariaManhuntSettings settings = ModContent.GetInstance<TerrariaManhuntSettings>();
            if (settings.AnnounceAchievements)
            {
                Main.Achievements.ClearAll();
            }
            if (settings.ForcePvP && Main.netMode == NetmodeID.MultiplayerClient)
            {
                Main.player[Main.myPlayer].hostile = true;
                NetMessage.SendData(MessageID.TogglePVP, -1, -1, null, Main.myPlayer);
            }
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Terraria_Manhunt.SendMessage("Terraria Manhunt: Most functionality is disabled on singleplayer.", Color.Violet);
            }
            else if (!Terraria_Manhunt.shownMultiplayerMessage)
            {
                Terraria_Manhunt.SendMessage("Terraria Manhunt: To use the tracker, type \"/tracker help\".\n * Thanks for your support!", Color.Violet);
                Terraria_Manhunt.shownMultiplayerMessage = true;
            }
        }
    }
}