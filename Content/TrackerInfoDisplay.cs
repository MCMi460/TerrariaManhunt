using TerrariaManhunt.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaManhunt.Content
{
    // Add tracker information
    public class TrackerInfoDisplay : InfoDisplay
    {
        public static Color InfoTextColor => new(255, 19, 19, Main.mouseTextColor);

        // Determines if tracker is displayed or not
        public override bool Active()
        {
            return Main.LocalPlayer.GetModPlayer<DisplayTrackerPlayer>().showTracker;
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            bool info = TerrariaManhunt.trackedPlayer != null;
            displayColor = Color.White;
            string displayInfo = "";

            if (info)
            {
                if (Main.CurrentPlayer.whoAmI == TerrariaManhunt.trackedPlayer.whoAmI)
                {
                    displayColor = InfoDisplay.InactiveInfoTextColor;
                    displayInfo = "You are targeted!";
                }
                else
                {
                    displayInfo = Main.CurrentPlayer.position.X > TerrariaManhunt.trackedPlayer.position.X ? "West" : "East";
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