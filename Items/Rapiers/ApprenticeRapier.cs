using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.Enums;
using Microsoft.Xna.Framework.Content;
using System.Security.Policy;
using vermage.Projectiles.Rapiers;

namespace vermage.Items.Rapiers
{
    public class ApprenticeRapier : BaseRapier
    {
        
        public override void SetDefaults()
        {

            Item.width = 36;
            Item.height = 36;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.Blue;

            Item.autoReuse = true;

            Item.shoot = ProjectileType<ApprenticeRapierProjectile>();
            Item.shootSpeed = 20f;

            Item.damage = 9;
            Item.knockBack = 4f;
            Item.crit = 4;
            
            MeleeUseTime = 20;

            MageShoot = ProjectileID.Spark;
            MageShootSpeed = 7f;
            MageKnockback = 0f;
            MageDamage = 6;
            MageUseTime = 26;

            Item.mana = 2;

            ComboSteps = 1;
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 5)
                .AddIngredient(ItemID.ManaCrystal, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 5)
                .AddIngredient(ItemID.ManaCrystal, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
