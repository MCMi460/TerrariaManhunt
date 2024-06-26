using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Renderers;
using Terraria.Initializers;
using Terraria.ModLoader;

namespace Terraria_Manhunt
{
    public partial class Terraria_Manhunt : Mod
    {
        public static bool shownMultiplayerMessage = false;

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
                IL_Projectile.Damage += HookTeamCheck;
                IL_Player.ItemCheck_MeleeHitPVP += HookTeamCheck;
            }
            if (settings.HideHealthBars)
            {
                IL_Main.DrawInterface_14_EntityHealthBars += HookDrawHealthBars;
                IL_Main.DrawMouseOver += HookHideHoverHealth;
            }
            if (settings.MasterDrops)
            {
                // I doubt all of these are necessary
                IL_Conditions.IsExpert.CanDrop += HookCanMaster;
                IL_Conditions.IsMasterMode.CanDrop += HookCanMaster;
                IL_Conditions.NotExpert.CanDrop += HookCantMaster;
                IL_Conditions.NotMasterMode.CanDrop += HookCantMaster;
                IL_Player.DropFromItem += HookEnableLoot;
                IL_NPC.NPCLoot_DropItems += HookEnableLoot;
                // The ones below are the more important ones
                IL_DropBasedOnMasterAndExpertMode.CanDrop += HookDropMaster;
                IL_DropBasedOnMasterAndExpertMode.TryDroppingItem_DropAttemptInfo_ItemDropRuleResolveAction += HookDropMaster;
                IL_DropBasedOnExpertMode.CanDrop += HookDropExpert;
                IL_DropBasedOnExpertMode.TryDroppingItem_DropAttemptInfo_ItemDropRuleResolveAction += HookDropExpert;
                IL_DropBasedOnMasterMode.CanDrop += HookDropMaster;
                IL_DropBasedOnMasterMode.TryDroppingItem_DropAttemptInfo_ItemDropRuleResolveAction += HookDropMaster;
            }
            if (settings.AnnounceAchievements)
            {
                IL_AchievementInitializer.OnAchievementCompleted += ILHookAchievementComplete;
                // Prevent saving -- much safer and more sane way to handle this
                IL_AchievementManager.Save_string_bool += ILHookAchievementSave;
            }
            if (settings.ForcePvP)
            {
                IL_Main.DrawPVPIcons += ILHookEndPvPChange;
            }
            if (settings.LuckBoost > 0f)
            {
                //MonoModHooks.Modify(typeof(Player).GetMethod("get_NormalizedLuck"), ILHookLuckClamp);
            }
        }

        public override void Unload()
        {
            // Prevent permeance
            Main.Achievements.Load();
        }
    }
}