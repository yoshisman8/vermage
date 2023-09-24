using Terraria.ModLoader;
using vermage.Projectiles.Spells;

namespace vermage.Buffs.Spells
{
    public class Thunder : BaseSpell
    {
        public override void ConfigureSpell()
        {
            CastingTime = 1.25f;
            Color = Systems.ManaColor.Black;
            Damage = new(1.45f, 1f);
            Knockback = new(0.35f, 1f);
            ManaCost = 30;
            Velocity = new(7f);
            Tier = 1;
            ProjectileType = ModContent.ProjectileType<JoltProjectile>();
        }
    }
}
