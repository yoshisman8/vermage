using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using vermage.Projectiles.Spells;

namespace vermage.Buffs.Spells
{
    public class Jolt : BaseSpell
    {
        public override void ConfigureSpell()
        {
            CastingTime = 1.25f;
            Color = Systems.ManaColor.Red;
            Damage = new(1.15f, 1f);
            Knockback = new(1f, 1f, 2f);
            ManaCost = 15;
            Velocity = new(7f);
            Tier = 1;
            ProjectileType = ModContent.ProjectileType<JoltProjectile>();
        }
    }
}
