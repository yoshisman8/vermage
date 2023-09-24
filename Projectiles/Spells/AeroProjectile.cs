using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using vermage.Buffs;
using vermage.Projectiles.Abstracts;
using vermage.Systems.Utilities;

namespace vermage.Projectiles.Spells
{
    internal class AeroProjectile : BaseSpellProjectile
    {
        public override int Homing => 4;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Main.projFrames[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.penetrate = 1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.timeLeft = 600;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.light = 1f;            //How much light emit around the projectile
            Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true;          //Can the projectile collide with tiles?
            Projectile.extraUpdates = 1;            //Set to above 0 if you want the projectile to update multiple time in a frame
        }

        public override void AI()
        {
            base.AI();
            // Loop through the 4 animation frames, spending 5 ticks on each
            // Projectile.frame — index of current frame
            if (++Projectile.frameCounter >= 5 * (1 + Projectile.extraUpdates))
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            foreach (var p in VerUtils.FindAllNearbyPlayers(Owner, 200, false).Where(x => !Owner.InOpposingTeam(x)))
            {
                p.AddBuff(ModContent.BuffType<GaleBuff>(), (int)VerOwner.BuffDuration.ApplyTo(2 * Main.frameRate));
            }

            foreach (var p in VerUtils.FindAllNearbyPlayers(Projectile.position, 200).Where(x => !Owner.InOpposingTeam(x)))
            {
                p.AddBuff(ModContent.BuffType<GaleBuff>(), (int)VerOwner.BuffDuration.ApplyTo(2 * Main.frameRate));
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(target, info);
            foreach (var p in VerUtils.FindAllNearbyPlayers(Owner, 200, false).Where(x => !Owner.InOpposingTeam(x)))
            {
                p.AddBuff(ModContent.BuffType<GaleBuff>(), (int)VerOwner.BuffDuration.ApplyTo(2 * Main.frameRate));
            }

            foreach (var p in VerUtils.FindAllNearbyPlayers(Projectile.position, 200).Where(x => !Owner.InOpposingTeam(x)))
            {
                p.AddBuff(ModContent.BuffType<GaleBuff>(), (int)VerOwner.BuffDuration.ApplyTo(2 * Main.frameRate));
            }
        }
    }
}
