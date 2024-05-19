using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Terraria_Manhunt
{
    public class TerrariaManhuntSettings : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;


        [Header("Visuals")]


        [Label("[i:264] [c/32FF82:Hide player heads on map]")]
        [Tooltip("Player heads that do not share a team with you are hidden on the map.\n"
            + "On: Unfriendly player heads are hidden.\n"
            + "Off: Default Terraria rules are followed.")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool HidePlayers { get; set; }

        [Label("[i:29] [c/32FF82:Hide other players' healthbars]")]
        [Tooltip("This includes teamed players.\n"
            + "On: Other players' healthbars are hidden.\n"
            + "Off: Default Terraria rules are followed.")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool HideHealthBars { get; set; }

        [Label("[i:1175] [c/32FF82:Hide death markers on map]")]
        [Tooltip("Still shows death markers of players on the same team.\n"
            + "On: Unfriendly death markers are hidden.\n"
            + "Off: Default Terraria rules are followed.")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool HideDeathMarkers { get; set; }

        [Label("[i:867] [c/32FF82:Hide NPC heads on map]")]
        [Tooltip("So that the whereabouts of NPCs are not public knowledge.\n"
            + "On: All NPCs are hidden from the map entirely.\n"
            + "Off: Default Terraria rules are followed.")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool HideNPCs { get; set; }

        [Label("[i:4084] [c/32FF82:Announce achievements in chat]")]
        [Tooltip("Achievements that players receive will be publicly noted in chat.\n"
            + "(This will also temporarily reset your achievement progress every session)\n"
            + "On: Achievement get messages appear universally.\n"
            + "Off: Default Terraria rules are followed.")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool AnnounceAchievements { get; set; }

        /*
        [Label("[i:893] [c/32FF82:Share hunter map data]")]
        [Tooltip("Players on a team will share new map data.\n"
            + "On: New map data updates apply to all teamed players.\n"
            + "Off: Default Terraria rules are followed.")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool ShareMap { get; set; }
        */


        [Header("Damage")]


        [Label("[i:4] [c/FF1919:Force PvP]")]
        [Tooltip("All players join with PvP automatically enabled.\n"
            + "On: PvP is eternally on.\n"
            + "Off: Default Terraria rules are followed.")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool ForcePvP { get; set; }

        [Label("[i:5095] [c/FF1919:Enable damageable NPCs] [c/327DFF:(except for the Guide)]")]
        [Tooltip("This is meant to balance the nurse.\n"
            + "On: All town NPCs can be hurt by players.\n"
            + "Off: Default Terraria rules are followed.")]
        [DefaultValue(true)]
        public bool HurtNPCs { get; set; }

        [Label("[i:795] [c/FF1919:Enable friendly-fire]")]
        [Tooltip("Players that share a team with you can now be caught in the cross-fire.\n"
            + "On: All players that share a team can hurt each other\n"
            + "Off: Default Terraria rules are followed.")]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool FriendlyFire { get; set; }

        [Label("[i:3322] [c/FF1919:Drop Master Mode items]")]
        [Tooltip("This allows Master Mode items to drop, even when on a Normal Mode world.\n"
            + "On: Master Mode lootbags/items drop.\n"
            + "Off: Default Terraria rules are followed.")]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool MasterDrops { get; set; }


        [Header("Miscellaneous")]


        [Label("[i:2351] [c/FF00A0:Disable Teleportation Potions]")]
        [Tooltip("Teleportation Potions' effects are replaced with those of a Recall Potion.\n"
            + "On: Teleportation Potions act like Recall Potions.\n"
            + "Off: Default Terraria rules are followed.")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool DisableTelePot { get; set; }

        [Label("[i:224] [c/FF00A0:Disallow target from setting spawn]")]
        [Tooltip("The target can no longer set their spawn point for free teleportation.\n"
            + "On: Target cannot set their spawn point.\n"
            + "Off: Default Terraria rules are followed.")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool DisallowSpawn { get; set; }
    }
}