using Terraria;
using Terraria.ModLoader;

namespace Terraria_Manhunt.Common.GlobalNPCs
{
    public class Guide : GlobalNPC
    {
        public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        {
            if (!ModContent.GetInstance<TerrariaManhuntSettings>().HurtNPCs || npc.TypeName == "Guide")
            {
                return base.CanBeHitByItem(npc, player, item);
            }
            return true;
        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            if (!ModContent.GetInstance<TerrariaManhuntSettings>().HurtNPCs || npc.TypeName == "Guide")
            {
                return base.CanBeHitByProjectile(npc, projectile);
            }
            return true;
        }
    }
}
