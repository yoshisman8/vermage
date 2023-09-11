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
using vermage.Systems.Utilities;
using vermage.Items.Abstracts;
using vermage.Projectiles.Guards;
using vermage.Projectiles.Foci;

namespace vermage.Items.Rapiers
{
    public class TheStinger : BaseRapier
    {
        public override void SetDefaults()
        {

            Item.width = 70;
            Item.height = 70;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.Orange;

            Item.crit = 7;
            Item.damage = 24;
            Item.knockBack = 4f;
            Item.useTime = 32;
            Item.useAnimation = 32;

            RapierProjectile = ProjectileType<TheStingerProjectile>();
            GuardProjectile = ProjectileType<TheStingerGuardProjectile>();
            FocusProjectile = ProjectileType<ExampleFocusProjectile>();

            base.SetDefaults();
        }

    }
}
