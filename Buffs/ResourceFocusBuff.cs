using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using vermage.Systems;

namespace vermage.Buffs
{
    public class ResourceFocusBuff : BaseFocusBuff
    {

        public override void Update(Player player, ref int buffIndex)
        {
            base.Update(player, ref buffIndex);

            player.manaCost -= 0.1f;

            player.GetModPlayer<VerPlayer>().CastingTimeMultiplier -= 0.05f;
        }
    }
}
