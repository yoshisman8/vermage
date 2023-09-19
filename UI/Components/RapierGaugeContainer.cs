using Humanizer;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vermage.Systems;
using Terraria.GameContent.UI.Elements;

namespace vermage.UI.Components
{
    public class RapierGaugeContainer : UIImage
    {
        public RapierGaugeContainer(Asset<Texture2D> texture) : base(texture)
        {

            Vector2 screenPos = VerConfig.Instance.GetRapierGaugePosition();

            Left.Set(screenPos.X, 0f);
            Top.Set(screenPos.Y, 0f);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Vector2 screenPos = VerConfig.Instance.GetRapierGaugePosition();

            if (Left.Pixels != screenPos.X) Left.Set(screenPos.X, 0f);
            if (Top.Pixels != screenPos.Y) Top.Set(screenPos.Y, 0f);
        }
    }
}
