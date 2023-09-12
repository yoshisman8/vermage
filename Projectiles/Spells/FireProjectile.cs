using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using vermage.Projectiles.Abstracts;
using vermage.Systems.Utilities;
using Terraria.ModLoader;
using vermage.Buffs;

namespace vermage.Projectiles.Spells
{
    public class FireProjectile : BaseSpellProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.penetrate = 1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.timeLeft = 600;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.alpha = 255;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
            Projectile.light = 1f;            //How much light emit around the projectile
            Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true;          //Can the projectile collide with tiles?
            Projectile.extraUpdates = 1;            //Set to above 0 if you want the projectile to update multiple time in a frame
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 70;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;         //The recording mode
        }
        public override bool PreDraw(ref Color lightColor)
        {
            default(RedTrail).Draw(Projectile);

            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            target.AddBuff(ModContent.BuffType<RedFlames>(), 5 * Main.frameRate);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(target, info);
            target.AddBuff(ModContent.BuffType<RedFlames>(), 5 * Main.frameRate);
        }
        public override void Kill(int timeLeft)
        {

            for (int d = 0; d < 14; d++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, 0, 0f + Main.rand.Next(-3, 3), 0f + Main.rand.Next(-3, 3), 150, default(Color), 1.5f);
            }
            for (int d = 0; d < 16; d++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, DustID.Firework_Red, 0f + Main.rand.Next(-2, 2), 0f + Main.rand.Next(-2, 2), 150, default(Color), 1.5f);
            }
            for (int d = 0; d < 10; d++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, DustID.FireflyHit, 0f + Main.rand.Next(-4, 4), 0f + Main.rand.Next(-4, 4), 150, default(Color), 1.5f);
            }
            for (int d = 0; d < 10; d++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, DustID.Firefly, 0f + Main.rand.Next(-4, 4), 0f + Main.rand.Next(-4, 4), 150, default(Color), 1.5f);
            }
            for (int d = 0; d < 10; d++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-3, 3), 0f + Main.rand.Next(-3, 3), 150, default(Color), 1.5f);
            }
        }
    }
}
