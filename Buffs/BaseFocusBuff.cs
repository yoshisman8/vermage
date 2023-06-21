using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using vermage.Systems;

namespace vermage.Buffs
{
    public abstract class BaseFocusBuff : ModBuff
    {

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            

            try
            {
                if (player.dead && !player.active)
                {
                    player.DelBuff(buffIndex);
                    buffIndex--;
                    return;
                }

                if (vermage.FocusRegistry.Any(x => x.Buff == Type))
                {
                    var reg = vermage.FocusRegistry.First(x => x.Buff == Type);

                    // If the minions exist reset the buff time, otherwise remove the buff from the player
                    if (player.ownedProjectileCounts[reg.Projectile] > 0)
                    {
                        player.buffTime[buffIndex] = 18000;
                        player.GetModPlayer<VerPlayer>().Focus = reg.Item;
                    }
                    else
                    {
                        player.DelBuff(buffIndex);
                        buffIndex--;
                    }
                }
                else
                {
                    player.DelBuff(buffIndex);
                    buffIndex--;
                }
            }

            catch (Exception ex) 
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            
        }
    }
}
