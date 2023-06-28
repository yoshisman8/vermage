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

namespace vermage.Items.Franes
{

    public abstract class BaseFrame : ModItem
    {
        public abstract void OnHitNPC(Projectile projectile, int damage, NPC target);
        public abstract void OnHitPlayer(Projectile projectile, int damage, Player player);
        public abstract void OnCast(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback); 

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.Size = new Vector2(62);
            Item.rare = ItemRarityID.Blue;
            Item.value = 0;
            Item.maxStack = 1;

            Item.useStyle = ItemUseStyleID.None;
            Item.autoReuse = false;

            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.ModItem is BaseFrame)
            {
                if (incomingItem.ModItem is BaseFrame)
                {
                    return false;
                }
                return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
            }
            else
            {
                return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
            }
        }
        
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            VerPlayer vPlayer = player.GetModPlayer<VerPlayer>();

            vPlayer.FrameOnCast = OnCast;
            vPlayer.FrameOnHitNPC = OnHitNPC;
            vPlayer.FrameOnHitPlayer = OnHitPlayer;
            vPlayer.Frame = Type;

            if (vPlayer.Focus.HasValue && vPlayer.FocusPos.HasValue)
            {
                if (player.ownedProjectileCounts[Item.shoot] < 1)
                {
                    Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), vPlayer.FocusPos.Value, new Vector2(0), Item.shoot, Item.damage, Item.knockBack, player.whoAmI, Item.buffType);
                }
            }
        }
    }
}
