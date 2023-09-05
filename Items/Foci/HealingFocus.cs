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
using Terraria.Localization;
using Terraria.ModLoader;
using vermage.Buffs;
using vermage.Projectiles.Foci;
using vermage.Systems;

namespace vermage.Items.Foci
{
    public class HealingFocus : BaseFoci
    {
        private int BaseHeal;
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Blue;
            Item.value = 100;
            Item.buffType = ModContent.BuffType<HealingFocusBuff>();
            Item.shoot = ModContent.ProjectileType<HealingFocusProjectile>();
            ActivationCost = 3;
            BaseHeal = 3;
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

            foreach (var p in VerUtils.FindAllNearbyPlayers(player, 4000, false))
            {
                if(p.team == player.team)
                {
                    p.AddBuff(ModContent.BuffType<CureBuff>(), 60 * 5, false);
                }
            }
            player.GetModPlayer<VerPlayer>().FociFramesLeft = 60 * 5;
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

            tooltips.Insert(index+1,new TooltipLine(Mod, "Tooltip1", Terraria.Localization.Language.GetTextValue("Mods.vermage.ItemTooltip.HealingFocusPassive", BaseHeal + bonus)));

            tooltips.Add(new TooltipLine(Mod, "Tooltip0", Terraria.Localization.Language.GetTextValue("Mods.vermage.ItemTooltip.HealingFocusActive")));
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

        public override void OnCast(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
        {
            
        }

        public override void OnHitNPC(Projectile projectile, NPC Target, NPC.HitInfo hitInfo, int damageDealt)
        {
            if (damageDealt > 0)
            {
                Player owner = Main.player[projectile.owner];

                int bonus = 0;

                if (ModLoader.HasMod("ThoriumMod"))
                {
                    bonus = (int)vermage.ThoriumMod.Call("GetHealBonus", owner);
                }

                foreach (var p in VerUtils.FindAllNearbyPlayers(owner, 1000, false))
                {
                    if (p.team == owner.team)
                    {
                        p.Heal(BaseHeal + bonus);
                    }
                }
            }
        }

        public override void OnHitPlayer(Projectile projectile, Player Target, Player.HurtInfo hitInfo, int damageDealt)
        {
            if (damageDealt > 0)
            {
                Player owner = Main.player[projectile.owner];

                int bonus = 0;

                if (ModLoader.HasMod("ThoriumMod"))
                {
                    bonus = (int)vermage.ThoriumMod.Call("GetHealBonus", owner);
                }

                foreach (var p in VerUtils.FindAllNearbyPlayers(owner, 1000, false))
                {
                    if (p.team == owner.team)
                    {
                        p.Heal(BaseHeal + bonus);
                    }
                }
            }
        }
    }
}
