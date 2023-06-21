using IL.Terraria.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
    public class HealingFocus : BaseFoci
    {
        private int BaseHeal;
        private int RegenDuration;
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Blue;
            Item.value = 100;
            Item.buffType = ModContent.BuffType<HealingFocusBuff>();
            Item.shoot = ModContent.ProjectileType<HealingFocusProjectile>();
            ActivationCost = 3;
            BaseHeal = 3;
            RegenDuration = 5;
        }
        public override void Activate(Player player)
        {
            base.Activate(player);

            for (int i = 0; i < 50; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust d = Dust.NewDustPerfect(player.Center, 106, speed * 5, Scale: 1.5f);
                d.noGravity = true;
            }

            int bonus = 0;

            if (ModLoader.HasMod("ThoriumMod"))
            {
                bonus = (int)vermage.ThoriumMod.Call("GetHealBonus", player);
            }

            foreach (var p in VerUtils.FindAllNearbyPlayers(player, 300))
            {
                
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            int bonus = 0;

            if (ModLoader.HasMod("ThoriumMod"))
            {
                bonus = (int)vermage.ThoriumMod.Call("GetHealBonus", Main.CurrentPlayer);
            }

            int index = tooltips.FindIndex(x => x.Name == "Tooltip0" && x.Mod == "Terraria");

            tooltips.Insert(index+1,new TooltipLine(Mod, "Tooltip1", Terraria.Localization.Language.GetTextValue("Mods.vermage.Tooltips.Regen", (BaseHeal + bonus) * RegenDuration, RegenDuration)));
        }
        public override void AddRecipes()
        {
            
            if (ModLoader.HasMod("ThoriumMod"))
            {
                int Quartz = vermage.ThoriumMod.Find<ModItem>("LifeQuartz").Type;

                CreateRecipe()
                .AddIngredient(Quartz, 5)
                .AddIngredient(ItemID.Ruby, 3)
                .AddTile(TileID.Anvils)
                .Register();
            }
            else
            {
                CreateRecipe()
                .AddIngredient(ItemID.LifeCrystal, 2)
                .AddIngredient(ItemID.Ruby, 3)
                .AddTile(TileID.Anvils)
                .Register();
            }
        }
    }
}
