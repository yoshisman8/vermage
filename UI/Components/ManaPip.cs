using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using vermage.Systems;

namespace vermage.UI.Components
{
    public class ManaPip : UIImage
    {
        private int NumberValue;
        private ManaColor ManaColor;
        private Color BaseColor;
        private int AlphaTimer;
        private bool AlphaDirection;

        VerPlayer player => Main.CurrentPlayer.GetModPlayer<VerPlayer>();
        float ManaQuantity => ManaColor == ManaColor.Black ? player.BlackMana : player.WhiteMana;

        public ManaPip(Asset<Texture2D> texture, int number, ManaColor color) : base(texture)
        {
            NumberValue = number;
            ManaColor = color;
            BaseColor = color == ManaColor.Black ? new Color(167, 73, 201) : Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            AlphaTimer++;

            if (AlphaTimer > 30)
            {
                AlphaTimer = 0;
                AlphaDirection ^= true;
            }
            SetColor();
        }

        private void SetColor()
        {
            if (player.MaxMana < NumberValue + 1)
            {
                Color = new(0, 0, 0);
            }
            else if (ManaQuantity <= NumberValue)
            {
                Color = Color.Transparent;
            }
            else if (ManaQuantity > NumberValue) 
            {
                if (player.GetCombinedMana() >= NumberValue + 1) Color = new(224, 16, 9, GetAlpha());
                else Color = BaseColor with { A = GetAlpha() };
            }
        }

        private byte GetAlpha()
        {
            if (ManaQuantity >= NumberValue + 1 && player.GetCombinedMana() < NumberValue + 1) return 100;
            else
            {
                if (AlphaDirection)
                {
                    return (byte)Math.Min(255, AlphaTimer * 8.5);
                }
                else
                {
                    return (byte)Utils.Clamp(255 - Math.Min(255, AlphaTimer * 8.5), 0, 255);
                }
            }
        }
    }
}
