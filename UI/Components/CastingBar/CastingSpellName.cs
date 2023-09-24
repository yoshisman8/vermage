using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using vermage.Systems;
using vermage.Systems.Utilities;

namespace vermage.UI.Components.CastingBar
{
    public class CastingSpellName : UIText
    {
        public string Cache;
        public CastingSpellName(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            Cache = text;
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

                        if (!vPlayer.CastingSpell.HasValue) return;

                        if (vPlayer.CastingSpell.Value.Name.Value != Cache)
                        {
                            SetText(vPlayer.CastingSpell.Value.Name.Value);
                            Cache = vPlayer.CastingSpell.Value.Name.Value;
                        }
                    }
                }
            }
        }
    }
}
