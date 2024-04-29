using Terraria;
using Terraria.ModLoader;

namespace TerrariaManhunt.Common.Players
{
    public class DisplayTrackerPlayer : ModPlayer
    {
        // Should show tracker or not
        public bool showTracker;

        // Our tracker acts a little differently, so this will do nothing
        public override void ResetInfoAccessories()
        {

        }

        // This, again, does nothing, as our tracker is not bound by accessory or team
        public override void RefreshInfoAccessoriesFromTeamPlayers(Player otherPlayer)
        {
            if (otherPlayer.GetModPlayer<DisplayTrackerPlayer>().showTracker)
            {
                //showTracker = true;
            }
            //base.RefreshInfoAccessoriesFromTeamPlayers(otherPlayer);
        }
    }
}