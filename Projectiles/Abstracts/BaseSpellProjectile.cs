using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using vermage.Systems;
using vermage.Systems.Handlers;

namespace vermage.Projectiles.Abstracts
{
    public abstract class BaseSpellProjectile : ModProjectile
    {
        private Player Owner;
        private VerPlayer VerOwner { get { return Owner.GetModPlayer<VerPlayer>(); } }  
        private ManaColor Color { get { return (ManaColor)Projectile.ai[0]; } }

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(24);
            Projectile.DamageType = ModContent.GetInstance<VermilionDamageClass>();
            Projectile.aiStyle = -1;             //The ai style of the projectile, please reference the source code of Terraria
            Projectile.friendly = true;         //Can the projectile deal damage to enemies?
            Projectile.hostile = false;         //Can the projectile deal damage to the player?
            
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage > 0)
            {
                VerOwner.AddMana(Color, 0.05f);
                VerOwner.ProcessOnHitWithSpell(Projectile, target, info, info.Damage);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (damageDone > 0)
            {
                VerOwner.AddMana(Color, 0.05f);
                VerOwner.ProcessOnHitWithSpell(Projectile, target, hit, damageDone);
            }
        }
    }
}
