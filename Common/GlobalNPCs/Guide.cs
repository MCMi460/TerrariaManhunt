using Terraria;
using Terraria.ModLoader;

namespace Terraria_Manhunt.Common.GlobalNPCs
{
    public class Guide : GlobalNPC
    {
        public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        {
            if (ModContent.GetInstance<TerrariaManhuntSettings>().HurtNPCs && npc.townNPC && npc.TypeName != "Guide")
            {
                return true;
            }
            return base.CanBeHitByItem(npc, player, item);
        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            if (ModContent.GetInstance<TerrariaManhuntSettings>().HurtNPCs && npc.townNPC && npc.TypeName != "Guide")
            {
                return true;
            }
            return base.CanBeHitByProjectile(npc, projectile);
        }
    }
}
