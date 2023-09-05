using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using vermage.Systems;

namespace vermage.Projectiles.Swings
{
    public abstract class PierceProjectile : ModProjectile
    {
        private Vector2 MousePos;
        private Vector2 StartingPos;
        private Player Player;

        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = -1;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<VermilionDamageClass>();
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            //Projectile.scale = 2.5f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            Projectile.timeLeft = (int)Projectile.ai[0];
            Player = Main.player[Projectile.owner];
            Projectile.spriteDirection = Player.direction;
            Projectile.direction = Projectile.spriteDirection;
            MousePos = Main.MouseWorld;

            StartingPos = Projectile.Center;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + (MathHelper.ToRadians(45f) * Projectile.spriteDirection) + (Projectile.direction == -1 ? MathHelper.ToRadians(180f) : 0f);
            
            return base.PreDraw(ref lightColor);
        }
        public override void AI()
        {
            if (Projectile.timeLeft <= 0)
            {
                Projectile.Kill();
                return;
            }


            Vector2 PlayerCenter = Player.RotatedRelativePoint(Player.MountedCenter);
            Vector2 Direction = StartingPos.DirectionTo(MousePos);
            Direction.Normalize();

            Vector2 PivotCenter = PlayerCenter + (Direction * ProjectileSize() * 0.25f) + Direction;
            Vector2 hand = PivotCenter + (Direction * ProjectileSize() * 0.15f);

            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, (Player.Center -
            new Vector2(hand.X, (hand.Y + DrawOriginOffsetY) + 20)).ToRotation() + MathHelper.PiOver2);

            Projectile.scale = GetScale(ProgressPercent());
            Projectile.Size = Projectile.Size + new Vector2(Projectile.scale);

            DrawOriginOffsetY = (int)-((1f - Projectile.scale) * ProjectileSize() * 0.13f);

            float progress = VerUtils.Easings.InQuint(ProgressPercent() + 0.2f);
            Projectile.Center = StartingPos + (Direction * (Projectile.Size.X * (1f * progress)));


        }
        private int ProjectileSize()
        {
            return (int)Math.Floor((Projectile.Size.X + Projectile.Size.Y) / 2);
        }
        private float ProgressPercent()
        {
            return Utils.Clamp(1 - (Projectile.timeLeft / Projectile.ai[0]), 0f, 1f);
        }
        private int GetAlpha(float progress)
        {
            return Utils.Clamp((int)(progress * 2 * 255), 0, 255);
        }
        private float GetScale(float progress)
        {
            return Utils.Clamp(VerUtils.Easings.OutCirc(progress) * 2f, 1f, 2f);

        }
        public override bool ShouldUpdatePosition() => false;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, damage, knockback, crit);

            if(damage > 0)
            {
                Player.GetModPlayer<VerPlayer>().AddMana(ManaColor.Black, 0.5f);

                Player.GetModPlayer<VerPlayer>().FocusOnHitNPC?.Invoke(Projectile, damage, target);
            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        {
            base.OnHitPvp(target, damage, crit);

            if (damage > 0)
            {
                Player.GetModPlayer<VerPlayer>().AddMana(ManaColor.Black, 0.5f);

                Player.GetModPlayer<VerPlayer>().FocusOnHitPlayer?.Invoke(Projectile, damage, target);
            }
        }
    }
}
