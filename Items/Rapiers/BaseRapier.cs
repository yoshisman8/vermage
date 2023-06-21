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
using vermage.Projectiles.Swings;
using vermage.Projectiles.Rapiers;
using vermage.Systems;
using static Terraria.ModLoader.ModContent;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace vermage.Items.Rapiers
{
    public abstract class BaseRapier : ModItem
    {
        public int MageShoot;
        public float MageShootSpeed;
        public float MageKnockback;
        public int MageDamage;
        public int MageUseTime;
        public int MeleeUseTime;
        public int CastTime;
        public int ComboSteps = 1;
        public int SlashProjectile;
        public int JabProjectile;
        public int CutProjectile;
        public int StabProjectile;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DamageType = GetInstance<VermilionDamageClass>();
            Item.useTime = MeleeUseTime;
            Item.useAnimation = MeleeUseTime;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
        public override void HoldItem(Player player)
        {
            base.HoldItem(player);

            VerPlayer vplayer = player.GetModPlayer<VerPlayer>();

            if (vplayer.Rapier.HasValue)
            {
                if (vplayer.Rapier.Value != Type) vplayer.ComboState = 0;
            }
            vplayer.Rapier = Type;

            if (player.ownedProjectileCounts[Item.shoot] < 1 && vplayer.AttackFramesLeft <= 0)
            {
                Terraria.Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), player.position, new Vector2(0), Item.shoot, Item.damage, Item.knockBack, player.whoAmI, ai1: Type);
            }

            if (vplayer.IsMageStance)
            {
                Item.useTime = 0;
                Item.useAnimation = 0;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.UseSound = null;
                Item.autoReuse = true;
            }
            else
            {
                Item.useTime = MeleeUseTime;
                Item.useAnimation = MeleeUseTime;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.UseSound = SoundID.Item1;
                Item.autoReuse = true;
            }
            if (vplayer.ComboState >= ComboSteps) vplayer.ComboState = 0;
        }
        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (!player.GetModPlayer<VerPlayer>().IsMageStance && player.altFunctionUse != 2)
            {
                mult = 0f;
            }
        }
        public override float UseSpeedMultiplier(Player player)
        {
            VerPlayer vplayer = player.GetModPlayer<VerPlayer>();
            return vplayer.IsMageStance ? player.GetAttackSpeed(GetInstance<VermilionDamageClass>()) : player.GetAttackSpeed(DamageClass.Melee);
        }
        public int GetUseFrames(Player player)
        {
            int TotalFrames = (int)((Item.useAnimation) / UseSpeedMultiplier(player)) + 5;
            return TotalFrames;
        }

        public override bool AltFunctionUse(Player player) => true;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var actionKey = VerPlayer.ActionKey.GetAssignedKeys();

            bool stance = Main.CurrentPlayer.GetModPlayer<VerPlayer>().IsMageStance;
            
            int nameIndex = tooltips.FindIndex(x => x.Mod == "Terraria" && x.Name == "ItemName");

            if (stance)
            {
                tooltips.Insert(nameIndex + 1, new TooltipLine(vermage.Instance, "ItemName", Terraria.Localization.Language.GetTextValue("Mods.vermage.Tooltips.MageStance")));
            }
            else
            {
                tooltips.Insert(nameIndex + 1, new TooltipLine(vermage.Instance, "ItemName", Terraria.Localization.Language.GetTextValue("Mods.vermage.Tooltips.MeleeStance")));
            }

            int dmgIndex = tooltips.FindIndex(x => x.Mod == "Terraria" && x.Name == "Damage");
            
            tooltips.Insert(dmgIndex + 1, new TooltipLine(vermage.Instance, "ItemName", Terraria.Localization.Language.GetTextValue("Mods.vermage.Tooltips.RightClick")));

            if (ModLoader.HasMod("ThoriumMod"))
            {
                tooltips.Add(new TooltipLine(vermage.Instance, "Tooltip0", Terraria.Localization.Language.GetTextValue("Mods.vermage.Tooltips.Inheritance")));
            }
        }
        public void SwapModes(Player player)
        {
            bool state = player.GetModPlayer<VerPlayer>().IsMageStance;
            if(state)
            {
                CombatText.NewText(player.Hitbox, Color.Red, Terraria.Localization.Language.GetTextValue("Mods.vermage.CombatTexts.MeleeStance"), false, false);
            }
            else
            {
                CombatText.NewText(player.Hitbox, Color.Red, Terraria.Localization.Language.GetTextValue("Mods.vermage.CombatTexts.MageStance"), false, false);
            }
            player.GetModPlayer<VerPlayer>().IsMageStance ^= true;
        }
        public override bool CanUseItem(Player player)
        {
            if(player.altFunctionUse == 2 && Main.mouseRight && Main.mouseRightRelease)
            {
                SwapModes(player);
                return false;
            }
            else
            {
                VerPlayer vPlayer = player.GetModPlayer<VerPlayer>();
                if (vPlayer.IsMageStance)
                {
                    if (vPlayer.IsCasting && vPlayer.CastFrames.Left <= 0)
                    {
                        return true;
                    }
                    else return false;
                }
                else
                {
                    return base.CanUseItem(player);
                }
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            VerPlayer vPlayer = player.GetModPlayer<VerPlayer>();
            if (vPlayer.IsMageStance)
            {
                Vector2 Vel;
                if (vPlayer.FocusPos.HasValue)
                {
                    Vel = vPlayer.FocusPos.Value.DirectionTo(Main.MouseWorld);
                }
                else
                {
                    Vel = player.DirectionTo(Main.MouseWorld);
                }
                Vel.Normalize();
                Vel *= MageShootSpeed;
                Cast(player, source, vPlayer.FocusPos ?? position, Vel, MageShoot, MageDamage, MageKnockback);
            }
            else
            {
                SwingMelee(player, source, position, velocity, type, damage, knockback);
            }

            return false;
        }

        public void SwingMelee(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            VerPlayer vPlayer = player.GetModPlayer<VerPlayer>();
            if (ComboSteps >= 1 && vPlayer.ComboState == 0)
            {
                Slash(vPlayer, source, position, velocity, type, damage, knockback);
            }
            else if (ComboSteps >= 2 && vPlayer.ComboState == 1)
            {
                Jab(vPlayer, source, position, velocity, type, damage, knockback);
            }
            else if (ComboSteps >= 3 && vPlayer.ComboState == 2)
            {
                Cut(vPlayer, source, position, velocity, type, damage, knockback);
            }
            else if (ComboSteps >= 4 && vPlayer.ComboState == 3)
            {
                Stab(vPlayer, source, position, velocity, type, damage, knockback);
            }
        }

        #region Virtual Methods
        public virtual void Slash(VerPlayer player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, player.Player.Center, velocity, SlashProjectile, damage, knockback, player.Player.whoAmI, GetUseFrames(player.Player));
            player.ComboState++;
            player.AttackFramesLeft = GetUseFrames(player.Player);
        }
        public virtual void Jab(VerPlayer player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, player.Player.Center, velocity, JabProjectile, damage, knockback, player.Player.whoAmI, GetUseFrames(player.Player), 0f);
            player.ComboState++;
            player.AttackFramesLeft = GetUseFrames(player.Player);
        }
        public virtual void Cut(VerPlayer player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, player.Player.Center, velocity, CutProjectile, damage, knockback, player.Player.whoAmI, GetUseFrames(player.Player));
            player.ComboState++;
            player.AttackFramesLeft = GetUseFrames(player.Player);
        }
        public virtual void Stab(VerPlayer player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, player.Player.Center, velocity, StabProjectile, damage, knockback, player.Player.whoAmI, GetUseFrames(player.Player));
            player.ComboState++;
            player.AttackFramesLeft = GetUseFrames(player.Player);
        }
        public virtual void Cast(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
        }
        #endregion
    }
}
