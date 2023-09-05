using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Enums;
using Terraria;
using Terraria.ModLoader;
using vermage.Systems;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using System.Security.Policy;
using static Terraria.ModLoader.PlayerDrawLayer;
using Steamworks;
using System.Drawing;
using vermage.Projectiles.Guards;

namespace vermage.Projectiles.Rapiers
{
    public class FleshPiercer2Projectile : BaseRapierProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Size = new Vector2(50,58);
        }

    }

    
}
