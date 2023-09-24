using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using vermage.Buffs;
using vermage.Projectiles.Abstracts;
using vermage.Systems.Utilities;

namespace vermage.Projectiles.Spells
{
    public class StoneProjectile : BaseSpellProjectile
    {
        public override int Homing => 2;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.penetrate = 1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.timeLeft = 600;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.light = 0.5f;            //How much light emit around the projectile
            Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true;          //Can the projectile collide with tiles?
            Projectile.extraUpdates = 1;            //Set to above 0 if you want the projectile to update multiple time in a frame
        }
        public override void AI()
        {
            base.AI();
            Projectile.rotation += 0.4f * (float)Projectile.direction;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            foreach (var p in VerUtils.FindAllNearbyPlayers(Owner, 200, false).Where(x => !Owner.InOpposingTeam(x)))
            {
                p.AddBuff(ModContent.BuffType<TitanSkinBuff>(), (int)VerOwner.BuffDuration.ApplyTo(2 * Main.frameRate));
            }

            foreach (var p in VerUtils.FindAllNearbyPlayers(Projectile.position, 200).Where(x => !Owner.InOpposingTeam(x)))
            {
                p.AddBuff(ModContent.BuffType<TitanSkinBuff>(), (int)VerOwner.BuffDuration.ApplyTo(2 * Main.frameRate));
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(target, info);
            foreach (var p in VerUtils.FindAllNearbyPlayers(Owner, 200, false).Where(x => !Owner.InOpposingTeam(x)))
            {
                p.AddBuff(ModContent.BuffType<TitanSkinBuff>(), (int)VerOwner.BuffDuration.ApplyTo(2 * Main.frameRate));
            }

            foreach (var p in VerUtils.FindAllNearbyPlayers(Projectile.position, 200).Where(x => !Owner.InOpposingTeam(x)))
            {
                p.AddBuff(ModContent.BuffType<TitanSkinBuff>(), (int)VerOwner.BuffDuration.ApplyTo(2 * Main.frameRate));
            }
        }
    }
}
