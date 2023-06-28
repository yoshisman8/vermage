using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using vermage.Systems;

namespace vermage.Projectiles.Guards
{
    public abstract class BaseGuardProjectile : ModProjectile
    {
        private int MasterType
        {
            get { return (int)Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }
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
                Projectile.DamageType = ModContent.GetInstance<VermilionDamageClass>();
            }
            else
            {
                Projectile.DamageType = DamageClass.Magic;
            }

            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 360;
            Projectile.hide = true;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool? CanCutTiles() => false;
        public override bool? CanDamage() => false;

        public override void AI()
        {
            base.AI();

            Player owner = Main.player[Projectile.owner];
            VerPlayer player = owner.GetModPlayer<VerPlayer>();

            if (player.AttackFramesLeft > 0)
            {
                Projectile.Kill();
                return;
            }

            if (!player.Rapier.HasValue)
            {
                Projectile.Kill();
                return;
            }

            if (!player.RapierPos.HasValue)
            {
                Projectile.Kill();
                return;
            }

            var data = player.RapierPos.Value;

            if (MasterType != data.Type)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 10;

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15; // Decrease alpha, increasing visibility.
            }

            Projectile.direction = owner.direction;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.gfxOffY = owner.gfxOffY;
            Projectile.Center = data.Center;
            Projectile.rotation = data.Rotation;
            DrawOffsetX = data.DrawOffsetX;
            DrawOriginOffsetX = data.DrawOriginOffsetX;
            DrawOriginOffsetY = data.DrawOriginOffsetY;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}
