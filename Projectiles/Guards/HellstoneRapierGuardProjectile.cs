using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vermage.Projectiles.Abstracts;

namespace vermage.Projectiles.Guards
{
    public class HellstoneRapierGuardProjectile : BaseGuardProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Size = new Microsoft.Xna.Framework.Vector2(70);
        }
    }
}
