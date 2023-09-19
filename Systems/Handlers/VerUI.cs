using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using vermage.UI;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace vermage.Systems.Handlers
{
    public class VerUI : ModSystem
    {
        internal RapierGuageUIState ManaBar;
        internal CastingGaugeUIState CastingBar;

        private UserInterface _ManaInterface;
        private UserInterface _CastingBar;
        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
            _ManaInterface?.Update(gameTime);
            _CastingBar?.Update(gameTime);
        }
        public override void Load()
        {
            base.Load();

            if (Main.netMode != NetmodeID.Server)
            {
                ManaBar = new RapierGuageUIState();
                ManaBar.Activate();

                _ManaInterface = new UserInterface();
                _ManaInterface.SetState(ManaBar);

                CastingBar = new();
                CastingBar.Activate();

                _CastingBar = new();
                _CastingBar.SetState(CastingBar);
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int manaBarIndex = layers.FindIndex(a => a.Name.Equals("Vanilla: Resource Bars"));
            if (manaBarIndex != -1)
            {
                layers.Insert(manaBarIndex, new LegacyGameInterfaceLayer("vermage: ManaBar",
                    delegate
                    {
                        _ManaInterface?.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                InterfaceScaleType.UI));

                layers.Insert(manaBarIndex, new LegacyGameInterfaceLayer("vermage: CastingBar",
                    delegate
                    {
                        _CastingBar?.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                InterfaceScaleType.UI));
            }
        }
    }
}
