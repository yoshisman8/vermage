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

        [Header("Casting Bar Position")]
        [Label("Horizontal Offset")]
        [DefaultValue(50.104603f)]
        [Range(0f, 100f)]
        [Tooltip("The horizontal offset for the casting bar.")]
        public float CastBarX { get; set; }

        [Label("Vertical Offset")]
        [Range(0f, 100f)]
        [DefaultValue(54.112984f)]
        [Tooltip("The vertical offset for the casting bar.")]
        public float CastBarY { get; set; }

        [Label("Lock Casting Bar Position")]
        [DefaultValue(true)]
        [Tooltip("Prevent the moving of the Casting Bar.")]
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
