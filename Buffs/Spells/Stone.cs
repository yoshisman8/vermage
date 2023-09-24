using Terraria.ModLoader;
using vermage.Projectiles.Spells;

namespace vermage.Buffs.Spells
{
    public class Stone : BaseSpell
    {
        public override void ConfigureSpell()
        {
            CastingTime = 1.85f;
            Color = Systems.ManaColor.White;
            Damage = new(1.45f, 1f);
            Knockback = new(1.5f, 1f);
            ManaCost = 21;
            Velocity = new(6f);
            Tier = 1;
            ProjectileType = ModContent.ProjectileType<StoneProjectile>();
        }
    }
}
