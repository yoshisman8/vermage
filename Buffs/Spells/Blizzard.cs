using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using vermage.Projectiles.Spells;

namespace vermage.Buffs.Spells
{
    public class Blizzard : BaseSpell
    {
        public override void ConfigureSpell()
        {
            CastingTime = 1.30f;
            Damage = new(2f, 1f);
            ManaCost = 20;
            Knockback = new(1f, 1f);
            Velocity = new(12f);
            Color = Systems.ManaColor.Black;
            ProjectileType = ModContent.ProjectileType<FireProjectile>();
        }
    }
}
