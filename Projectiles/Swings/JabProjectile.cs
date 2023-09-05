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
    public abstract class JabProjectile : ModProjectile
    {
        private Vector2 MousePos;
        private Vector2 StartingPos;
        private Player Player;
        private bool IsChild
        {
            get
            {
                return Projectile.ai[1] == 1;
            }
            set
            {
                Projectile.ai[1] = value ? 1 : 0;
            }
        }
        private int ChildSpawned = 0;
        private int ChildCounter = 0;
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = -1;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<VermilionDamageClass>();
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1.3f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            Projectile.timeLeft = (int)(Projectile.ai[0] * 0.5f);
            Projectile.ai[0] *= 0.5f;
            Player = Main.player[Projectile.owner];
            Projectile.spriteDirection = Player.direction;
            Projectile.direction = Projectile.spriteDirection;
            MousePos = Main.MouseWorld;
            if (!IsChild)
            {
                Projectile.alpha = 255;
                Projectile.hide = true;
            }
            else
            {
                StartingPos = Projectile.Center;
            }
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

            if (IsChild)
            {
                float progress = VerUtils.Easings.OutQuint(ProgressPercent());

                Vector2 PlayerCenter = Player.RotatedRelativePoint(Player.MountedCenter);
                Vector2 Direction = PlayerCenter.DirectionTo(MousePos);
                Direction.Normalize();

                Vector2 PivotCenter = PlayerCenter + (Direction * Projectile.Size.X * 0.25f) + Direction;

                Projectile.alpha -= 5;

                Projectile.Center = StartingPos + (Direction * (Projectile.Size.X * (0.55f * progress)));
            }
            else
            {
                Vector2 PlayerCenter = Player.RotatedRelativePoint(Player.MountedCenter);
                Vector2 Direction = PlayerCenter.DirectionTo(MousePos);
                Direction.Normalize();

                Vector2 PivotCenter = PlayerCenter + (Direction * ProjectileSize() * 0.25f) + Direction;
                Vector2 hand = PivotCenter + (Direction * ProjectileSize() * 0.15f);

                Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, (Player.Center -
                new Vector2(hand.X, (hand.Y + DrawOriginOffsetY) + 20)).ToRotation() + MathHelper.PiOver2);

                if (ChildSpawned == 0)
                {
                    Terraria.Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), hand, Projectile.velocity, Type, Projectile.damage, Projectile.knockBack, Player.whoAmI, Projectile.ai[0], 1f);
                    ChildSpawned++;
                }
                else if (ChildCounter >= Math.Floor(Projectile.ai[0] / 4))
                {

                    Vector2 rot = hand.RotatedBy(MathHelper.ToRadians(ChildSpawned % 2 == 0 ? Main.rand.Next(25, 35) : Main.rand.Next(-35, -25)), PlayerCenter);

                    Terraria.Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), rot, Projectile.velocity, Type, (int)Math.Ceiling(Projectile.damage/3f), Projectile.knockBack, Player.whoAmI, Projectile.ai[0], 1f);
                    ChildSpawned++;
                    ChildCounter = 0;
                }
                ChildCounter++;
            }
        }
        private int ProjectileSize()
        {
            return (int)Math.Floor((Projectile.Size.X + Projectile.Size.Y) / 2);
        }
        private float ProgressPercent()
        {
            return Utils.Clamp(1 - (Projectile.timeLeft / Projectile.ai[0]), 0f, 1f);
        }
        public override bool ShouldUpdatePosition() => false;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, damage, knockback, crit);

            if(damage > 0)
            {
                Player.GetModPlayer<VerPlayer>().AddMana(ManaColor.Black, 0.02f);

                Player.GetModPlayer<VerPlayer>().FocusOnHitNPC?.Invoke(Projectile, damage, target);
            }
        }
        public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        {
            base.OnHitPvp(target, damage, crit);

            if (damage > 0)
            {
                Player.GetModPlayer<VerPlayer>().AddMana(ManaColor.Black, 0.02f);

                Player.GetModPlayer<VerPlayer>().FocusOnHitPlayer?.Invoke(Projectile, damage, target);
            }
        }
    }
}
