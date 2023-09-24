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
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using vermage.Items.Abstracts;
using vermage.Systems;
using vermage.Systems.Handlers;
using vermage.Systems.Utilities;
using vermage.UI.Components.Spellbook;
using static System.Net.Mime.MediaTypeNames;

namespace vermage.UI
{
    public class SpellbookUIState : UIState
    {
        public const string ContainerPath = "vermage/Assets/UI/SpellbookBackground";
        public const string EquipButttonPath = "vermage/Assets/UI/SpellbookEquipButton";
        public const string PrevButton = "vermage/Assets/UI/SpellbookPrevious";
        public const string PrevButtonHover = "vermage/Assets/UI/SpellbookPreviousHover";
        public const string NextButton = "vermage/Assets/UI/SpellbookNext";
        public const string NextButtonHover = "vermage/Assets/UI/SpellbookNextHover";
        public const string DefaultBookmark = "vermage/Assets/UI/SpellbookBookmarkFrame";
        public static bool Shown { get; set; } = false;
        public static string Spell;
        public static void Rebuild()
        {
            ModContent.GetInstance<VerUI>().Spellbook.BuildSpellbook();
        }
        public static SpellData? SpellData => Spell.IsNullOrEmpty() ? null : vermage.Spells[Spell];
        public static int Page = 0;

        private SpellbookContainer Container;
        private UIElement Spellgrid;
        private SpellbookSpellNameContainer NameContainer;
        private SpellbookCastingTimeContainer CastingTimeContainer;
        private SpellbookManaCostContainer ManaCostContainer;
        private SpellbookDamageContainer DamageContainer;
        private SpellbookKnockbackContainer KnockbackContainer;
        private SpellbookDescription DescriptionBox;
        private SpellbookEquipIcon EquipButton;
        private UIImageButton Prev;
        private UIImageButton Next;
        private UIText PageNum;
        private List<SpellbookIcon> IconList = new();
        private List<SpellbookBookmark> Bookmarks = new();
        private VerPlayer Player => Main.LocalPlayer?.GetModPlayer<VerPlayer>();

        public override void OnInitialize()
        {
            base.OnInitialize();

            Container = new(ModContent.Request<Texture2D>(ContainerPath));

            NameContainer = new();
            NameContainer.Left.Set(341, 0);
            NameContainer.Top.Set(48, 0);
            Container.Append(NameContainer);

            CastingTimeContainer = new();
            CastingTimeContainer.Left.Set(342, 0f);
            CastingTimeContainer.Top.Set(99, 0f);
            Container.Append(CastingTimeContainer);

            ManaCostContainer = new();
            ManaCostContainer.Left.Set(444, 0f);
            ManaCostContainer.Top.Set(99, 0f);
            Container.Append(ManaCostContainer);

            DamageContainer = new();
            DamageContainer.Left.Set(342, 0f);
            DamageContainer.Top.Set(125, 0f);
            Container.Append(DamageContainer);

            KnockbackContainer = new();
            KnockbackContainer.Left.Set(444, 0f);
            KnockbackContainer.Top.Set(125, 0f);
            Container.Append(KnockbackContainer);

            DescriptionBox = new("-");
            DescriptionBox.Left.Set(343, 0f);
            DescriptionBox.Top.Set(152, 0f);
            Container.Append(DescriptionBox);


            EquipButton = new(ModContent.Request<Texture2D>(EquipButttonPath), Language.GetText("Mods.vermage.Tooltips.Equip").Value);
            EquipButton.Left.Set(393, 0f);
            EquipButton.Top.Set(307, 0f);
            Container.Append(EquipButton);

            

            Spellgrid = new();
            Spellgrid.Width.Set(201, 0f);
            Spellgrid.Height.Set(282, 0f);
            Spellgrid.Left.Set(92, 0f);
            Spellgrid.Top.Set(47, 0f);
            Container.Append(Spellgrid);

            //BuildSpellbook();

            for (int i = 0; i < 5; i++)
            {
                SpellbookBookmark icon = new(ModContent.Request<Texture2D>(DefaultBookmark), i);
                icon.Top.Set(22 + (i * 64), 0f);
                icon.Left.Set(12, 0f);
                Bookmarks.Add(icon);
                Container.Append(icon);
            }

            Append(Container);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!Shown)
            {
                if (!Spell.IsNullOrEmpty()) Spell = null;
                if (Page > 0) { Page = 0; BuildSpellbook(); }
            }
        }

        internal void BuildSpellbook()
        {
            RemoveAllChildren();
            IconList = new();
            Bookmarks = new();

            Container = new(ModContent.Request<Texture2D>(ContainerPath));

            NameContainer = new();
            NameContainer.Left.Set(341, 0);
            NameContainer.Top.Set(48, 0);
            Container.Append(NameContainer);

            CastingTimeContainer = new();
            CastingTimeContainer.Left.Set(342, 0f);
            CastingTimeContainer.Top.Set(99, 0f);
            Container.Append(CastingTimeContainer);

            ManaCostContainer = new();
            ManaCostContainer.Left.Set(444, 0f);
            ManaCostContainer.Top.Set(99, 0f);
            Container.Append(ManaCostContainer);

            DamageContainer = new();
            DamageContainer.Left.Set(342, 0f);
            DamageContainer.Top.Set(125, 0f);
            Container.Append(DamageContainer);

            KnockbackContainer = new();
            KnockbackContainer.Left.Set(444, 0f);
            KnockbackContainer.Top.Set(125, 0f);
            Container.Append(KnockbackContainer);

            DescriptionBox = new("-");
            DescriptionBox.Left.Set(343, 0f);
            DescriptionBox.Top.Set(152, 0f);
            Container.Append(DescriptionBox);


            EquipButton = new(ModContent.Request<Texture2D>(EquipButttonPath), Language.GetText("Mods.vermage.Tooltips.Equip").Value);
            EquipButton.Left.Set(393, 0f);
            EquipButton.Top.Set(307, 0f);
            Container.Append(EquipButton);

            for (int i = 0; i < 5; i++)
            {
                SpellbookBookmark icon = new(ModContent.Request<Texture2D>(DefaultBookmark), i);
                icon.Top.Set(22 + (i * 64), 0f);
                icon.Left.Set(12, 0f);
                Bookmarks.Add(icon);
                Container.Append(icon);
            }

            Spellgrid = new();
            Spellgrid.Width.Set(201, 0f);
            Spellgrid.Height.Set(282, 0f);
            Spellgrid.Left.Set(92, 0f);
            Spellgrid.Top.Set(47, 0f);
            Container.Append(Spellgrid);

            PageNum = new((Page + 1).ToString());
            PageNum.Width.Set(27, 0f);
            PageNum.Height.Set(20, 0f);
            PageNum.Left.Set(87, 0f);
            PageNum.Top.Set(262, 0f);
            Spellgrid.Append(PageNum);

            Prev = new(ModContent.Request<Texture2D>(PrevButton));
            Prev.SetHoverImage(ModContent.Request<Texture2D>(PrevButtonHover));
            Prev.SetVisibility(1f, 1f);
            Prev.Left.Set(0, 0);
            Prev.Top.Set(262, 0f);
            Prev.OnLeftClick += Prev_OnLeftClick;
            Spellgrid.Append(Prev);

            Next = new(ModContent.Request<Texture2D>(NextButton));
            Next.SetHoverImage(ModContent.Request<Texture2D>(NextButtonHover));
            Next.SetVisibility(1f, 1f);
            Next.Left.Set(175, 0);
            Next.Top.Set(262, 0f);
            Next.OnLeftClick += Next_OnLeftClick;
            Spellgrid.Append(Next);

            List<string> Spells = Player.UnlockedSpells.Where(x => x.Value).Select(x => x.Key).OrderBy(x => vermage.Spells[x].Name.Value).ToList();
            Spells.AddRange(Player.UnlockedSpells.Where(x => !x.Value).Select(x => x.Key).OrderBy(x => vermage.Spells[x].Name.Value));

            // Number of elements being displayed.
            int items = Math.Min(20, Spells.Count - (20 * Page));

            // Where in this list we are starting to fetch. Starts at 0, goes up in increments of 20.
            int StartingIndex = 0 + (20 * Page);

            Range range = new(StartingIndex, StartingIndex + items);

            var renderList = Spells.Take(range).ToArray();

            for (int i = 0; i < renderList.Count(); i++)
            {
                SpellData data = vermage.Spells[renderList[i]];

                Asset<Texture2D> texture = ModContent.Request<Texture2D>(data.IconPath);
                SpellbookIcon icon = new(texture, renderList[i]);

                int row = (int)Math.Floor((i) / 4f);
                int column = i - (4 * row);

                icon.Left.Set(0 + (51 * column), 0f);
                icon.Top.Set(1 + (51 * row), 0f);

                Spellgrid.Append(icon);
                IconList.Add(icon);
            }

            Container.Append(Spellgrid);

            Append(Container);
        }
        private int GetTotalPages() => (int)Math.Ceiling((float)vermage.Spells.Count / 20f);
        private void Next_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Page < GetTotalPages() - 1)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Page++;
                BuildSpellbook();
            }
            if (Page == GetTotalPages() - 1)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Page = 0;
                BuildSpellbook();
            }
        }

        private void Prev_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Page > 0)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Page--;
                BuildSpellbook();
            }
            if (Page == 0)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Page = GetTotalPages() -1;
                BuildSpellbook();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Shown) base.Draw(spriteBatch);
        }
    }
}
