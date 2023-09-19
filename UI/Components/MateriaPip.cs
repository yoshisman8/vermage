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
using vermage.Items.Materia;
using vermage.Systems;

namespace vermage.UI.Components
{
    public class MateriaPip : UIImage
    {
        private int Slot;
        VerPlayer player => Main.CurrentPlayer.GetModPlayer<VerPlayer>();

        public MateriaPip(Asset<Texture2D> texture, int slot) : base(texture)
        {
            Slot = slot;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            SetColor();
            base.DrawSelf(spriteBatch);
        }
        private void SetColor()
        {
            if (player.MaterialColors.Count < Slot + 1) Color = Color.Transparent;
            else
            {
                MateriaColor mat = player.MaterialColors[Slot];
                switch (mat)
                {
                    case MateriaColor.Red:
                        Color = new(255, 34, 0, 100); break;
                    case MateriaColor.Green:
                        Color = new(60, 255, 0, 100); break;
                    case MateriaColor.Blue:
                        Color = new(0, 110, 255, 100); break;
                    case MateriaColor.Purple:
                        Color = new(226, 5, 255, 100); break;
                    case MateriaColor.Orange:
                        Color = new(255, 145, 0, 100); break;
                    case MateriaColor.Yellow:
                        Color = new(247, 255, 128, 100); break;
                    default:
                        Color = Color.White; break;
                }
            }
        }
    }
}
