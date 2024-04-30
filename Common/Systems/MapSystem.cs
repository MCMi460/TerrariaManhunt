using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.Map;

namespace TerrariaManHunt.Common
{
    public class MapSystem : ModSystem
    {
        public override void PreDrawMapIconOverlay(IReadOnlyList<IMapLayer> layers, MapOverlayDrawContext mapOverlayDrawContext)
        {
            foreach (var layer in layers)
            {
                layer.Hide();
            }
        }
    }
}
