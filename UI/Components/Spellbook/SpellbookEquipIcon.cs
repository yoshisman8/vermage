using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using vermage.Systems;
using vermage.Systems.Utilities;

namespace vermage.UI.Components.Spellbook
{
    public class SpellbookEquipIcon : UIImageButton
    {
        private UIText Text;
        private VerPlayer Player => Main.CurrentPlayer.GetModPlayer<VerPlayer>();

        public SpellbookEquipIcon(Asset<Texture2D> texture, string _text) : base(texture)
        {
            Width.Set(97, 0f);
            Height.Set(23, 0f);

            Text = new(_text);
            Text.Left.Set(21, 0);
            Text.Top.Set(3, 0f);
            Text.Width.Set(73, 0f);
            Text.Height.Set(17, 0f);

            Append(Text);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!SpellbookUIState.Spell.IsNullOrEmpty())
            {
                if (Player.HasSpellUnloked(SpellbookUIState.Spell)) base.Draw(spriteBatch);
            }
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (SpellbookUIState.Spell.IsNullOrEmpty()) return;
            if (!Player.HasSpellUnloked(SpellbookUIState.Spell)) return;

            if (Player.EquippedSpells.Count >= 5) return;
            if (Player.EquippedSpells.Contains(SpellbookUIState.Spell)) return;

            Player.EquippedSpells.Add(SpellbookUIState.Spell);
            SoundEngine.PlaySound(new("vermage/Assets/Sounds/Scribble", SoundType.Sound));
            SpellbookUIState.Spell = null;
        }
    }
}
