using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;


namespace vermage.Systems
{
    public class VermilionDamageClass : DamageClass
    {
        public override bool UseStandardCritCalcs => true;

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Magic) return true;
            if (ModLoader.HasMod("ThoriumMod"))
            {
                if(damageClass == vermage.HealerClass) return true;
            }
            return false;
        }
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Magic) return StatInheritanceData.Full;
            if (ModLoader.HasMod("ThoriumMod"))
            {
                if (damageClass == vermage.HealerClass) return StatInheritanceData.Full;
            }
            return StatInheritanceData.None;
        }
    }
}
