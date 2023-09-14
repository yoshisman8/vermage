using log4net.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
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
        [DefaultValue(0.50104603f)]
        [Range(0f, 1f)]
        public float CastBarX { get; set; }

        [LabelKey("$Mods.vermage.Configs.CastBarY.DisplayName")]
        [TooltipKey("$Mods.vermage.Configs.CastBarY.Tooltip")]
        [Range(0f, 1f)]
        [DefaultValue(0.54112984f)]
        public float CastBarY { get; set; }

        [Header("$Mods.vermage.Configs.Headers.RapierGauge")]
        [LabelKey("$Mods.vermage.Configs.RapierGaugeX.DisplayName")]
        [TooltipKey("$Mods.vermage.Configs.RapierGaugeX.Tooltip")]
        [DefaultValue(0.50104603f)]
        [Range(0f, 1f)]
        public float RapierGaugeX { get; set; }


        [LabelKey("$Mods.vermage.Configs.RapierGaugeY.DisplayName")]
        [TooltipKey("$Mods.vermage.Configs.RapierGaugeY.Tooltip")]
        [Range(0f, 1f)]
        [DefaultValue(0.54112984f)]
        public float RapierGaugeY { get; set; }

        [Header("$Mods.vermage.Configs.Headers.MateriaSlots")]
        [LabelKey("$Mods.vermage.Configs.MateriaSlotsX.DisplayName")]
        [TooltipKey("$Mods.vermage.Configs.MateriaSlotsX.Tooltip")]
        [DefaultValue(0.82f)]
        [Range(0f, 1f)]
        public float MateriaSlotsX { get; set; }


        [LabelKey("$Mods.vermage.Configs.MateriaSlotsY.DisplayName")]
        [TooltipKey("$Mods.vermage.Configs.MateriaSlotsY.Tooltip")]
        [Range(0f, 1f)]
        [DefaultValue(0.489999999f)]
        public float MateriaSlotsY { get; set; }

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            return true;
        }

        public Vector2 GetCastingBarPosition()
        {
            return new(Main.screenWidth * CastBarX, Main.screenHeight * CastBarY);
        }
        public Vector2 GetMateriaSlotPosition()
        {
            return new(Main.screenWidth * MateriaSlotsX, Main.screenHeight * MateriaSlotsY);
        }
        public Vector2 GetManaGaugePosition()
        {
            return new(Main.screenWidth * RapierGaugeX, Main.screenHeight * RapierGaugeY);
        }
    }
}
