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
    public class SpellbookCastingTime : UIText
    {
        private string cache;

        public SpellbookCastingTime(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            cache = text;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!SpellbookUIState.Spell.IsNullOrEmpty())
            {
                string text = string.Format("{0:F2}s", SpellbookUIState.SpellData.Value.CastingTime);
                if (cache != text)
                {
                    SetText(text);
                    cache = text;
                }
            }
        }
        private VerPlayer Player => Main.CurrentPlayer.GetModPlayer<VerPlayer>();
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!SpellbookUIState.Spell.IsNullOrEmpty())
            {
                if (Player.HasSpellUnloked(SpellbookUIState.Spell)) base.DrawSelf(spriteBatch);
            }
        }
    }
}
