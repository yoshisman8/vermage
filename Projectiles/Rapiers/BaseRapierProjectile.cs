using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Enums;
using Terraria;
using Terraria.ModLoader;
using vermage.Systems;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using System.Security.Policy;
using static Terraria.ModLoader.PlayerDrawLayer;
using Steamworks;
using System.Drawing;

namespace vermage.Projectiles.Rapiers
{
    public abstract class BaseRapierProjectile : ModProjectile
    {
        public int IdleTimer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public int RapierID
        {
            get => (int)Projectile.ai[1];
        }
        public Vector2 StancePos;

        int idlePause;
        float idleDegrees = 0f;
        bool floatUpOrDown; //false is Up, true is Down

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(36);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;

            if (ModLoader.HasMod("ThoriumMod"))
            {
                Projectile.DamageType = GetInstance<VermilionDamageClass>();
            }
            else
            {
                Projectile.DamageType = DamageClass.Magic;
            }

            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 360;
            Projectile.hide = true;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            VerPlayer vPlayer = player.GetModPlayer<VerPlayer>();

            if (vPlayer.Rapier.HasValue)
            {
                if (vPlayer.Rapier.Value != RapierID || vPlayer.AttackFramesLeft > 0)
                {
                    Projectile.Kill();
                    return;
                }
            }
            else
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 10;
            player.heldProj = Projectile.whoAmI;


            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15; // Decrease alpha, increasing visibility.
            }

            UpdateIdle();
            UpdateStance(player);
        }
        private void UpdateStance(Player player)
        {
            VerPlayer vPlayer = player.GetModPlayer<VerPlayer>();

            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);

            Vector2 direction = playerCenter.DirectionTo(Main.MouseWorld);
            direction.Normalize();

            Projectile.spriteDirection = player.direction;
            Projectile.direction = Projectile.spriteDirection;

            Vector2 PivotCenter = playerCenter + direction * Projectile.Size.X * 0.12f + direction;

            if (vPlayer.IsMageStance)
            {
                Projectile.Center = PivotCenter + direction * Projectile.Size.X * 0.01f;

                Projectile.rotation = direction.ToRotation() + MathHelper.ToRadians(180f) * Projectile.spriteDirection + (Projectile.direction == -1 ? MathHelper.ToRadians(180f) : 0f);

                DrawOffsetX = (int)(Projectile.Size.X * 0.17f) * Projectile.direction;
                DrawOriginOffsetY = (int)(-Projectile.Size.Y * 0.30f);
                DrawOriginOffsetX = (int)(-Projectile.Size.X * 0.28f) * Projectile.direction;

            }
            else
            {
                DrawOriginOffsetY = (int)(-Projectile.Size.Y * 0.10f);
                DrawOriginOffsetX = 0;
                DrawOffsetX = 0;

                Projectile.Center = PivotCenter + direction * Projectile.Size.X * 0.30f;

                Projectile.rotation = direction.ToRotation() + MathHelper.ToRadians(idleDegrees) + MathHelper.ToRadians(25f) * Projectile.spriteDirection + (Projectile.direction == -1 ? MathHelper.ToRadians(180f) : 0f);
            }

            StancePos = Projectile.Center;

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, (player.Center -
                new Vector2(Projectile.Center.X, Projectile.Center.Y + DrawOriginOffsetY + 20)
                ).ToRotation() + MathHelper.PiOver2);
        }
        private void UpdateIdle()
        {
            if (floatUpOrDown)//Up
            {
                if (IdleTimer > 5)
                {
                    idleDegrees++;
                    IdleTimer = 0;
                }
            }
            else
            {
                if (IdleTimer > 5)
                {
                    idleDegrees--;
                    IdleTimer = 0;
                }
            }
            if (idleDegrees > 5)
            {
                idlePause = 60;
                idleDegrees = 5;
                floatUpOrDown = false;

            }
            if (idleDegrees < -5)
            {
                idlePause = 60;
                idleDegrees = -5;
                floatUpOrDown = true;

            }
            if (idlePause < 0)
            {
                if (idleDegrees < -3 && idleDegrees > 3)
                {
                    IdleTimer += 2;
                }
                else
                {
                    IdleTimer++;
                }
            }

            idlePause--;
        }

        //private void UpdateAttack(Player player)
        //{
        //    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.Center -
        //        new Vector2(Main.MouseWorld.X, (Main.MouseWorld.Y + DrawOriginOffsetY) + 20)
        //        ).ToRotation() + MathHelper.PiOver2);

        //    Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, false, false);
        //    Vector2 direction = playerCenter.DirectionTo(Main.MouseWorld);
        //    direction.Normalize();

        //    ///Projectile.Center = playerCenter + (direction * Projectile.Size.X * 0.5f) + direction * (Timer -1f);
        //    Projectile.Center = StancePos + direction * (Timer -1f);

        //    Projectile.spriteDirection = (Vector2.Dot(direction, Vector2.UnitX) >= 0f).ToDirectionInt();
        //    Projectile.direction = Projectile.spriteDirection;

        //    Projectile.rotation = direction.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;

        //    SetVisualOffsets();
        //}
        //private void SetVisualOffsets()
        //{
        //    // 32 is the sprite size (here both width and height equal)
        //    int HalfSpriteWidth = (int)Projectile.Size.X / 2;
        //    int HalfSpriteHeight = (int)Projectile.Size.Y / 2;

        //    int HalfProjWidth = Projectile.width / 2;
        //    int HalfProjHeight = Projectile.height / 2;

        //    // Vanilla configuration for "hitbox in middle of sprite"
        //    DrawOriginOffsetX = 0;
        //    DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
        //    DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);

        //    // Vanilla configuration for "hitbox towards the end"
        //    //if (Projectile.spriteDirection == 1) {
        //    //	DrawOriginOffsetX = -(HalfProjWidth - HalfSpriteWidth);
        //    //	DrawOffsetX = (int)-DrawOriginOffsetX * 2;
        //    //	DrawOriginOffsetY = 0;
        //    //}
        //    //else {
        //    //	DrawOriginOffsetX = (HalfProjWidth - HalfSpriteWidth);
        //    //	DrawOffsetX = 0;
        //    //	DrawOriginOffsetY = 0;
        //    //}
        //}

        public override bool ShouldUpdatePosition() => false;
        public override bool? CanCutTiles() => false;
        public override bool? CanDamage() => false;

    }
}
