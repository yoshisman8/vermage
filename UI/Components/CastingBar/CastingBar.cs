using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;
using Terraria;
using Terraria.ID;
using vermage.Systems;
using Terraria.GameContent;

namespace vermage.UI.Components.CastingBar
{
    public class CastingBar : UIElement
    {
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float Quotient = CastingGaugeUIState.CastingProgress / (float)CastingGaugeUIState.CastingTime;

            float clamp = Utils.Clamp(Quotient, 0f, 1f);

            var hitbox = GetDimensions().ToRectangle();

            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * clamp);
            for (int i = 0; i < steps; i++)
            {
                float percent = (float)i / (right - left);
                spriteBatch.Draw((Texture2D)TextureAssets.MagicPixel, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), CastingGaugeUIState.Color.GetColor(percent));
            }
        }
    }
}
