using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;

namespace vermage.UI.Components
{
    public class CastingTimer : UIText
    {
        private float seconds;
        public CastingTimer(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (CastingGaugeUIState.CastLeft > 0)
            {
                seconds = (float)CastingGaugeUIState.CastLeft / 60f;

                SetText("CASTING " + seconds.ToString("00.00"));
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (CastingGaugeUIState.CastLeft > 0)
            {
                base.Draw(spriteBatch);
            }
            else
            {
                return;
            }
            
        }
    }
}
