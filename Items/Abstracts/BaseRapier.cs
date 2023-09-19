using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using vermage.Projectiles.Rapiers;
using vermage.Systems;
using static Terraria.ModLoader.ModContent;
using static Terraria.ModLoader.PlayerDrawLayer;
using vermage.Systems.Utilities;
using vermage.Systems.Handlers;
using Terraria.Localization;
using Microsoft.CodeAnalysis;

namespace vermage.Items.Abstracts
{
    public abstract class BaseRapier : ModItem
    {
        public RapierData RapierData => new RapierData()
        {
            RapierProjectile = RapierProjectile,
            FocusProjectile = FocusProjectile,
            GuardProjectile = GuardProjectile
        };

        public int RapierProjectile = -1;
        public int FocusProjectile = -1;
        public int GuardProjectile = -1;

        public override void SetDefaults()
        {
            Item.DamageType = GetInstance<VermilionDamageClass>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = null;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = false;
        }
        public override void SetStaticDefaults()
        {
            vermage.Rapiers.Add(FullName, RapierData);
        }
        public override void HoldItem(Player player)
        {
            base.HoldItem(player);

            VerPlayer vplayer = player.GetModPlayer<VerPlayer>();

            if (!vplayer.HasSpellUnloked("vermage/Jolt"))
            {
                vplayer.UnlockSpell("vermage/Jolt");
            }

            Item.autoReuse = player.autoReuseGlove;

            if (player.ownedProjectileCounts[RapierProjectile] < 1 && (vplayer.RapierBehavior == Behavior.Idle || vplayer.RapierBehavior == Behavior.Casting))
            {
                Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), player.position, new Vector2(0), RapierProjectile, Item.damage, Item.knockBack, player.whoAmI, (int)Behavior.Idle);
            }
            if (player.ownedProjectileCounts[FocusProjectile] < 1)
            {
                Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.position, new Vector2(0), FocusProjectile, Item.damage, Item.knockBack, player.whoAmI, (int)Behavior.Idle);
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            VerPlayer player = Main.CurrentPlayer.GetModPlayer<VerPlayer>();

            SpellData? spell = player.GetCurrentSpell();

            if(spell.HasValue)
            {
                int dmgIndex = tooltips.FindIndex(x => x.Mod == "Terraria" && x.Name == "Damage");
                tooltips.Insert(dmgIndex + 1, new TooltipLine(vermage.Instance, "ItemName", Language.GetTextValue("Mods.vermage.Tooltips.RightClick", spell.Value.Name)));
            }

            if (ModLoader.HasMod("ThoriumMod"))
            {
                tooltips.Add(new TooltipLine(vermage.Instance, "Tooltip0", Language.GetTextValue("Mods.vermage.Tooltips.InheritanceThorium")));
            }
            else
            {
                tooltips.Add(new TooltipLine(vermage.Instance, "Tooltip0", Language.GetTextValue("Mods.vermage.Tooltips.Inheritance")));
            }
        }
        public override float UseSpeedMultiplier(Player player) => player.GetAttackSpeed<MeleeDamageClass>();
        public override bool CanUseItem(Player player) => player.GetModPlayer<VerPlayer>().RapierBehavior != Behavior.Casting;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            VerPlayer verPlayer = player.GetModPlayer<VerPlayer>();

            verPlayer.CastingSpell = null;
            verPlayer.WasCasting = false;
            if (verPlayer.RapierBehavior == Behavior.Idle || verPlayer.RapierBehavior == Behavior.Thrust)
            {
                if (verPlayer.GetCombinedMana() > 0)
                {
                    verPlayer.RapierBehavior = Behavior.Swing;

                    verPlayer.AddMana(ManaColor.Red, -1);

                    Projectile.NewProjectileDirect(source, position, velocity, RapierData.RapierProjectile, damage, knockback, player.whoAmI, (int)Behavior.EnchantedSwing, Item.useAnimation);
                }
                else
                {
                    verPlayer.RapierBehavior = Behavior.Swing;

                    Projectile.NewProjectileDirect(source, position, velocity, RapierData.RapierProjectile, damage, knockback, player.whoAmI, (int)Behavior.Swing, Item.useAnimation);
                }
            }
            else if (verPlayer.RapierBehavior == Behavior.Swing)
            {
                if (verPlayer.GetCombinedMana() > 0)
                {
                    verPlayer.RapierBehavior = Behavior.Thrust;

                    verPlayer.AddMana(ManaColor.Red, -1);

                    Projectile.NewProjectileDirect(source, position, velocity, RapierData.RapierProjectile, damage, knockback, player.whoAmI, (int)Behavior.EnchantedThrust, Item.useAnimation);
                }
                else
                {
                    verPlayer.RapierBehavior = Behavior.Thrust;

                    Projectile.NewProjectileDirect(source, position, velocity, RapierData.RapierProjectile, damage, knockback, player.whoAmI, (int)Behavior.Thrust, Item.useAnimation);
                }
            }

            return false;
        }
    }
}
