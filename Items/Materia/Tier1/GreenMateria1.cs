using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Enums;
using vermage.Systems;

namespace vermage.Items.Materia.Tier1
{
    public class GreenMateria1 : Materia
    {
        public override MateriaColor Color => MateriaColor.Green;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);

            player.GetModPlayer<VerPlayer>().doubleFinisherchance += 0.10f;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = (int)ItemRarityColor.Blue1;
        }
    }
}
