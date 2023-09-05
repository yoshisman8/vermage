﻿using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.UI;
using vermage.Systems;
using vermage.UI.Components;
using Terraria;
using Terraria.Net;
using Terraria.ID;
using vermage.Systems.Utilities;

namespace vermage.UI
{
    public class CastingGaugeUIState : UIState
    {
        public static Gradient Color;
        public static int TotalCast;
        public static int CastLeft;

        private CastingGaugeContainer Container;
        private UIImage CastingGauge;
        private SpellIcon SpellIcon;
        private SpellName SpellName;
        private CastingBar Bar;
        private CastingTimer Timer;

        public override void OnInitialize()
        {
            base.OnInitialize();

            Gradient def = new(new Color(123, 24, 57));
            def.AddStop(1f, new Color(197, 118, 144));

            Color = def;

            Container = new(ModContent.Request<Texture2D>("vermage/Assets/UI/CastingBarBackground"));
            Container.Width.Set(200f,0f);
            Container.Height.Set(50f, 0f);

            Bar = new();
            Bar.Width.Set(189f, 0f);
            Bar.Height.Set(6f, 0f);
            Bar.Left.Set(5f, 0f);
            Bar.Top.Set(22f, 0f);

            Container.Append(Bar);

            CastingGauge = new(ModContent.Request<Texture2D>("vermage/Assets/UI/CastingBarForeground"));
            CastingGauge.Width.Set(200f, 0f);
            CastingGauge.Height.Set(50f, 0f);

            Container.Append(CastingGauge);

            Timer = new("CASTING");
            Timer.Left.Set(3f, 0f);
            Timer.Top.Set(32f, 0f);

            Container.Append(Timer);

            SpellName = new("JOLT");
            SpellName.Left.Set(6f, 0f);
            SpellName.Top.Set(3f, 0f);

            Container.Append(SpellName);

            SpellIcon = new("vermage/Assets/Spells/Jolt");
            SpellIcon.Left.Set(-48f, 0f);
            SpellIcon.Top.Set(1f, 0f);

            Container.Append(SpellIcon);

            Append(Container);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Main.netMode != NetmodeID.Server)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Player player = Main.CurrentPlayer;
                    if (player.active && !player.dead)
                    {
                        VerPlayer vPlayer = player.GetModPlayer<VerPlayer>();

                        if (vPlayer.IsCasting)
                        {
                            TotalCast = vPlayer.CastFrames.Total;
                            CastLeft = vPlayer.CastFrames.Left;
                        }
                        else
                        {
                            TotalCast = CastLeft = 0;
                        }
                    }
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (CastLeft > 0)
            {
                base.Draw(spriteBatch);
            }
            else return;
        }
    }
}
