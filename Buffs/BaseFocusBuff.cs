using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.NetModules;
using Terraria.ModLoader;
using vermage.Items.Foci;
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
            if (player.dead && !player.active)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
                return;
            }
            VerPlayer vPlayer = player.GetModPlayer<VerPlayer>();

            BaseFoci[] FocusList = vermage.GetAllFocus();

            if (FocusList.Length == 0)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
                return;
            }

            if(!FocusList.Any((f) => f.Item.buffType == Type))
            {
                player.DelBuff(buffIndex);
                buffIndex--;
                return;
            }

            BaseFoci focus = FocusList.First(f => f.Item.buffType == Type);

            vPlayer.FocusOnCast = focus.OnCast;
            vPlayer.FocusOnHitNPC = focus.OnHitNPC;
            vPlayer.FocusOnHitPlayer = focus.OnHitPlayer;

            player.buffTime[buffIndex] = 120;
            player.GetModPlayer<VerPlayer>().Focus = (focus.Type, Type, focus.Item.shoot);

            if (player.ownedProjectileCounts[focus.Item.shoot] < 0)
            {
                Projectile.NewProjectileDirect(player.GetSource_Buff(buffIndex), player.position, new Vector2(0), focus.Item.shoot, 0, 0, player.whoAmI, Type);
            }
        }
    }
}
