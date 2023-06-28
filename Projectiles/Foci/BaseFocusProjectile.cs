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

namespace vermage.Projectiles.Foci
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
        int BuffId
        {
            get { return (int)Projectile.ai[0]; }
        }
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

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            return true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            VerPlayer vPlayer = player.GetModPlayer<VerPlayer>();
            Projectile.scale = 0.7f;
            if (!player.HasBuff(BuffId))
            {
                Projectile.Kill();
            }
            if (player.dead && !player.active)
            {
                Projectile.Kill();
            }
            Projectile.timeLeft = 10;
            Projectile.alpha -= 10;

            if (vPlayer.IsMageStance && vPlayer.IsCasting)
            {
                UpdateAttack(player);
            }
            else
            {
                UpdateHeld(player);
            }
            vPlayer.FocusPos = new Vector2(Projectile.Center.X, Projectile.Center.Y + DrawOriginOffsetY);
        }
        private void UpdateHeld(Player player)
        {
            VerPlayer verPlayer = player.GetModPlayer<VerPlayer>();

            player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, (player.Center -
                new Vector2(Projectile.Center.X, Projectile.Center.Y + DrawOriginOffsetY + 20)
                ).ToRotation() + MathHelper.PiOver2);

            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.direction = player.direction;

            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation = player.velocity.X * 0.05f;

            //Projectile.position.X = player.Center.X;
            if (Projectile.spriteDirection == 1)
            {
                Projectile.position.X = player.Center.X;

            }
            else
            {
                Projectile.position.X = player.Center.X - 62;

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
        private void UpdateAttack(Player player)
        {
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);

            Vector2 direction = playerCenter.DirectionTo(Main.MouseWorld);
            direction.Normalize();

            Projectile.spriteDirection = player.direction;
            Projectile.direction = Projectile.spriteDirection;

            DrawOriginOffsetY = 0;
            DrawOriginOffsetX = 0;
            DrawOffsetX = 0;

            Vector2 PivotCenter = playerCenter + direction * Projectile.Size.X * 0.40f + direction;

            Projectile.Center = (PivotCenter + direction * Projectile.Size.X * 0.20f).RotatedBy(MathHelper.ToRadians(-35f * Projectile.direction), playerCenter);

            Projectile.rotation = direction.ToRotation() + MathHelper.ToRadians(45f) * Projectile.spriteDirection + (Projectile.direction == -1? MathHelper.ToRadians(180f) : 0);
        }
        public override void Kill(int timeLeft)
        {

        }

    }
}
