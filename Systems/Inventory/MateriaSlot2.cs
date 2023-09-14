using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace vermage.Systems.Inventory
{
    public class MateriaSlot2 : MateriaSlot
    {
        public override bool DrawFunctionalSlot => Player.GetModPlayer<VerPlayer>().UnlockedMateriaSlot2;

        public override Vector2? CustomLocation 
        { 
            get
            {
                var parent = ModContent.GetInstance<MateriaSlot>();

                return parent.CustomLocation + new Vector2(0, 50);
            } 
        }
    }
}
