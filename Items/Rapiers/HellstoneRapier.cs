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
using vermage.Systems.Utilities;
using vermage.Items.Abstracts;
using vermage.Projectiles.Foci;
using vermage.Projectiles.Guards;
using vermage.Systems;

namespace vermage.Items.Rapiers
{
    public class HellstoneRapier : BaseRapier
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 70;
            Item.height = 70;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.Orange;

            Item.crit = 7;
            Item.damage = 24;
            Item.knockBack = 4f;
            Item.useTime = 64;
            Item.useAnimation = 64;

            RapierProjectile = ProjectileType<HellstoneRapierProjectile>();
            GuardProjectile = ProjectileType<HellstoneRapierGuardProjectile>();
            FocusProjectile = ProjectileType<ExampleFocusProjectile>();

            Item.shoot = RapierProjectile;
        }

        public override void HoldItem(Player player)
        {
            base.HoldItem(player);
            VerPlayer vplayer = player.GetModPlayer<VerPlayer>();

            if (!vplayer.HasSpellUnloked("vermage/Fire"))
            {
                vplayer.UnlockSpell("vermage/Fire");
                vplayer.Slot2 = "vermage/Fire";
                Main.NewText(Language.GetTextValue("Mods.vermage.Messages.UnlockMessage", vermage.Spells["vermage/Fire"].Name.Value));
            }
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
