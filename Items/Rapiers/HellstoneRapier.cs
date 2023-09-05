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
using vermage.Systems.Utilities;

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

            Item.crit = 7;

            Item.mana = 15;

            base.SetDefaults();
        }

        public override DuelData GetDuelData()
        {
            return new DuelData()
                .SetDamage(24)
                .SetKnockback(4f)
                .SetUseTime(64)
                .SetFirstAttack(ProjectileType<HellstoneSlashProjectile>())
                .SetSecondAttack(ProjectileType<HellstoneJabProjectile>());
        }
        public override SpellData GetSpellData()
        {
            return SpellData.Jolt()
                .SetDamage(1.83f)
                .SetCastTime((int)(60f * 1.8f))
                .SetKnockback(7f)
                .SetProjectileSpeed(7f);
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
