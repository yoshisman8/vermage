using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using vermage.Items.Abstracts;
using vermage.Systems;

namespace vermage
{
    public class vermage : Mod
	{
		public static DamageClass HealerClass { get; set; }
        public static Mod ThoriumMod { get; set; }
        public static Mod Instance { get; set; }
        public static Dictionary<string, RapierData> Rapiers { get; set; } = new();
        public static Dictionary<string, SpellData> Spells { get; set; } = new();


        public override void Load()
        {
            base.Load();
            Instance = this;
            VerPlayer.ToggleSpellbook = KeybindLoader.RegisterKeybind(this, "Toggle Spellbook", Microsoft.Xna.Framework.Input.Keys.P);
            VerPlayer.SwapSpells = KeybindLoader.RegisterKeybind(this, "Swap Spells", Microsoft.Xna.Framework.Input.Keys.F);

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod))
            {
                ThoriumMod = thoriumMod;

                if (thoriumMod.TryFind("HealerDamage", out DamageClass damageClass))
                {
                    HealerClass = damageClass;
                }
            }
        }


        public override void Unload()
        {
            Instance = null;
            VerPlayer.ToggleSpellbook = null;
            VerPlayer.SwapSpells = null;
            ThoriumMod = null;
            HealerClass = null;
            Rapiers.Clear();
            Spells.Clear();
        }

        internal static void SaveConfig(VerConfig cfg)
        {
            MethodInfo saveMethodInfo = typeof(ConfigManager).GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic);
            if (saveMethodInfo != null)
            {
                saveMethodInfo.Invoke(null, new object[] { cfg });
                return;
            }
            Instance.Logger.Warn("In-game SaveConfig failed, code update required.");
        }
    }
}