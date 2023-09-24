using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using vermage.Systems.Utilities;

namespace vermage.UI.Components.Spellbook
{
    public class SpellbookSpellNameContainer : UIElement
    {
        public SpellbookSpellName Name;
        private SpellbookColorIndicator Indicator;

        public SpellbookSpellNameContainer()
        {
            Width.Set(341, 0f);
            Height.Set(48, 0f);

            Name = new("Name", 0.80f, true);
            Name.Width.Set(150, 0f);
            Name.Height.Set(42, 0f);
            Name.Left.Set(3, 0f);
            Name.Top.Set(9, 0f);


            Append(Name);

            Indicator = new SpellbookColorIndicator();
            Indicator.Left.Set(155, 0f);
            Indicator.Top.Set(3, 0f);
            Indicator.Width.Set(42, 0f);
            Indicator.Height.Set(42, 0f);

            Append(Indicator);
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            if (!SpellbookUIState.Spell.IsNullOrEmpty()) base.DrawChildren(spriteBatch);
        }
    }
}
