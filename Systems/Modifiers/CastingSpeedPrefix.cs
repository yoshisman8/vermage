using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace vermage.Systems.Modifiers
{
    public class CastingSpeedPrefix : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;
        

        public virtual float Power => 1f;
        public LocalizedText Tooltip => this.GetLocalization("AdditionalTooltip");

        public override void ApplyAccessoryEffects(Player player)
        {
            player.GetModPlayer<VerPlayer>().CastingSpeed -= 0.02f * Power;
        }
        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            yield return new TooltipLine(Mod, Name, Tooltip.Format((int)Math.Floor(0.02f * Power))) { IsModifier = true };
        }
    }
}
