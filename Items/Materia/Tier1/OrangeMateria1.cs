using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Enums;
using Terraria;
using vermage.Systems;

namespace vermage.Items.Materia.Tier1
{
    public class OrangeMateria1 : Materia
    {
        public override MateriaColor Color => MateriaColor.Orange;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);

            player.GetModPlayer<VerPlayer>().WhiteManaGainRate += 0.10f;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = (int)ItemRarityColor.Blue1;
        }
    }
}
