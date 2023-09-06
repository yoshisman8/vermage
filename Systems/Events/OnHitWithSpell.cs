using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace vermage.Systems.Events
{
    public class OnHitWithSpell : IVerEvent
    {
        public string SourceID { get; set; }
        public Action<Projectile, NPC, NPC.HitInfo, int> OnHitNPC { get; set; }
        public Action<Projectile, Player, Player.HurtInfo, int> OnHitPlayer { get; set; }

        public OnHitWithSpell(string sourceID, Action<Projectile, NPC, NPC.HitInfo, int> onHitNPC, Action<Projectile, Player, Player.HurtInfo, int> onHitPlayer)
        {
            SourceID = sourceID;
            OnHitNPC = onHitNPC;
            OnHitPlayer = onHitPlayer;
        }
    }
}
