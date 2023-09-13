using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.Utilities;
using vermage.Systems;
using vermage.Systems.Inventory;

namespace vermage.Items.Materia
{
    public abstract class Materia : ModItem
    {
        public virtual MateriaColor Color => MateriaColor.Red;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<VerPlayer>().MaterialColors.Add(Color);
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            VerPlayer verPlayer = player.GetModPlayer<VerPlayer>();
            return !verPlayer.MaterialColors.Contains(Color) && verPlayer.MaterialColors.Count < 3 && modded;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.accessory = true;
            Item.Size = new(26);
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return false;
        }
        public override bool AllowPrefix(int pre)
        {
            return false;
        }
    }

    public enum MateriaColor
    {
        Red,
        Green,
        Blue,
        Yellow,
        Purple,
        Orange
    }
}
