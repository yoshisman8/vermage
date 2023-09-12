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
using vermage.Projectiles.Abstracts;

namespace vermage.Projectiles.Rapiers
{
    public class ShadowsEdgeProjectile : BaseRapierProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Size = new Vector2(50,56);
        }

        public override void IdleStanceAI()
        {
            base.IdleStanceAI();
            if (VerOwner.CastingSpell.HasValue || VerOwner.WasCasting)
            {
                DrawOffsetX = (int)(Projectile.Size.X * 0.15f) * Projectile.direction;
                DrawOriginOffsetY = (int)(-Projectile.Size.Y * 0.30f);
                DrawOriginOffsetX = (int)(-Projectile.Size.X * 0.18f) * Projectile.direction;
            }
        }
    }

    
}
