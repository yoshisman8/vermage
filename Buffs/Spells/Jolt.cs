﻿using Terraria.ModLoader;
using vermage.Projectiles.Spells;

namespace vermage.Buffs.Spells
{
    public class Jolt : BaseSpell
    {
        public override void ConfigureSpell()
        {
            CastingTime = 1.45f;
            Color = Systems.ManaColor.Red;
            Damage = new(1.45f, 1f);
            Knockback = new(1f, 1f, 2f);
            ManaCost = 21;
            Velocity = new(7f);
            Tier = 1;
            ProjectileType = ModContent.ProjectileType<JoltProjectile>();
        }
    }
}
