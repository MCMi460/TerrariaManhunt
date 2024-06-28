using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria_Manhunt.Common.Players;

namespace Terraria_Manhunt
{
    public partial class Terraria_Manhunt : Mod
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
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
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
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
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
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
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
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
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
                c.EmitDelegate<Func<int, bool>>(i => Main.player[i].GetModPlayer<ManhuntPlayer>().trackedPlayer == i);

                // If not, move on to set the spawn point
                c.Emit(Mono.Cecil.Cil.OpCodes.Brfalse, label);
                // If so, prematurely end the change spawn method
                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);

                c.MarkLabel(label);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
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
                c.EmitDelegate<Func<int, bool>>(i => Main.player[i].GetModPlayer<ManhuntPlayer>().trackedPlayer == i);

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
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
            }
        }

        // Melee weapons and projectiles now hit friendly players
        public static void HookTeamCheck(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.GotoNext(i => i.MatchLdfld<Player>("team"));
                c.Index++;

                // Pops the team on the stack
                c.Emit(Mono.Cecil.Cil.OpCodes.Pop);
                // And replaces it with the default team
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, 0);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
            }
        }

        // Hide all players' health bars
        public static void HookDrawHealthBars(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.GotoNext(i => i.MatchLdfld<Player>("invis"));
                c.Index++;

                // Pops the invis stat on the stack
                c.Emit(Mono.Cecil.Cil.OpCodes.Pop);
                // And replaces it with true
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, 1);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
            }
        }

        // Hides player health text on hover
        public static void HookHideHoverHealth(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.GotoNext(i => i.MatchLdflda<Player>("statLifeMax2"));
                c.Index += 5;

                c.Emit(Mono.Cecil.Cil.OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.player)));
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_S, (byte)6);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldelem_Ref);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldfld, typeof(Player).GetField(nameof(Player.name)));

                c.Emit(Mono.Cecil.Cil.OpCodes.Stloc_S, (byte)9);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
            }
        }

        // Returns true
        public static void HookCanMaster(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, 1);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
            }
        }

        // Returns false
        public static void HookCantMaster(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, 0);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
            }
        }

        // Enables loot
        public static void HookEnableLoot(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.GotoNext(i => i.MatchCall<Main>("get_expertMode"));
                c.Index++;

                c.Emit(Mono.Cecil.Cil.OpCodes.Pop);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, 1);

                c.GotoNext(i => i.MatchCall<Main>("get_masterMode"));
                c.Index++;

                c.Emit(Mono.Cecil.Cil.OpCodes.Pop);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, 1);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
            }
        }

        // Enable Master
        public static void HookDropMaster(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.GotoNext(i => i.MatchLdfld<DropAttemptInfo>("IsMasterMode"));
                c.Index++;

                c.Emit(Mono.Cecil.Cil.OpCodes.Pop);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, 1);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
            }
        }

        // Enable Expert
        public static void HookDropExpert(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.GotoNext(i => i.MatchLdfld<DropAttemptInfo>("IsExpertMode"));
                c.Index++;

                c.Emit(Mono.Cecil.Cil.OpCodes.Pop);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, 1);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
            }
        }

        // Rewrite of achievement announcement method
        public static void ILHookAchievementComplete(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.GotoNext(i => i.MatchCall(typeof(Terraria.Localization.Language).GetMethod(
                    nameof(Terraria.Localization.Language.GetTextValue), new Type[] { typeof(string), typeof(object) })));
                c.Index++;

                c.Emit(Mono.Cecil.Cil.OpCodes.Newobj, typeof(ChatMessage).GetConstructor(new Type[] { typeof(string) }));

                c.Emit(Mono.Cecil.Cil.OpCodes.Call, typeof(ChatHelper).GetMethod("SendChatMessageFromClient"));
                
                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
            }
        }

        // Instantly end writing to achievements.dat
        public static void ILHookAchievementSave(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
            }
        }

        // Disables changing of PvP status
        public static void ILHookEndPvPChange(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.GotoNext(i => i.MatchLdsfld<Main>("mouseLeft"));
                c.Index += 1;

                c.Emit(Mono.Cecil.Cil.OpCodes.Pop);
                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, 0);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
            }
        }

        // Modifies normalized luck ... unused
        public static void ILHookLuckClamp(ILContext il)
        {
            try
            {
                var c = new ILCursor(il);

                c.Emit(Mono.Cecil.Cil.OpCodes.Ldc_R4, 50.0f);

                c.Emit(Mono.Cecil.Cil.OpCodes.Ret);
            }
            catch (Exception e)
            {
                throw new ILPatchFailureException(ModContent.GetInstance<Terraria_Manhunt>(), il, e);
            }
        }
    }
}