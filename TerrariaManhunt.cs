using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;

namespace TerrariaManhunt
{
    public partial class TerrariaManhunt : Mod
    {
        public override void Load()
        {
            TerrariaManhuntSettings settings = ModContent.GetInstance<TerrariaManhuntSettings>();
            // Load IL edits (if enabled in settings)
            if (settings.HidePlayers)
            {
                IL_MapHeadRenderer.DrawPlayerHead += HookDrawPlayerHead;
            }
            if (settings.HideNPCs)
            {
                IL_NPCHeadRenderer.DrawWithOutlines += HookDrawNPCHead;
            }
            if (settings.HideDeathMarkers)
            {
                // Fullscreen map
                IL_Main.DrawPlayerDeathMarker += HookDrawDeathMarker;
                // Minimap
                IL_Main.DrawMap += HookDrawMinimapMarkers;
            }
            if (settings.DisallowSpawn)
            {
                IL_Player.ChangeSpawn += HookDisallowSpawn;
            }
            if (settings.DisableTelePot)
            {
                IL_Player.TeleportationPotion += HookTelePot;
            }
            if (settings.FriendlyFire)
            {
                IL_Player.InOpposingTeam += HookOpposingTeam;
                IL_Player.ItemCheck_MeleeHitPVP += HookMeleeCheck;
            }
            if (settings.HideHealthBars)
            {
                IL_Main.DrawInterface_14_EntityHealthBars += HookDrawHealthBars;
            }
        }
    }
}