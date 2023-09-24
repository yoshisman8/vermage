using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using vermage.Systems;
using Terraria;
using vermage.Items.Abstracts;
using Terraria.ModLoader.Config;
using Terraria.ModLoader;
using System.Data;

namespace vermage.UI.Components.Spellbook
{
    public class SpellbookContainer : UIImage
    {
        public SpellbookContainer(Asset<Texture2D> texture) : base(texture)
        {
            Vector2 screenPos = new(Main.screenWidth / 2, Main.screenHeight / 2);
            Width.Set(582, 0f);
            Height.Set(350, 0f);

            Left.Set(screenPos.X - Width.Pixels / 2, 0f);
            Top.Set(screenPos.Y - Height.Pixels / 2, 0f);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
