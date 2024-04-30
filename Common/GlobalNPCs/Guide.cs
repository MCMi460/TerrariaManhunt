using Terraria;
using Terraria.ModLoader;

namespace TerrariaManhunt.Common.GlobalNPCs
{
    public class Guide : GlobalNPC
    {
        public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        {
            return npc.TypeName != "Guide";
        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            return npc.TypeName != "Guide";
        }
    }
}
