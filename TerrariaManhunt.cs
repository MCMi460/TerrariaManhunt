using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ID;
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
        }

        public static void HookDrawPlayerHead(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<TerrariaManhunt>(), il, e);
            }
        }

        public static void HookDrawNPCHead(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<TerrariaManhunt>(), il, e);
            }
        }

        // Note: Does not hide minimap TextureAssets.MapDeath ... mostly because I'm too lazy to care
        public static void HookDrawDeathMarker(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, 0);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<TerrariaManhunt>(), il, e);
            }
        }
    }
}