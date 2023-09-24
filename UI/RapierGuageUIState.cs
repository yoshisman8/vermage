using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.UI;
using vermage.Systems;
using vermage.UI.Components.ManaGauge;

namespace vermage.UI
{
    public class RapierGuageUIState : UIState
    {
        private const string Gauge = "vermage/Assets/UI/RapierGauge";
        private const string GaugePipB = "vermage/Assets/UI/RapierGaugeSegmentB";
        private const string GaugePipW = "vermage/Assets/UI/RapierGaugeSegmentW";
        private const string Materia = "vermage/Assets/UI/MateriaRapierGauge";

        public ManaGaugeContainer Container;
        public List<ManaGaugePip> BlackPips = new();
        public List<ManaGaugePip> WhitePips = new();
        public ManaGaugeMateria MateriaPip1;
        public ManaGaugeMateria MateriaPip2;
        public ManaGaugeMateria MateriaPip3;
        public ManaGaugeSpellName SpellName;

        VerPlayer player => Main.CurrentPlayer.GetModPlayer<VerPlayer>();

        public override void OnInitialize()
        {
            base.OnInitialize();

            Container = new(ModContent.Request<Texture2D>(Gauge));
            Container.Width.Set(382f, 0f);
            Container.Height.Set(64f, 0f);

            for (int i = 0; i < 5; i++)
            {
                ManaGaugePip pip = new(ModContent.Request<Texture2D>(GaugePipB), i, Systems.ManaColor.Black);
                pip.Width.Set(44, 0f);
                pip.Height.Set(4, 0f);
                pip.Left.Set(118 + (50 * i), 0f);
                pip.Top.Set(20, 0f);

                Container.Append(pip);
                BlackPips.Add(pip);
            }

            for (int i = 0; i < 5; i++)
            {
                ManaGaugePip pip = new(ModContent.Request<Texture2D>(GaugePipW), i, Systems.ManaColor.White);
                pip.Width.Set(44, 0f);
                pip.Height.Set(4, 0f);
                pip.Left.Set(118 + (50 * i), 0f);
                pip.Top.Set(28, 0f);

                Container.Append(pip);
                WhitePips.Add(pip);
            }

            MateriaPip1 = new(ModContent.Request<Texture2D>(Materia), 0);
            MateriaPip1.Width.Set(14, 0f);
            MateriaPip1.Height.Set(14, 0f);
            MateriaPip1.Left.Set(68, 0f);
            MateriaPip1.Top.Set(46, 0f);

            Container.Append(MateriaPip1);

            MateriaPip2 = new(ModContent.Request<Texture2D>(Materia), 1);
            MateriaPip2.Width.Set(14, 0f);
            MateriaPip2.Height.Set(14, 0f);
            MateriaPip2.Left.Set(88, 0f);
            MateriaPip2.Top.Set(48, 0f);

            Container.Append(MateriaPip2);

            MateriaPip3 = new(ModContent.Request<Texture2D>(Materia), 2);
            MateriaPip3.Width.Set(14, 0f);
            MateriaPip3.Height.Set(14, 0f);
            MateriaPip3.Left.Set(108, 0f);
            MateriaPip3.Top.Set(48, 0f);
            
            Container.Append(MateriaPip3);

            SpellName = new("SpellName");
            SpellName.Left.Set(165, 0f);
            SpellName.Top.Set(39, 0f);

            Container.Append(SpellName);

            Append(Container);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (player.Rapier == null) return;
            base.Draw(spriteBatch);
        }
    }
}
