using Terraria.ModLoader;
using vermage.Projectiles.Spells;

namespace vermage.Buffs.Spells
{
    public class Fire : BaseSpell
    {
        public override void ConfigureSpell()
        {
            CastingTime = 1.5f;
            Damage = new(150f, 1f);
            ManaCost = 18;
            Knockback = new(1f, 1f);
            Velocity = new(7f);
            Color = Systems.ManaColor.Black;
            ProjectileType = ModContent.ProjectileType<FireProjectile>();
        }
    }
}
