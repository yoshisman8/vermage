using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using vermage.Systems;
using vermage.Systems.Utilities;

namespace vermage.UI.Components.Spellbook
{
    public class SpellbookColorIndicator : UIElement
    {
        public Asset<Texture2D> texture;
        public SpellbookColorIndicator()
        {
            texture = ModContent.Request<Texture2D>("vermage/Assets/UI/SpellbookManaColor");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (!SpellbookUIState.Spell.IsNullOrEmpty() && IsMouseHovering)
            {
                ManaColor mana = vermage.Spells[SpellbookUIState.Spell].Color;

                switch (mana)
                {
                    case ManaColor.Black:
                        Main.hoverItemName = Language.GetTextValue("Mods.vermage.Tooltips.GenerateBlack");
                        Main.mouseText = true;
                        break;
                    case ManaColor.White:
                        Main.hoverItemName = Language.GetTextValue("Mods.vermage.Tooltips.GenerateWhite");
                        Main.mouseText = true;
                        break;
                    default:
                        Main.hoverItemName = Language.GetTextValue("Mods.vermage.Tooltips.GenerateRed");
                        Main.mouseText = true;
                        break;
                }
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture.Value, GetDimensions().Position(), GetColor(SpellbookUIState.Spell));
        }

        private Color GetColor(string spell)
        {
            if (spell.IsNullOrEmpty()) return Color.White;

            ManaColor mana = vermage.Spells[spell].Color;

            switch (mana)
            {
                case ManaColor.Black:
                    return new Color(167, 73, 201);
                case ManaColor.White:
                    return Color.White;
                default:
                    return new(224, 16, 9);
            }
        }
    }
}
