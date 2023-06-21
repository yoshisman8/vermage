using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using vermage.Systems;

namespace vermage.UI.Components
{
    public class CastingGaugeContainer : DraggableUIPanel
    {
        public CastingGaugeContainer(Asset<Texture2D> texture) : base(texture)
        {
            Vector2 screenRatioPosition = new(VerConfig.Instance.CastBarX, VerConfig.Instance.CastBarY);

            if (screenRatioPosition.X < 0f || screenRatioPosition.X > 100f)
            {
                screenRatioPosition.X = 50.104603f;
            }
            if (screenRatioPosition.Y < 0f || screenRatioPosition.Y > 100f)
            {
                screenRatioPosition.Y = 54.112984f;
            }

            Vector2 screenPos = screenRatioPosition;
            screenPos.X = (int)(screenPos.X * 0.01f * Main.screenWidth);
            screenPos.Y = (int)(screenPos.Y * 0.01f * Main.screenHeight);

            Left.Set(screenPos.X, 0f);
            Top.Set(screenPos.Y, 0f);
        }

        public override void SaveChanges()
        {
            var dimensions = GetDimensions();

            Vector2 Relative = new(dimensions.X / Main.screenWidth * 100f, dimensions.Y / Main.screenHeight * 100f);

            if (VerConfig.Instance.CastBarX != Relative.X || VerConfig.Instance.CastBarY != Relative.Y)
            {
                VerConfig.Instance.CastBarX = Relative.X;
                VerConfig.Instance.CastBarY = Relative.Y;
                vermage.SaveConfig(VerConfig.Instance);
            }
        }
        public override bool ShouldDrag()
        {
            return !VerConfig.Instance.LockCastingBar;
        }
    }
}
