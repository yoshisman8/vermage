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
using vermage.Systems.Utilities;
using vermage.Items.Abstracts;
using vermage.Projectiles.Foci;
using vermage.Projectiles.Guards;

namespace vermage.Items.Rapiers
{
    public class DarkSaber : BaseRapier
    {
        public override void SetDefaults()
        {

            Item.width = 50;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.Orange;

            Item.crit = 7;
            Item.damage = 24;
            Item.knockBack = 4f;
            Item.useTime = 64;
            Item.useAnimation = 64;

            RapierProjectile = ProjectileType<DarkSaberProjectile>();
            FocusProjectile = ProjectileType<ExampleFocusProjectile>();

            base.SetDefaults();
        }


    }
}
