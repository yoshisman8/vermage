using Microsoft.Xna.Framework;
using rail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using vermage.Systems;

namespace vermage.Projectiles.Abstracts
{
    public abstract class BaseFocusProjectile : ModProjectile
    {
        int idlePause;
        bool floatUpOrDown; //false is Up, true is Down
        int floatTimer
        {
            get { return (int)Projectile.ai[1]; }
            set { Projectile.ai[1] = value; }
        }
        private Player Owner { get { return Main.player[Projectile.owner]; } }
        private VerPlayer VerOwner { get { return Owner.GetModPlayer<VerPlayer>(); } }
        public override void SetDefaults()
        {

            AIType = 0;
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.minion = false;
            Projectile.minionSlots = 0f;
            Projectile.timeLeft = 240;
            Projectile.penetrate = 999;
            Projectile.hide = false;
            Projectile.alpha = 255;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
        }

        public override void AI()
        {
            Projectile.scale = 0.7f;

            if (!VerOwner.RapierData.HasValue)
            {
                Projectile.Kill();
                return;
            }

            if (VerOwner.RapierData.Value.FocusProjectile != Type)
            {
                Projectile.Kill();
                return;
            }

            if (Owner.dead && !Owner.active)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 10;
            Projectile.alpha -= 10;

            if (VerOwner.CastingSpell.HasValue)
            {
                UpdateAttack();
            }
            else
            {
                UpdateHeld();
            }
            VerOwner.FocusPosition = Projectile.Center;
        }
        private void UpdateHeld()
        {
            Owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, (Owner.Center -
                new Vector2(Projectile.Center.X, Projectile.Center.Y + DrawOriginOffsetY + 20)
                ).ToRotation() + MathHelper.PiOver2);

            Vector2 ownerMountedCenter = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            Projectile.direction = Owner.direction;

            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation = Owner.velocity.X * 0.05f;

            //Projectile.position.X = player.Center.X;
            if (Projectile.spriteDirection == 1)
            {
                Projectile.position.X = Owner.Center.X;

            }
            else
            {
                Projectile.position.X = Owner.Center.X - 62;

            }
            Projectile.position.Y = ownerMountedCenter.Y - Projectile.height / 2 - 12;

            //This is 0 unless a auto attack has been initated, in which it'll tick up.
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            if (Projectile.alpha > 255)
            {
                Projectile.alpha = 255;
            }

            UpdateMovement();
        }
        private void UpdateMovement()
        {
            if (floatUpOrDown)//Up
            {
                if (floatTimer > 7)
                {
                    DrawOriginOffsetY++;
                    floatTimer = 0;
                }
            }
            else
            {
                if (floatTimer > 7)
                {
                    DrawOriginOffsetY--;
                    floatTimer = 0;
                }
            }
            if (DrawOriginOffsetY > -10)
            {
                idlePause = 10;
                DrawOriginOffsetY = -10;
                floatUpOrDown = false;

            }
            if (DrawOriginOffsetY < -20)
            {
                idlePause = 10;
                DrawOriginOffsetY = -20;
                floatUpOrDown = true;

            }
            if (idlePause < 0)
            {
                if (DrawOriginOffsetY < -12 && DrawOriginOffsetY > -18)
                {
                    floatTimer += 2;
                }
                else
                {
                    floatTimer++;
                }
            }

            idlePause--;

        }
        private void UpdateAttack()
        {
            Vector2 playerCenter = Owner.RotatedRelativePoint(Owner.MountedCenter);

            Vector2 direction = playerCenter.DirectionTo(Main.MouseWorld);
            direction.Normalize();

            Projectile.spriteDirection = Owner.direction;
            Projectile.direction = Projectile.spriteDirection;

            DrawOriginOffsetY = 0;
            DrawOriginOffsetX = 0;
            DrawOffsetX = 0;

            Vector2 PivotCenter = playerCenter + direction * Projectile.Size.X * 0.40f + direction;

            Projectile.Center = (PivotCenter + direction * Projectile.Size.X * 0.20f).RotatedBy(MathHelper.ToRadians(-35f * Projectile.direction), playerCenter);

            Projectile.rotation = direction.ToRotation() + MathHelper.ToRadians(45f) * Projectile.spriteDirection + (Projectile.direction == -1 ? MathHelper.ToRadians(180f) : 0);
        }

    }
}
