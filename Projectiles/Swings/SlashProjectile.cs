using Terraria;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using vermage.Systems;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Diagnostics;
using vermage.Systems.Handlers;

namespace vermage.Projectiles.Swings
{
    public abstract class SlashProjectile : ModProjectile
    {
        private Player Player;
        private Vector2 StartPos;
        public override void SetDefaults()
        {
            base.SetDefaults();
            AIType = -1;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<VermilionDamageClass>();
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1.2f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            Projectile.timeLeft = (int)Projectile.ai[0];
            Player = Main.player[Projectile.owner];
            Projectile.spriteDirection = Player.direction;
            Projectile.direction = Projectile.spriteDirection;
            StartPos = Player.RotatedRelativePoint(Player.MountedCenter).DirectionTo(Main.MouseWorld);
        }
        public override bool PreAI()
        {
            return true;
        }
        public override bool ShouldUpdatePosition() => false;

        public override void AI()
        {
            if(Projectile.timeLeft <= 0)
            {
                Projectile.Kill();
                return;
            }

            Player.heldProj = Projectile.whoAmI;

            Vector2 PlayerCenter = Player.RotatedRelativePoint(Player.MountedCenter);
            Vector2 Direction = PlayerCenter.DirectionTo(StartPos);
            Direction.Normalize();

            Vector2 NormVel = Projectile.velocity.SafeNormalize(new Vector2(0));
            Vector2 PivotCenter = PlayerCenter + (NormVel * Projectile.Size.X * 0.89f) + Direction;
            

            float rawProg = ProgressPercent();

            if (rawProg >= 0.65f)
            {
                Projectile.alpha += 75;
            }

            float progress = VerUtils.Easings.OutExpo(rawProg);

            float degress = MathHelper.ToRadians(180f * progress);

            Vector2 hand = PivotCenter + (Direction * Projectile.Size.X * 0.25f) * Projectile.direction;

            Vector2 rotated  = hand.RotatedBy(MathHelper.ToRadians(-90f * Projectile.direction) + degress * Projectile.direction, PlayerCenter);


            Projectile.Center = rotated;

            DrawOriginOffsetX = 0;
            DrawOriginOffsetY = (int)(-Projectile.Size.Y * 0.5f);
            DrawOffsetX = (int)(-Projectile.Size.X * 0.05f) * Projectile.direction;


            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, (Player.Center -
                new Vector2(Projectile.Center.X, (Projectile.Center.Y + DrawOriginOffsetY) + 20)
                ).ToRotation() + MathHelper.PiOver2);

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(-75f) + MathHelper.ToRadians(160f * progress) * Projectile.spriteDirection + (Projectile.direction == -1 ? MathHelper.ToRadians(-15f) : 0f);
        }

        private float ProgressPercent()
        {
            return Utils.Clamp(1 - (Projectile.timeLeft / Projectile.ai[0]), 0f, 1f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (damageDone > 0)
            {
                Player.GetModPlayer<VerPlayer>().AddMana(ManaColor.Black, 0.05f);

                Player.GetModPlayer<VerPlayer>().FocusOnHitNPC?.Invoke(Projectile, target,hit, damageDone);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(target, info);

            if (info.Damage > 0)
            {
                Player.GetModPlayer<VerPlayer>().AddMana(ManaColor.Black, 0.05f);

                Player.GetModPlayer<VerPlayer>().FocusOnHitPlayer?.Invoke(Projectile, target, info, info.Damage);
            }
        }

    }
}
