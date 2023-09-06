﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using vermage.Items.Rapiers;
using vermage.Systems;

namespace vermage.Items.Abstracts
{
    public struct SpellData
    {
        public LocalizedText Name { get; set; }
        public LocalizedText Tooltip { get; set; }
        public LocalizedText UnlockHint { get; set; }
        public ManaColor Color { get; set; }
        public int Tier { get; set; }
        public int ProjectileType { get; set; }
        public int ManaCost { get; set; }
        public Vector2 Velocity { get; set; }
        public StatModifier Damage { get; set; }
        public StatModifier Knockback { get; set; }
        public float CastingTime { get; set; }
    }
}