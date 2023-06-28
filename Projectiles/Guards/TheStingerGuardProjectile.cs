using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vermage.Projectiles.Guards
{
    public class TheStingerGuardProjectile : BaseGuardProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.Size = new Microsoft.Xna.Framework.Vector2(50);
        }
    }
}
