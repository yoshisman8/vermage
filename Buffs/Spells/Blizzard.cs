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
            CastingTime = 1.65f;
            Damage = new(1.85f, 1f);
            ManaCost = 12;
            Knockback = new(0.75f, 1f);
            Velocity = new(10f);
            Color = Systems.ManaColor.Black;
            ProjectileType = ModContent.ProjectileType<BlizzardProjectile>();
        }
    }
}
