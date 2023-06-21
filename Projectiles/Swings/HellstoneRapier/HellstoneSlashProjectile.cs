using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vermage.Projectiles.Swings.HellstoneRapier
{
    public class HellstoneSlashProjectile : SlashProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Size = new Vector2(70);
        }
    }
}
