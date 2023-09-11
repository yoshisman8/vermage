using log4net.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace vermage.Systems
{
    public class VerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static VerConfig Instance;

        [Header("$Mods.vermage.Configs.Headers.CastingBar")]
        [LabelKey("$Mods.vermage.Configs.CastBarX.DisplayName")]
        [TooltipKey("$Mods.vermage.Configs.CastBarX.Tooltip")]
        [DefaultValue(50.104603f)]
        [Range(0f, 100f)]
        public float CastBarX { get; set; }


        [LabelKey("$Mods.vermage.Configs.CastBarY.DisplayName")]
        [TooltipKey("$Mods.vermage.Configs.CastBarY.Tooltip")]
        [Range(0f, 100f)]
        [DefaultValue(54.112984f)]
        public float CastBarY { get; set; }

        [LabelKey("$Mods.vermage.Configs.LockCastingBar.DisplayName")]
        [TooltipKey("$Mods.vermage.Configs.LockCastingBar.Tooltip")]
        [DefaultValue(true)]
        public bool LockCastingBar { get; set; }

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            return true;
        }

        public static void Unload()
        {
            Instance = null;
        }
    }
}
