using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Terraria_Manhunt.Common.Players
{
    // This syncs the client with the server and vice versa
    public class ManhuntPlayer : ModPlayer
    {
        public bool showTracker = true;
        public int trackedPlayer = 255;

        public override void Load()
        {
            On_Player.RollLuck += new On_Player.hook_RollLuck(this.HookRollLuck);
            base.Load();
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)Terraria_Manhunt.MessageType.SyncPlayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)(newPlayer ? 1 : 0));
            packet.Write((byte)trackedPlayer);
            packet.Write((byte)(showTracker ? 1 : 0));
            packet.Send(toWho, fromWho);
        }

        public void ReceivePlayerSync(BinaryReader reader)
        {
            trackedPlayer = (int) reader.ReadByte();
            if (trackedPlayer == Main.myPlayer)
                this.OnSpeedrunner();
            else
                this.OnHunter();
            showTracker = (int)reader.ReadByte() == 1;
        }

        public override void OnEnterWorld()
        {
            TerrariaManhuntSettings settings = ModContent.GetInstance<TerrariaManhuntSettings>();
            if (settings.AnnounceAchievements)
                Main.Achievements.ClearAll();
            if (settings.ForcePvP && Main.netMode == NetmodeID.MultiplayerClient)
            {
                Main.player[Main.myPlayer].hostile = true;
                NetMessage.SendData(MessageID.TogglePVP, -1, -1, null, Main.myPlayer);
            }
            if (Main.netMode == NetmodeID.SinglePlayer)
                Terraria_Manhunt.SendMessage("Terraria Manhunt: Most functionality is disabled on singleplayer.", Color.Violet);
            else if (!Terraria_Manhunt.shownMultiplayerMessage)
            {
                Terraria_Manhunt.SendMessage("Terraria Manhunt: To use the tracker, type \"/tracker help\".\n * Thanks for your support!", Color.Violet);
                Terraria_Manhunt.shownMultiplayerMessage = true;
            }
        }

        private int HookRollLuck(On_Player.orig_RollLuck orig, Player self, int range)
        {
            float luckBoost = ModContent.GetInstance<TerrariaManhuntSettings>().LuckBoost;
            if (luckBoost > 0f && self.whoAmI == self.GetModPlayer<ManhuntPlayer>().trackedPlayer)
            {
                self.RecalculateLuck();
                self.luck += luckBoost;
                // See Player.RollLuck()
                if (self.luck > 0f)
                {
                    for (float i = self.luck; i > 1f && range > 1; i--)
                    { // && range > 1 allows for extra luck ... I think
                        range = Main.rand.Next(range / 2, range); // Slowly push closer per point of luck
                    }
                } else if (self.luck < 0f && Main.rand.NextFloat() < 0f - self.luck)
                {
                    // Normal behavior, I think
                    range = Main.rand.Next(range, range * 2);
                }
            }
            return Main.rand.Next(range);
        }

        private void OnSpeedrunner()
        {
            Terraria_Manhunt.SendMessage("You are now the speedrunner!", Color.Purple);
            //TerrariaManhuntSettings settings = ModContent.GetInstance<TerrariaManhuntSettings>();
        }

        private void OnHunter()
        {
            //Terraria_Manhunt.SendMessage("You are now a hunter!", Color.Purple);
        }

        public override void PostUpdateMiscEffects()
        {
            TerrariaManhuntSettings settings = ModContent.GetInstance<TerrariaManhuntSettings>();
            Main.LocalPlayer.moveSpeed *= trackedPlayer == Main.myPlayer ? settings.SpeedBoostRunner : settings.SpeedBoostHunters;

            base.PostUpdateMiscEffects();
        }
    }
}