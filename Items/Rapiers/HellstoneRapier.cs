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
using vermage.Projectiles.Rapiers;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.Enums;
using Microsoft.Xna.Framework.Content;
using System.Security.Policy;
using vermage.Projectiles.Swings;
using vermage.Projectiles.Swings.HellstoneRapier;
using vermage.Projectiles.Spells;

namespace vermage.Items.Rapiers
{
    public class HellstoneRapier : BaseRapier
    {
        public override void SetDefaults()
        {

            Item.width = 70;
            Item.height = 70;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.Orange;

            Item.autoReuse = true;

            Item.shoot = ProjectileType<HellstoneRapierProjectile>();
            Item.shootSpeed = 20f;

            Item.damage = 24;
            Item.knockBack = 4f;
            Item.crit = 7;
            
            MeleeUseTime = 70;

            MageShoot = ProjectileType<JoltProjectile>();
            MageShootSpeed = 7f;
            MageKnockback = 5f;
            MageDamage = 6;
            MageUseTime = 15;
            CastTime = (int)(60f * 1.5f);

            Item.mana = 2;
            ComboSteps = 2;
            SlashProjectile = ProjectileType<HellstoneSlashProjectile>();
            JabProjectile = ProjectileType<HellstoneJabProjectile>();

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
