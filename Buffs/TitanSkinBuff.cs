using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace vermage.Buffs
{
    public class TitanSkinBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.10f;
        }
    }
}
