using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using vermage.Projectiles.Spells;

namespace vermage.Systems.Utilities
{
    public struct SpellData
    {
        public string Name;
        public string IconPath;
        public int Projectile;
        public float ProjectileSpeed;
        public int CastTime;
        public float DamageMultiplier;
        public float Knockback;

        public SpellData(string name, string iconPath)
        {
            Name = name;
            IconPath = iconPath;
        }

        public SpellData(string _name, string _iconPath, int _projectile, float _projectileSpeed, int _castTime, float _damage, int _knockback)
        {
            Name = _name;
            IconPath = _iconPath;
            Projectile = _projectile;
            ProjectileSpeed = _projectileSpeed;
            CastTime = _castTime;
            DamageMultiplier = _damage;
            Knockback = _knockback;
        }

        public SpellData SetProjectile(int type)
        {
            Projectile = type;
            return this;
        }
        public SpellData SetProjectileSpeed(float speed)
        {
            ProjectileSpeed = speed;
            return this;
        }
        public SpellData SetCastTime(int castTime)
        {
            CastTime = castTime;
            return this;
        }
        public SpellData SetDamage(float damage)
        {
            DamageMultiplier = damage;
            return this;
        }
        public SpellData SetKnockback(float knockback)
        {
            Knockback = knockback;
            return this;
        }
        public static SpellData Jolt() { return new SpellData("Jolt", "vermage/Assets/Spells/Jolt").SetProjectile(ModContent.ProjectileType<JoltProjectile>()); }
    }

    public struct DuelData
    {
        public int FirstProjectile;
        public int SecondProjectile;
        public int ThirdProjectile;
        public int Damage;
        public float Knockback;
        public int UseTime;

        public DuelData(int _damage, float _knockback, int _useTime, int _slash = -1, int _jab = -1, int _cut = -1, int _stab = -1)
        {
            Damage = _damage;
            Knockback = _knockback;
            UseTime = _useTime;
            FirstProjectile = _slash;
            SecondProjectile = _jab;
            ThirdProjectile = _cut;
        }
        public DuelData SetDamage(int damage)
        {
            Damage = damage;
            return this;
        }
        public DuelData SetKnockback(float knockback)
        {
            Knockback = knockback;
            return this;
        }
        public DuelData SetUseTime(int time)
        {
            UseTime = time;
            return this;
        }
        public DuelData SetFirstAttack(int type)
        {
            FirstProjectile = type;
            return this;
        }
        public DuelData SetSecondAttack(int type)
        {
            SecondProjectile = type;
            return this;
        }
        public DuelData SetThirdAttack(int type)
        {
            ThirdProjectile = type;
            return this;
        }
        public int ComboSteps()
        {
            return (FirstProjectile != -1 ? 1 : 0) + (SecondProjectile != -1 ? 1 : 0) + (ThirdProjectile != -1 ? 1 : 0);
        }
    }
}
