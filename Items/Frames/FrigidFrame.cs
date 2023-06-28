using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using vermage.Items.Franes;
using vermage.Projectiles.Frames;

namespace vermage.Items.Frames
{
    public class FrigidFrame : BaseFrame
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.shoot = ModContent.ProjectileType<FrigidFrameProjectile>();
        }
        public override void OnCast(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
        {
            
        }

        public override void OnHitNPC(Projectile projectile, int damage, NPC target)
        {
            target.AddBuff(BuffID.Frostburn, 300);
        }

        public override void OnHitPlayer(Projectile projectile, int damage, Player player)
        {
            player.AddBuff(BuffID.Frostburn, 300);
        }
    }
}
