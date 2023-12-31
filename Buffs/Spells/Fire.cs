﻿using Terraria.ModLoader;
using vermage.Projectiles.Spells;

namespace vermage.Buffs.Spells
{
    public class Fire : BaseSpell
    {
        public override void ConfigureSpell()
        {
            CastingTime = 1.45f;
            Damage = new(1.65f, 1f);
            ManaCost = 27;
            Knockback = new(0.75f, 1f);
            Velocity = new(8f);
            Color = Systems.ManaColor.Black;
            ProjectileType = ModContent.ProjectileType<FireProjectile>();
        }
    }
}