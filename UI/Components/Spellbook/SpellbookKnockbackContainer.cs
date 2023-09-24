using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.UI;
using vermage.Systems.Utilities;

namespace vermage.UI.Components.Spellbook
{
    public class SpellbookKnockbackContainer : UIElement
    {
        private SpellbookKnockback Text;

        public SpellbookKnockbackContainer()
        {
            Width.Set(97, 0);
            Height.Set(23, 0f);

            Text = new("100%");
            Text.Width.Set(73, 0);
            Text.Height.Set(17,0);
            Text.Left.Set(21, 0);
            Text.Top.Set(3, 0f);

            Append(Text);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsMouseHovering)
            {
                Main.hoverItemName = Language.GetTextValue("Mods.vermage.Tooltips.Knockback");
                Main.mouseText = true;
            }
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            if(!SpellbookUIState.Spell.IsNullOrEmpty()) base.DrawChildren(spriteBatch);
        }
    }
}
