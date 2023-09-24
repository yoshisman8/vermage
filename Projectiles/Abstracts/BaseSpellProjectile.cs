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
using vermage.Systems.Utilities;

namespace vermage.Projectiles.Abstracts
{
    public abstract class BaseSpellProjectile : ModProjectile
    {
        internal Player Owner { get { return Main.player[Projectile.owner]; } }
        internal VerPlayer VerOwner { get { return Owner?.GetModPlayer<VerPlayer>(); } }  
        private ManaColor Color { get { return (ManaColor)Projectile.ai[0]; } }
        internal float Timer
        {
            get
            {
                return Projectile.ai[1];
            }
            set
            {
                Projectile.ai[1] = value;
            }
        }
        public virtual int Homing { get; } = -1;

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(24);
            Projectile.DamageType = ModContent.GetInstance<VermilionDamageClass>();
            Projectile.aiStyle = -1;             //The ai style of the projectile, please reference the source code of Terraria
            Projectile.friendly = true;         //Can the projectile deal damage to enemies?
            Projectile.hostile = false;         //Can the projectile deal damage to the player?
            
        }

        public override void AI()
        {
            base.AI();
            if (Homing > 0)
            {
                Timer++;
                if (Timer >= 6 * Homing)
                {
                    float maxDetectRadius = 400f; // The maximum radius at which a projectile can detect a target
                    float projSpeed = 7f; // The speed at which the projectile moves towards the target

                    // Trying to find NPC closest to the projectile
                    NPC closestNPC = VerUtils.FindClosestNPC(Projectile.Center, maxDetectRadius);
                    if (closestNPC == null)
                    {
                        base.AI();
                        return;
                    }

                    // If found, change the velocity of the projectile and turn it in the direction of the target
                    // Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
                    Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    Timer = 0;
                }
            }
            
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage > 0)
            {
                VerOwner.AddMana(Color, 0.10f);
                VerOwner.ProcessOnHitWithSpell(Projectile, target, info, info.Damage);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (damageDone > 0)
            {
                VerOwner.AddMana(Color, 0.10f);
                VerOwner.ProcessOnHitWithSpell(Projectile, target, hit, damageDone);
            }
        }
    }
}
