using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Terraria_Manhunt
{
    public partial class Terraria_Manhunt : Mod
    {
        public static void SendMessage(string text, Color? color = null)
        {
            if (!color.HasValue)
            {
                color = Color.White;
            }
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text, color);
            }
            else
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color.Value);
            }
        }
    }
}