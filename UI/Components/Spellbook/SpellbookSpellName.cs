using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using vermage.Systems.Utilities;

namespace vermage.UI.Components.Spellbook
{
    public class SpellbookSpellName : UIText
    {
        public string cache;
        public SpellbookSpellName(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            cache = text;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!SpellbookUIState.Spell.IsNullOrEmpty())
            {
                if (cache != SpellbookUIState.SpellData.Value.Name.Value)
                {
                    SetText(SpellbookUIState.SpellData.Value.Name.Value);
                    cache = SpellbookUIState.SpellData.Value.Name.Value;
                }
            }
        }
    }
}
