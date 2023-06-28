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

        public int GuardType;

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

            Projectile.ownerHitCheck = false;
            Projectile.extraUpdates = 1;
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

            Projectile.gfxOffY = player.gfxOffY;

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15; // Decrease alpha, increasing visibility.
            }

            if (player.ownedProjectileCounts[GuardType] < 1)
            {
                Projectile.NewProjectileDirect(player.GetSource_FromThis(), Projectile.Center, new Vector2(0), GuardType, 0, 0, player.whoAmI, Type);
            }

            UpdateIdle();
            UpdateStance(player);

            vPlayer.RapierPos = (Type, Projectile.Center, Projectile.rotation, DrawOffsetX, DrawOriginOffsetX, DrawOriginOffsetY);
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

        public override bool ShouldUpdatePosition() => false;
        public override bool? CanCutTiles() => false;
        public override bool? CanDamage() => false;

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            
        }
    }
}
