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
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using vermage.Systems.Handlers;
using Terraria.DataStructures;
using System.Drawing;
using vermage.Items.Abstracts;
using vermage.Systems.Utilities;

namespace vermage.Projectiles.Rapiers
{
    public abstract class BaseRapierProjectile : ModProjectile
    {
        public Behavior Behavior
        {
            get => (Behavior)Projectile.ai[0];
            set => Projectile.ai[0] = (int)value;
        }
        public int TotalTime
        {
            get => (int)(Projectile.ai[1] * (1 + Projectile.extraUpdates));
            set => Projectile.ai[1] = value;
        }
        public int Timer
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        public Player Owner;
        public VerPlayer VerOwner { get { return Owner.GetModPlayer<VerPlayer>(); } }
        public Vector2 StartPosition;
        public Vector2 MousePosition; 


        int idlePause;
        float idleDegrees = 0f;
        bool floatUpOrDown; //false is Up, true is Down

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(36);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = GetInstance<VermilionDamageClass>();

            Projectile.ownerHitCheck = false;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 10;
            Projectile.hide = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Owner = Main.player[Projectile.owner];
            Projectile.spriteDirection = Owner.direction;
            Projectile.direction = Projectile.spriteDirection;
            switch (Behavior)
            {
                case Behavior.Swing:
                    Projectile.timeLeft = TotalTime;
                    StartPosition = Projectile.Center;
                    Projectile.scale = 1.2f;
                    break;

                case Behavior.Thrust:
                    Projectile.timeLeft = TotalTime;
                    StartPosition = Projectile.Center;
                    MousePosition = Main.MouseWorld;
                    break;

                default:
                    Projectile.alpha = 255;
                    break;
            }
        }


        public override void AI()
        {
            if (!VerOwner.RapierData.HasValue)
            {
                Projectile.Kill();
                return;
            }
            if (VerOwner.RapierData.Value.RapierProjectile != Type)
            {
                Projectile.Kill();
                return;
            }
            
            if (Behavior == Behavior.Idle && (VerOwner.RapierBehavior == Behavior.Swing || VerOwner.RapierBehavior == Behavior.Thrust))
            {
                Projectile.Kill();
                return;
            }

            Owner.heldProj = Projectile.whoAmI;

            Projectile.gfxOffY = Owner.gfxOffY;

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15; // Decrease alpha, increasing visibility.
            }
            
            switch (Behavior)
            {
                case Behavior.Idle:
                    UpdateIdle();
                    IdleStanceAI();
                    break;
                case Behavior.Swing:
                    SwingAI();
                    Timer++;
                    break;
                case Behavior.Thrust:
                    ThrustAI();
                    Timer++;
                    break;
                default:
                    UpdateIdle();
                    IdleStanceAI();
                    break;
            }
        }
        #region IdleAI
        public virtual void IdleStanceAI()
        {
            Projectile.timeLeft = 10;
            Vector2 playerCenter = Owner.RotatedRelativePoint(Owner.MountedCenter);

            Vector2 direction = playerCenter.DirectionTo(Main.MouseWorld);
            direction.Normalize();

            Projectile.spriteDirection = Owner.direction;
            Projectile.direction = Projectile.spriteDirection;

            Vector2 PivotCenter = playerCenter + direction * Projectile.Size.X * 0.12f + direction;

            if (VerOwner.CastingSpell.HasValue || VerOwner.WasCasting)
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

            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, (Owner.Center -
                new Vector2(Projectile.Center.X, Projectile.Center.Y + DrawOriginOffsetY + 20)
                ).ToRotation() + MathHelper.PiOver2);

            Owner.direction = Math.Sign(Main.MouseWorld.X - Owner.Center.X);
            
            if (VerOwner.RapierData.Value.GuardProjectile != -1)
            {
                if (Owner.ownedProjectileCounts[VerOwner.RapierData.Value.GuardProjectile] < 1 && VerOwner.RapierData.Value.GuardProjectile != -1)
                {
                    Projectile.NewProjectileDirect(Owner.GetSource_FromThis(), Projectile.Center, new Vector2(0), VerOwner.RapierData.Value.GuardProjectile, 0, 0, Owner.whoAmI, Projectile.whoAmI);
                }
            }
        }
        public virtual void UpdateIdle()
        {
            if (floatUpOrDown)//Up
            {
                if (Timer > 5)
                {
                    idleDegrees++;
                    Timer = 0;
                }
            }
            else
            {
                if (Timer > 5)
                {
                    idleDegrees--;
                    Timer = 0;
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
                    Timer += 2;
                }
                else
                {
                    Timer++;
                }
            }

            idlePause--;
        }
        #endregion

        #region SwingAI
        public virtual void SwingAI()
        {
            if (Timer > TotalTime)
            {
                Projectile.Kill();
                return;
            }

            Vector2 PlayerCenter = Owner.RotatedRelativePoint(Owner.MountedCenter);
            Vector2 Direction = PlayerCenter.DirectionTo(StartPosition);
            Direction.Normalize();

            Vector2 NormVel = Projectile.velocity.SafeNormalize(new Vector2(0));
            Vector2 PivotCenter = PlayerCenter + (NormVel * Projectile.Size.X * 0.89f) + Direction;


            float rawProg = AnimationPercent();

            float progress = VerUtils.Easings.OutExpo(rawProg);

            float degress = MathHelper.ToRadians(180f * progress);

            Vector2 hand = PivotCenter + (Direction * Projectile.Size.X * 0.25f) * Projectile.direction;

            Vector2 rotated = hand.RotatedBy(MathHelper.ToRadians(-90f * Projectile.direction) + degress * Projectile.direction, PlayerCenter);


            Projectile.Center = rotated;

            DrawOriginOffsetX = 0;
            DrawOriginOffsetY = (int)(-Projectile.Size.Y * 0.5f);
            DrawOffsetX = (int)(-Projectile.Size.X * 0.05f) * Projectile.direction;


            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, (Owner.Center -
                new Vector2(Projectile.Center.X, (Projectile.Center.Y + DrawOriginOffsetY) + 20)
                ).ToRotation() + MathHelper.PiOver2);

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(-75f) + MathHelper.ToRadians(160f * progress) * Projectile.spriteDirection + (Projectile.direction == -1 ? MathHelper.ToRadians(-15f) : 0f);
        }
        #endregion

        #region ThrustAI
        public virtual void ThrustAI()
        {
            if (Timer > TotalTime)
            {
                Projectile.Kill();
                return;
            }

            Vector2 PlayerCenter = Owner.RotatedRelativePoint(Owner.MountedCenter);
            Vector2 Direction = StartPosition.DirectionTo(MousePosition);
            Direction.Normalize();

            Vector2 PivotCenter = PlayerCenter + (Direction * ThrusProjectileSize() * 0.25f) + Direction;
            Vector2 hand = PivotCenter + (Direction * ThrusProjectileSize() * 0.15f);

            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, (Owner.Center -
            new Vector2(hand.X, (hand.Y + DrawOriginOffsetY) + 20)).ToRotation() + MathHelper.PiOver2);

            Projectile.scale = GetThrusScale(AnimationPercent());
            Projectile.Size = Projectile.Size + new Vector2(Projectile.scale);

            DrawOriginOffsetY = (int)-((1f - Projectile.scale) * ThrusProjectileSize() * 0.13f);

            float progress = VerUtils.Easings.InQuint(AnimationPercent() + 0.2f);
            Projectile.Center = StartPosition + (Direction * (Projectile.Size.X * (1f * progress)));
        }
        private int ThrusProjectileSize()
        {
            return (int)Math.Floor((Projectile.Size.X + Projectile.Size.Y) / 2);
        }
        private int GetThrusAlpha(float progress)
        {
            return Utils.Clamp((int)(progress * 2 * 255), 0, 255);
        }
        private float GetThrusScale(float progress)
        {
            return Utils.Clamp(VerUtils.Easings.OutCirc(progress) * 2f, 1f, 2f);
        }
        #endregion

        private float AnimationPercent()
        {
            return Utils.Clamp(Timer/TotalTime, 0f, 1f);
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool? CanCutTiles() => Behavior != Behavior.Idle;
        public override bool? CanDamage() => Behavior != Behavior.Idle;

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage > 0) 
                VerOwner.ProcessOnHitWithRapier(Projectile, target, info, info.Damage);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (damageDone > 0)
                VerOwner.ProcessOnHitWithRapier(Projectile, target, hit, damageDone);
        }
    }
    public enum Behavior
    {
        Idle,
        Casting,
        Swing,
        Thrust,
        EnchantedSwing,
        EnchantedThrust,
        Dash
    }
}
