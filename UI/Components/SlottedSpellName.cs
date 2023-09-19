using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using vermage.Systems;

namespace vermage.UI.Components
{
    public class SlottedSpellName : UIText
    {
        public string Cache;
        VerPlayer player => Main.CurrentPlayer.GetModPlayer<VerPlayer>();

        public SlottedSpellName(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            Cache = text;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (player.GetCurrentSpell().HasValue)
            {
                string name = player.GetCurrentSpell().Value.Name.Value;
                if (Cache != name)
                {
                    SetText(name);
                    Cache = name;
                }
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!player.GetCurrentSpell().HasValue) return;
            base.DrawSelf(spriteBatch);
        }
    }
}
