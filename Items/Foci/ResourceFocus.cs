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
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using vermage.Buffs;
using vermage.Projectiles.Foci;
using vermage.Systems;

namespace vermage.Items.Foci
{
    public class ResourceFocus : BaseFoci
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Blue;
            Item.value = 100;
            Item.buffType = ModContent.BuffType<ResourceFocusBuff>();
            Item.shoot = ModContent.ProjectileType<ResourceFocusProjectile>();
            ActivationCost = 3;
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

            foreach (var p in VerUtils.FindAllNearbyPlayers(player, 300, false))
            {
                
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            tooltips.Add(new TooltipLine(Mod, "Tooltip0", Terraria.Localization.Language.GetTextValue("Mods.vermage.ItemTooltip.ResourceFocusActive")));

            if (ModLoader.HasMod("CalamityMod"))
            {
                tooltips.Add(new TooltipLine(Mod, "Tooltip1", Terraria.Localization.Language.GetTextValue("Mods.vermage.ItemTooltip.ResourceFocusActiveCalamity")));
            }
            if (ModLoader.HasMod("ThoriumMod"))
            {
                tooltips.Add(new TooltipLine(Mod, "Tooltip2", Terraria.Localization.Language.GetTextValue("Mods.vermage.ItemTooltip.ResourceFocusActiveThorium")));
            }
            if (ModLoader.HasMod("DBZMODPORT"))
            {
                tooltips.Add(new TooltipLine(Mod, "Tooltip3", Terraria.Localization.Language.GetTextValue("Mods.vermage.ItemTooltip.ResourceFocusActiveDBT")));
            }
            if (ModLoader.HasMod("KaiokenMod"))
            {
                tooltips.Add(new TooltipLine(Mod, "Tooltip4", Terraria.Localization.Language.GetTextValue("Mods.vermage.ItemTooltip.ResourceFocusActiveKaioken")));
            }
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

        public override void OnHitNPC(Projectile projectile, int damage, NPC target)
        {
            
        }

        public override void OnHitPlayer(Projectile projectile, int damage, Player player)
        {
            
        }

        public override void OnCast(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
        {
            
        }
    }
}
