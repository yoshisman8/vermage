using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vermage.Projectiles.Swings.TheStinger
{
    public class TheStingerPierceProjectile : PierceProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Size = new Vector2(50);
        }
    }
}
