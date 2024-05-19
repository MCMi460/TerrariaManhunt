using Terraria_Manhunt.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Terraria_Manhunt.Content
{
    // Add tracker information
    public class TrackerInfoDisplay : InfoDisplay
    {
        public static Color InfoTextColor => new(255, 19, 19, Main.mouseTextColor);

        // Determines if tracker is displayed or not
        public override bool Active()
        {
            return Main.CurrentPlayer.GetModPlayer<ManhuntPlayer>().showTracker;
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            displayColor = Color.White;
            string displayInfo = "";
            ManhuntPlayer modPlayer = Main.CurrentPlayer.GetModPlayer<ManhuntPlayer>();

            if (modPlayer.trackedPlayer < 255)
            {
                Player player = Main.player[modPlayer.trackedPlayer];
                if (Main.myPlayer == modPlayer.trackedPlayer)
                {
                    displayColor = InfoDisplay.InactiveInfoTextColor;
                    displayInfo = "You are targeted!";
                }
                else
                {
                    displayInfo = Main.CurrentPlayer.position.X > player.position.X ? "West" : "East";
                }
            }
            else
            {
                displayColor = InfoDisplay.InactiveInfoTextColor;
                displayInfo = "No player targeted!";
            }

            return displayInfo;
        }
    }
}