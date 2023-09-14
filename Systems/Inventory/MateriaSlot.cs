using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using vermage.Items.Materia;

namespace vermage.Systems.Inventory
{
    public class MateriaSlot : ModAccessorySlot
    {
        public override bool DrawVanitySlot => false;
        public override bool DrawDyeSlot => false;
        public override string FunctionalTexture => "vermage/Assets/UI/MateriaSlotFront";
        public override string FunctionalBackgroundTexture => "vermage/Assets/UI/MateriaSlotBack";
        public override Vector2? CustomLocation
        {
            get
            {
                return VerConfig.Instance.GetMateriaSlotPosition();
            }
        }
        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
        {
            return context == AccessorySlotType.FunctionalSlot && checkItem.ModItem is Materia && Player.GetModPlayer<VerPlayer>().MaterialColors.Count < 3;
        }
        public override void OnMouseHover(AccessorySlotType context)
        {
            Main.hoverItemName = Language.GetTextValue("Mods.vermage.Tooltips.MateriaSlot");
            Main.mouseText = true;
        }
        
    }
}
