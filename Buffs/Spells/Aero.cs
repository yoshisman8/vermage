using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using vermage.Projectiles.Spells;

namespace vermage.Buffs.Spells
{
    public class Aero : BaseSpell
    {
        public override void ConfigureSpell()
        {
            CastingTime = 1.30f;
            Damage = new(1.05f, 1f);
            ManaCost = 11;
            Knockback = new(0.8f, 1f);
            Velocity = new(10f);
            Color = Systems.ManaColor.White;
            ProjectileType = ModContent.ProjectileType<FireProjectile>();
        }
    }
}
