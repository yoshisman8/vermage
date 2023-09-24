using Terraria.ModLoader;
using vermage.Projectiles.Spells;

namespace vermage.Buffs.Spells
{
    public class Water : BaseSpell
    {
        public override void ConfigureSpell()
        {
            CastingTime = 1.45f;
            Color = Systems.ManaColor.White;
            Damage = new(1f, 1f);
            Knockback = new(0.35f, 1f);
            ManaCost = 27;
            Velocity = new(8f);
            Tier = 1;
            ProjectileType = ModContent.ProjectileType<WaterProjectile>();
        }
    }
}
