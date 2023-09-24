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
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using vermage.Items.Abstracts;
using vermage.Systems;

namespace vermage.UI.Components.Spellbook
{
    public class SpellbookIcon : UIImageButton
    {
        private const string FramePath = "vermage/Assets/UI/SpellbookPageFrame";
        private const string SelectedPath = "vermage/Assets/UI/SpellbookInspect";
        private const string LockedPath = "vermage/Assets/UI/SpellbookLocked";
        private Asset<Texture2D> Frame;
        private Asset<Texture2D> Selected;
        private Asset<Texture2D> Locked;
        private string SpellID;
        private VerPlayer Player => Main.CurrentPlayer.GetModPlayer<VerPlayer>();

        public SpellbookIcon(Asset<Texture2D> texture, string _spellID) : base(texture)
        {
            Frame = ModContent.Request<Texture2D>(FramePath);
            Selected = ModContent.Request<Texture2D>(SelectedPath);
            Locked = ModContent.Request<Texture2D>(LockedPath);
            SpellID = _spellID;


            SetVisibility(1f, 1f);
            SetHoverImage(Selected);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (!Player.HasSpellUnloked(SpellID) && !IsMouseHovering)
            {
                spriteBatch.Draw(Locked.Value, GetDimensions().Position(), Color.White);
            }
            if (SpellbookUIState.Spell == SpellID && !IsMouseHovering)
            {
                spriteBatch.Draw(Selected.Value, GetDimensions().Position(), Color.White);
            }
            spriteBatch.Draw(Frame.Value, GetDimensions().Position() + new Vector2(-1, -1), Color.White);
        }
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            SpellbookUIState.Spell = SpellID;
            SoundEngine.PlaySound(SoundID.MenuTick);
        }
    }
}
