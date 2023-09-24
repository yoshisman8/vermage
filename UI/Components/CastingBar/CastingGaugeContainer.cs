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

namespace vermage.UI.Components.CastingBar
{
    public class CastingGaugeContainer : UIImage
    {
        public CastingGaugeContainer(Asset<Texture2D> texture) : base(texture)
        {

            Vector2 screenPos = VerConfig.Instance.GetCastingBarPosition();

            Left.Set(screenPos.X, 0f);
            Top.Set(screenPos.Y, 0f);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Vector2 screenPos = VerConfig.Instance.GetCastingBarPosition();

            if (Left.Pixels != screenPos.X) Left.Set(screenPos.X, 0f);
            if (Top.Pixels != screenPos.Y) Top.Set(screenPos.Y, 0f);
        }
    }
}
