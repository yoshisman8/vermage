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
            FullName = FullName,
            ItemType = Type,
            UseTime = Item.useTime,
            Damage = Item.damage,
            Knockback = Item.knockBack,
            RapierProjectile = RapierProjectile,
            FocusProjectile = FocusProjectile
        };

        public int RapierProjectile = -1;
        public int FocusProjectile = -1;
        public int GuardProjectile = -1;

        private int Rapier = -1;
        public override void SetDefaults()
        {
            Item.DamageType = GetInstance<VermilionDamageClass>();
            Item.useTime = 1;
            Item.useAnimation = Item.useTime;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = null;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
        }
        public override void SetStaticDefaults()
        {
            vermage.Rapiers.Add(FullName, RapierData);
        }
        public override void HoldItem(Player player)
        {
            base.HoldItem(player);

            VerPlayer vplayer = player.GetModPlayer<VerPlayer>();

            if (vplayer.RapierBehavior != Behavior.Idle && vplayer.LastRapierUsage.TryGetValue(FullName, out var date))
            {
                if ((DateTime.Now - date).TotalSeconds > 1)
                {
                    vplayer.RapierBehavior = Behavior.Idle;
                    vplayer.BehaviorFrames = 0;
                }
            }
            else
            {
                vplayer.LastRapierUsage.Add(FullName, DateTime.Now);
            }

            if (vplayer.RapierBehavior == Behavior.Idle || vplayer.RapierBehavior == Behavior.Casting)
            {
                Item.useTime = 1;
                Item.useAnimation = 1;
            }
            else
            {
                Item.useAnimation = RapierData.UseTime;
                Item.useTime = (int)(RapierData.UseTime * player.GetAttackSpeed<MeleeDamageClass>());
            }

            if (player.ownedProjectileCounts[RapierProjectile] < 1 && vplayer.RapierBehavior == Behavior.Idle)
            {
                Rapier = Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), player.position, new Vector2(0), RapierProjectile, Item.damage, Item.knockBack, player.whoAmI, (int)Behavior.Idle).whoAmI;
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
        public override float UseSpeedMultiplier(Player player) => 1;
        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            VerPlayer verPlayer = player.GetModPlayer<VerPlayer>();

            if (player.altFunctionUse == 2)
            {
                var spell = verPlayer.GetCurrentSpell();
                if (spell.HasValue && !verPlayer.IsCasting)
                {
                    verPlayer.IsCasting = true;
                    verPlayer.WasCasting = true;
                    verPlayer.RapierBehavior = Behavior.Casting;
                    verPlayer.BehaviorFrames = (int)verPlayer.CastingSpeed.ApplyTo(spell.Value.CastingTime);
                }
                else if (spell.HasValue && verPlayer.IsCasting && verPlayer.CastingTimer == verPlayer.BehaviorFrames)
                {
                    Projectile.NewProjectileDirect(source, verPlayer.FocusPosition ?? position, spell.Value.Velocity, spell.Value.ProjectileType, (int)spell.Value.Damage.ApplyTo(damage), spell.Value.Knockback.ApplyTo(knockback), (int)spell.Value.Color);
                }
            }
            else
            {
                verPlayer.IsCasting = false;
                verPlayer.WasCasting = false;
                if (verPlayer.RapierBehavior == Behavior.Idle || verPlayer.RapierBehavior == Behavior.Thrust)
                {
                    if (verPlayer.GetCombinedMana() > 0)
                    {
                        verPlayer.RapierBehavior = Behavior.Swing;
                        verPlayer.BehaviorFrames = Item.useAnimation;

                        verPlayer.AddMana(ManaColor.Red, -1);

                        Projectile.NewProjectileDirect(source, position, velocity, RapierData.RapierProjectile, damage, knockback, (int)Behavior.EnchantedSwing);
                    }
                    else
                    {
                        verPlayer.RapierBehavior = Behavior.Swing;
                        verPlayer.BehaviorFrames = Item.useAnimation;

                        Projectile.NewProjectileDirect(source, position, velocity, RapierData.RapierProjectile, damage, knockback, player.whoAmI, (int)Behavior.Swing);
                    }
                }
                else if (verPlayer.RapierBehavior == Behavior.Swing)
                {
                    if (verPlayer.GetCombinedMana() > 0)
                    {
                        verPlayer.RapierBehavior = Behavior.Thrust;
                        verPlayer.BehaviorFrames = Item.useAnimation;

                        verPlayer.AddMana(ManaColor.Red, -1);

                        Projectile.NewProjectileDirect(source, position, velocity, RapierData.RapierProjectile, damage, knockback, (int)Behavior.EnchantedThrust);
                    }
                    else
                    {
                        verPlayer.RapierBehavior = Behavior.Thrust;
                        verPlayer.BehaviorFrames = Item.useAnimation;

                        Projectile.NewProjectileDirect(source, position, velocity, RapierData.RapierProjectile, damage, knockback, player.whoAmI, (int)Behavior.Thrust);
                    }
                }
            }

            if (verPlayer.LastRapierUsage.ContainsKey(FullName)) verPlayer.LastRapierUsage[FullName] = DateTime.Now;
            else verPlayer.LastRapierUsage.Add(FullName, DateTime.Now);

            return false;
        }
    }
}
