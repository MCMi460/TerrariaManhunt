﻿using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ModLoader;
using TerrariaManhunt.Common.Players;

namespace TerrariaManhunt
{
    public partial class TerrariaManhunt : Mod
    {
        public static void HookDrawPlayerHead(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.GotoNext(i => i.MatchLdarg0());
                var label = il.DefineLabel();

                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_2);
                // Check if player is on the same team
                c.EmitDelegate<Func<Player, bool>>(player =>
                    (player.team != Main.CurrentPlayer.team) ||
                    (player.team == 0 && player.whoAmI != Main.myPlayer)
                );
                // If so, go to label and continue function as normal
                c.Emit(Mono.Cecil.Cil.OpCodes.Brfalse, label);
                // Otherwise, prematurely end method
                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);

                c.MarkLabel(label);
            }
            catch (Exception e)
            {
                MonoModHooks.DumpIL(ModContent.GetInstance<TerrariaManhunt>(), il);
                throw new ILPatchFailureException(ModContent.GetInstance<TerrariaManhunt>(), il, e);
            }
        }

        public static void HookDrawNPCHead(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                // Prematurely end method to prevent rendering NPC Map heads
                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<TerrariaManhunt>(), il, e);
            }
        }

        /*
         * Hides death markers on fullscreen map from players on other teams
         * Note: Does not hide minimap TextureAssets.MapDeath
         * See: HookDrawMinimapMarkers
        */
        public static void HookDrawDeathMarker(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                var label = il.DefineLabel();

                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_S, (byte)6);

                // Check if player is on the same team
                c.EmitDelegate<Func<int, bool>>(i => Main.player[i].team != Main.CurrentPlayer.team);
                // If so, go to label and continue function as normal
                c.Emit(Mono.Cecil.Cil.OpCodes.Brfalse, label);
                // Otherwise, prematurely end method
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, 0);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);

                c.MarkLabel(label);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<TerrariaManhunt>(), il, e);
            }
        }

        // Hides minimap icons, regardless of team
        public static void HookDrawMinimapMarkers(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.GotoNext(i => i.MatchLdfld<Player>("showLastDeath"));
                c.Index++;

                // Pops the "showLastDeath" boolean from stack
                c.Emit(Mono.Cecil.Cil.OpCodes.Pop);
                // Replaces it with "false"
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, 0);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<TerrariaManhunt>(), il, e);
            }
        }

        // Disables setting of the target's spawn point
        public static void HookDisallowSpawn(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                var label = il.DefineLabel();

                // Load the index in Main.player array
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldfld, typeof(Player).GetField(nameof(Player.whoAmI)));

                // Check if the current player is the target
                c.EmitDelegate<Func<int, bool>>(i => Main.player[i].GetModPlayer<TrackedPlayerSync>().trackedPlayer == i);

                // If not, move on to set the spawn point
                c.Emit(Mono.Cecil.Cil.OpCodes.Brfalse, label);
                // If so, prematurely end the change spawn method
                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);

                c.MarkLabel(label);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<TerrariaManhunt>(), il, e);
            }
        }

        // Converts teleportation potions into bottled shellphones
        public static void HookTelePot(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                var label = il.DefineLabel();

                // Load the index in Main.player array
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldfld, typeof(Player).GetField(nameof(Player.whoAmI)));

                // Check if the current player is the target
                c.EmitDelegate<Func<int, bool>>(i => Main.player[i].GetModPlayer<TrackedPlayerSync>().trackedPlayer == i);

                // If not, move on to use the teleportation potion
                c.Emit(Mono.Cecil.Cil.OpCodes.Brfalse, label);
                // If so, call the shellphone method
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
                c.Emit(Mono.Cecil.Cil.OpCodes.Call, typeof(Player).GetMethod("Shellphone_Spawn"));
                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);

                c.MarkLabel(label);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<TerrariaManhunt>(), il, e);
            }
        }
    }
}