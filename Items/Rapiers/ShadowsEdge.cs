using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using vermage.Projectiles.Rapiers;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.Enums;
using Microsoft.Xna.Framework.Content;
using System.Security.Policy;
using vermage.Projectiles.Swings;
using vermage.Projectiles.Swings.HellstoneRapier;
using vermage.Projectiles.Spells;
using vermage.Projectiles.Swings.TheStinger;
using vermage.Systems.Utilities;

namespace vermage.Items.Rapiers
{
    public class ShadowsEdge : BaseRapier
    {
        public override void SetDefaults()
        {

            Item.width = 50;
            Item.height = 56;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.Orange;

            Item.autoReuse = true;

            Item.shoot = ProjectileType<ShadowsEdgeProjectile>();

            Item.crit = 7;

            Item.mana = 13;

            base.SetDefaults();
        }

        public override DuelData GetDuelData()
        {
            return new DuelData()
                .SetDamage(24)
                .SetKnockback(4f)
                .SetUseTime(32)
                .SetFirstAttack(ProjectileType<TheStingerJabProjectile>())
                .SetSecondAttack(ProjectileType<TheStingerSlashProjectile>())
                .SetThirdAttack(ProjectileType<TheStingerPierceProjectile>());
        }
        public override SpellData GetSpellData()
        {
            return SpellData.Jolt().SetDamage(1.25f).SetCastTime((int)(60f * 1f)).SetKnockback(3f).SetProjectileSpeed(7f);
        }

    }
}
