using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using vermage.Items.Abstracts;
using vermage.Systems;

namespace vermage.UI.Components.Spellbook
{
    internal class SpellbookBookmark : UIImageButton
    {
        private const string FramePath = "vermage/Assets/UI/SpellbookBookmarkFrame";
        private const string DeletePath = "vermage/Assets/UI/SpellbookDelete";

        private Asset<Texture2D> Delete;
        private UIImage Frame;
        private int Slot;

        private VerPlayer Player => Main.CurrentPlayer.GetModPlayer<VerPlayer>();

        public SpellbookBookmark(Asset<Texture2D> texture, int _slot) : base(texture)
        {
            Frame = new(ModContent.Request<Texture2D>(FramePath));
            Delete = ModContent.Request<Texture2D>(DeletePath);
            Slot = _slot;

            Append(Frame);

            SetVisibility(1f, 1f);
            SetHoverImage(Delete);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Player.GetSpellInSlot(Slot).HasValue)
            {
                SpellData data = Player.GetSpellInSlot(Slot).Value;

                SetImage(ModContent.Request<Texture2D>(data.IconPath));

                if (IsMouseHovering)
                {
                    Main.hoverItemName = Language.GetTextValue("Mods.vermage.Tooltips.Unequip", data.Name.Value);
                    Main.mouseText = true;
                }
                base.Draw(spriteBatch);
            }
        }
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            if (Player.GetSpellInSlot(Slot).HasValue)
            {
                Player.EquippedSpells.RemoveAt(Slot);
                Player.SelectedSlot = 0;
                SoundEngine.PlaySound(new("vermage/Assets/Sounds/Scribble", SoundType.Sound));
            }
        }
    }
}
