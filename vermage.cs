using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using vermage.Items.Foci;
using vermage.Systems;

namespace vermage
{
    public class vermage : Mod
	{
		public static DamageClass HealerClass { get; set; }
        public static Mod ThoriumMod { get; set; }
        public static Mod Instance { get; set; }
        public override void Load()
        {
            base.Load();
            Instance = this;
            VerPlayer.ActionKey = KeybindLoader.RegisterKeybind(this, "Activate Focus", Microsoft.Xna.Framework.Input.Keys.Q);
            VerPlayer.QuickFociToggle = KeybindLoader.RegisterKeybind(this, "Quick Focus Swap", Microsoft.Xna.Framework.Input.Keys.F);

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
            VerPlayer.ActionKey = null;
            VerPlayer.QuickFociToggle = null;
            ThoriumMod = null;
            HealerClass = null;
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

        public static BaseFoci[] GetAllFocus()
        {
            return Instance.GetContent<BaseFoci>().ToArray();
        }
    }
}