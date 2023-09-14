using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace vermage.Buffs
{
    public class RedFlames : ModBuff
    {
        private int timer = 0;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            timer++;
            if (timer >= Main.frameRate)
            {
                timer = 0;
                npc.SimpleStrikeNPC(1, 0);
            }
        }
        public override void Update(Player player, ref int buffIndex)
        {
            timer++;
            if (timer >= Main.frameRate)
            {
                timer = 0;
                player.Hurt(PlayerDeathReason.ByOther(Type), 1, 0, knockback: 0f, dodgeable: false, armorPenetration: 1f);
            }
        }
    }
}
