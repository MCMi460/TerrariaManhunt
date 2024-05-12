using Terraria;
using Terraria.ModLoader;

namespace Terraria_Manhunt.Common.Systems
{
    public class ExitSystem : ModSystem
    {
        public override void PreSaveAndQuit()
        {
            if (ModContent.GetInstance<TerrariaManhuntSettings>().AnnounceAchievements)
            {
                Main.Achievements.ClearAll();
            }
        }
    }
}