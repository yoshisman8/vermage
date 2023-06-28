using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using vermage.Items.Franes;
using vermage.Systems;

namespace vermage.Projectiles.Frames
{
    public abstract class BaseFrameProjectile : ModProjectile
    {
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
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            VerPlayer verOwner = owner.GetModPlayer<VerPlayer>();

            if (!verOwner.Focus.HasValue || !verOwner.FocusPos.HasValue)
            {
                Projectile.Kill();
                return;
            }

            if (!verOwner.Frame.HasValue)
            {
                Projectile.Kill();
                return;
            }

            BaseFrame frame = ModContent.GetModItem(verOwner.Frame.Value) as BaseFrame;

            if (frame == null)
            {
                Projectile.Kill();
                return;
            }
            if (frame.Item.shoot != Type)
            {
                Projectile.Kill();
                return;
            }
            Projectile.alpha -= 5;
            Projectile.Center = verOwner.FocusPos.Value + new Vector2(0, Projectile.Size.Y*0.08f);
            Projectile.timeLeft = 4;

            if (verOwner.IsCasting)
            {
                UpdateAttack(owner);
            }
            else
            {
                Projectile.direction = owner.direction;

                Projectile.spriteDirection = Projectile.direction;
                Projectile.rotation = owner.velocity.X * 0.05f;
            }
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

            Projectile.rotation = direction.ToRotation() + MathHelper.ToRadians(45f) * Projectile.spriteDirection + (Projectile.direction == -1 ? MathHelper.ToRadians(180f) : 0);
        }
    }
}
