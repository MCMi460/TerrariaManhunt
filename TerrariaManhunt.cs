using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;

namespace TerrariaManhunt
{
	public partial class TerrariaManhunt : Mod
	{
        public override void Load()
        {
            IL_MapHeadRenderer.DrawPlayerHead += HookDrawPlayerHead;
            IL_NPCHeadRenderer.DrawWithOutlines += HookDrawNPCHead;
            IL_Main.DrawPlayerDeathMarker += HookDrawDeathMarker;
            IL_Main.DrawMap += HookDrawMinimapMarkers;
        }
    }
}