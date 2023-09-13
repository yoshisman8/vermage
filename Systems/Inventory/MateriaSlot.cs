using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using vermage.Items.Materia;

namespace vermage.Systems.Inventory
{
    internal class MateriaSlot : ModAccessorySlot
    {
        public override bool DrawVanitySlot => false;
        public override bool DrawDyeSlot => false;
        public override string FunctionalTexture => "vermage/Assets/UI/MateriaSlot";

        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
        {
            return checkItem.ModItem is Materia && Player.GetModPlayer<VerPlayer>().MaterialColors.Count < 3;
        }
        public override void OnMouseHover(AccessorySlotType context)
        {
            Main.hoverItemName = Language.GetTextValue("Mods.vermage.Tooltips.MateriaSlot");
            Main.mouseText = true;
        }
    }
}
