using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace vermage.Systems.Inventory
{
    public class MateriaSlot3 : MateriaSlot
    {
        public override bool DrawFunctionalSlot => Player.GetModPlayer<VerPlayer>().UnlockedMateriaSlot3;

        public override Vector2? CustomLocation 
        { 
            get
            {
                var parent = ModContent.GetInstance<MateriaSlot2>();

                return parent.CustomLocation + new Vector2(0, 50);
            } 
        }
    }
}
