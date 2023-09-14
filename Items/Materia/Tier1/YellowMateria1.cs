using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Enums;
using Terraria;
using vermage.Systems;
using Terraria.ModLoader;
using Terraria.Localization;

namespace vermage.Items.Materia.Tier1
{
    public class YellowMateria1 : Materia
    {
        public override MateriaColor Color => MateriaColor.Yellow;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);

            player.GetModPlayer<VerPlayer>().BuffDuration += 0.10f;

            if (ModLoader.HasMod("ThoriumMod")) 
            {
                vermage.ThoriumMod.Call("BonusHealerHealBonus", player, 1);
            }
        }
        public LocalizedText ThoriumSupport => this.GetLocalization("ThoriumSupport");
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = (int)ItemRarityColor.Blue1;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            if (ModLoader.HasMod("ThoriumMod"))
            {
                tooltips.Add(new(Mod,nameof(ThoriumSupport), ThoriumSupport.Value));
            }
        }
    }
}
