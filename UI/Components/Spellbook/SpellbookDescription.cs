using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using vermage.Systems;
using vermage.Systems.Utilities;

namespace vermage.UI.Components.Spellbook
{
    internal class SpellbookDescription : UIText
    {
        private VerPlayer Player => Main.CurrentPlayer.GetModPlayer<VerPlayer>();

        private string cache;
        public SpellbookDescription(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            cache = text;
            Width.Set(197, 0f);
            Height.Set(147, 0f);

            IsWrapped = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (SpellbookUIState.SpellData.HasValue)
            {
                string text; 

                if (Player.HasSpellUnloked(SpellbookUIState.Spell)) text = SpellbookUIState.SpellData.Value.Tooltip.Value;
                else text = SpellbookUIState.SpellData.Value.UnlockHint.Value;

                if (cache != text)
                {
                    SetText(text);
                    cache = text;
                }
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!SpellbookUIState.Spell.IsNullOrEmpty()) base.DrawSelf(spriteBatch);
        }
    }
}
