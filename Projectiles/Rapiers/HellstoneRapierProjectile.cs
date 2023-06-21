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

namespace vermage.Projectiles.Rapiers
{
    public class HellstoneRapierProjectile : BaseRapierProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Size = new Vector2(70);
        }

        public override void AI()
        {
            base.AI();

            if (Main.rand.NextBool(5)) // only spawn 20% of the time
            {
                int choice = Main.rand.Next(3); // choose a random number: 0, 1, or 2
                if (choice == 0) // use that number to select dustID: 15, 57, or 58
                {
                    choice = 7;
                }
                else if (choice == 1)
                {
                    choice = 36;
                }
                else
                {
                    choice = 127;
                }
                // Spawn the dust
                Dust.NewDust(Projectile.position + new Vector2(DrawOriginOffsetX, DrawOriginOffsetY), Projectile.width, Projectile.height, choice, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, default, 0.7f);
            }
        }
    }
}
