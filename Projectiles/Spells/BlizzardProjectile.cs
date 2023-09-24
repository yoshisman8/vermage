using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using vermage.Buffs;
using vermage.Projectiles.Abstracts;
using vermage.Systems.Utilities;

namespace vermage.Projectiles.Spells
{
    internal class BlizzardProjectile : BaseSpellProjectile
    {
        public override int Homing => 0;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.penetrate = 4;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
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
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void Kill(int timeLeft)
        {

            for (int d = 0; d < 14; d++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, 0, 0f + Main.rand.Next(-3, 3), 0f + Main.rand.Next(-3, 3), 150, default(Color), 1.5f);
            }
            for (int d = 0; d < 16; d++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, DustID.Ice, 0f + Main.rand.Next(-2, 2), 0f + Main.rand.Next(-2, 2), 150, default(Color), 1.5f);
            }
            for (int d = 0; d < 10; d++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke, 0f + Main.rand.Next(-4, 4), 0f + Main.rand.Next(-4, 4), 150, default(Color), 1.5f);
            }
            for (int d = 0; d < 10; d++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, DustID.SnowflakeIce, 0f + Main.rand.Next(-4, 4), 0f + Main.rand.Next(-4, 4), 150, default(Color), 1.5f);
            }
            for (int d = 0; d < 10; d++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, DustID.Cloud, 0f + Main.rand.Next(-3, 3), 0f + Main.rand.Next(-3, 3), 150, default(Color), 1.5f);
            }
        }
    }
}
