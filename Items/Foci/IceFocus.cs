using IL.Terraria.Localization;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using vermage.Buffs;
using vermage.Projectiles.Foci;
using vermage.Systems;

namespace vermage.Items.Foci
{
    public class IceFocus : BaseFoci
    {

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Blue;
            Item.value = 100;
            Item.buffType = ModContent.BuffType<IceFocusBuff>();
            Item.shoot = ModContent.ProjectileType<IceFocusProjectile>();
            ActivationCost = 1;
        }
        public override void AddRecipes()
        {
            
            if (ModLoader.HasMod("ThoriumMod"))
            {
                int IceShard = vermage.ThoriumMod.Find<ModItem>("IcyShard").Type;

                CreateRecipe()
                .AddIngredient(IceShard, 7)
                .AddIngredient(ItemID.Glass, 5)
                .AddTile(TileID.WorkBenches)
                .Register();
            }
            else
            {
                CreateRecipe()
                .AddIngredient(ItemID.Sapphire, 4)
                .AddIngredient(ItemID.Glass, 5)
                .AddTile(TileID.WorkBenches)
                .Register();
            }
        }
    }
}
