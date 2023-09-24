using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using vermage.Projectiles.Abstracts;
using vermage.Systems.Utilities;

namespace vermage.Projectiles.Spells
{
    public class WaterProjectile : BaseSpellProjectile
    {
        public override int Homing => 3;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.penetrate = 1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.timeLeft = 600;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.light = 1f;            //How much light emit around the projectile
            Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            Projectile.tileCollide = false;          //Can the projectile collide with tiles?
            Projectile.extraUpdates = 1;            //Set to above 0 if you want the projectile to update multiple time in a frame

            Projectile.width = 68;
            Projectile.height = 68;

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

            List<int> healed = new List<int>();

            foreach (var p in VerUtils.FindAllNearbyPlayers(Owner, 200, false).Where(x=> !Owner.InOpposingTeam(x)))
            {
                p.Heal(VerOwner.GetHealingPower());
                healed.Add(p.whoAmI);
            }

            foreach (var p in VerUtils.FindAllNearbyPlayers(Projectile.position, 200).Where(x => !Owner.InOpposingTeam(x)))
            {
                if (healed.Contains(p.whoAmI)) continue;
                p.Heal(VerOwner.GetHealingPower());
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(target, info);
            List<int> healed = new List<int>();

            foreach (var p in VerUtils.FindAllNearbyPlayers(Owner, 200, false).Where(x => !Owner.InOpposingTeam(x)))
            {
                p.Heal(VerOwner.GetHealingPower());
                healed.Add(p.whoAmI);
            }

            foreach (var p in VerUtils.FindAllNearbyPlayers(Projectile.position, 200).Where(x => !Owner.InOpposingTeam(x)))
            {
                if (healed.Contains(p.whoAmI)) continue;
                p.Heal(VerOwner.GetHealingPower());
            }
        }
    }
}
