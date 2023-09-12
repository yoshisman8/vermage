using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using vermage.Items.Abstracts;
using vermage.Systems;

namespace vermage.Buffs.Spells
{
    public abstract class BaseSpell : ModBuff
    {
        public virtual LocalizedText UnlockHint => Language.GetText($"Mods.{Mod.Name}.Buffs.{Name}.UnlockHint");
        public ManaColor Color { get; set; }
        public int Tier { get; set; } = 1;
        public int ProjectileType { get; set; }
        public int ManaCost { get; set; }
        public float CastingTime { get; set; } = 1.25f;
        public Vector2 Velocity { get; set; }
        public StatModifier Damage { get; set; }
        public StatModifier Knockback { get; set; }
        public string SpellSoundEffectPath { get; set; }
        public SpellData SpellData => new SpellData()
        {
            Color = Color,
            Damage = Damage,
            Knockback = Knockback,
            ManaCost = ManaCost,
            ProjectileType = ProjectileType,
            Velocity = Velocity,
            UnlockHint = UnlockHint,
            Tier = Tier,
            Name = DisplayName,
            Tooltip = Description,
            CastingTime = CastingTime,
            IconPath = this.Texture,
            SpellSFXPath = SpellSoundEffectPath
        };

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            
            ConfigureSpell();

            vermage.Spells.Add(FullName, SpellData);
        }
        /// <summary>
        /// Used to actually configure the aspects of the spell.
        /// </summary>
        public abstract void ConfigureSpell();
    }
}
