using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using vermage.Systems;
using Steamworks;
using Mono.Cecil;

namespace vermage.Items.Foci
{

    public abstract class BaseFoci : ModItem
    {
        /// <summary>
        /// How much Red mana is needed to activate this item.
        /// </summary>
        public int ActivationCost = 0;

        /// <summary>
        /// Duration the Foci remains active in frames.
        /// </summary>
        public int ActiveDuration = 0;

        public abstract void OnHitNPC(Projectile projectile, int damage, NPC target);
        public abstract void OnHitPlayer(Projectile projectile, int damage, Player player);
        public abstract void OnCast(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback);

        public override void SetDefaults()
        {
            Item.accessory = false;
            Item.Size = new Vector2(62);
            Item.rare = ItemRarityID.Blue;
            Item.value = 0;
            Item.maxStack = 1;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = false;

            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var keys = VerPlayer.ActionKey.GetAssignedKeys();
            tooltips.Add(new TooltipLine(vermage.Instance, "HealMana", Terraria.Localization.Language.GetTextValue("Mods.vermage.Tooltips.ActionKey", keys.Count > 0 ? keys[0] : "Unassigned")));
            tooltips.Add(new TooltipLine(vermage.Instance, "UseMana", Terraria.Localization.Language.GetTextValue("Mods.vermage.Tooltips.ManaCost", ActivationCost)));
        }
        public virtual void Activate(Player player)
        {
            player.GetModPlayer<VerPlayer>().FociFramesLeft = ActiveDuration;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        
        public void Toggle(Player player)
        {
            if (player.HasBuff(Item.buffType))
            {
                player.GetModPlayer<VerPlayer>().RemoveAllFoci();
            }
            else
            {
                player.GetModPlayer<VerPlayer>().RemoveAllFoci();

                player.AddBuff(Item.buffType, 2);

                if (player.ownedProjectileCounts[Item.shoot] < 1)
                {
                    Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.position, new Vector2(0), Item.shoot, Item.damage, Item.knockBack, player.whoAmI, Item.buffType);
                }
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Toggle(player);
            return false;
        }
    }
}
