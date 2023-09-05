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
using Terraria.ID;
using Terraria.ModLoader;
using vermage.Systems;
using vermage.Systems.Utilities;

namespace vermage.UI.Components
{
    public class SpellIcon : UIImage
    {
        private string Cache;
        public SpellIcon(string path) : base(ModContent.Request<Texture2D>(path))
        {
            Cache = path;
            var texture = ModContent.Request<Texture2D>(path);
            
            SetImage(texture);
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

                        SpellData? data = vPlayer.ActiveSpell;

                        if (!data.HasValue) return;

                        if (data.Value.IconPath != Cache)
                        {
                            SetImage(ModContent.Request<Texture2D>(data.Value.IconPath));
                            Cache = data.Value.IconPath;
                        }
                    }
                }
            }
        }
    }
}
