using Humanizer;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReLogic.Content;
using Terraria.UI;
using Terraria;

namespace vermage.UI.Components
{
    public class DraggableUIPanel : UIImage
    {
        // Stores the offset from the top left of the UIPanel while dragging.
        private Vector2 offset;
        public bool dragging;

        public DraggableUIPanel(Asset<Texture2D> texture) : base(texture)
        {
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            if (ShouldDrag()) 
            {
                DragStart(evt);
                base.MouseDown(evt);
            }
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            if (ShouldDrag()) 
            {
                DragEnd(evt);
                base.MouseUp(evt);
            }
        }

        public void DragStart(UIMouseEvent evt)
        {
            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            //offset = new Vector2(0, 0);
            dragging = true;
        }

        public void DragEnd(UIMouseEvent evt)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            Left.Set(end.X - offset.X, 0f);
            Top.Set(end.Y - offset.Y, 0f);

            Recalculate();

            SaveChanges();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 mousePos = new(Main.mouseX, Main.mouseY);

            if (ContainsPoint(mousePos) && dragging)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging)
            {
                Left.Set(mousePos.X - offset.X, 0f);
                Top.Set(mousePos.Y - offset.Y, 0f);
                Recalculate();
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ContainsPoint(Main.MouseScreen) && dragging)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }

        public virtual void SaveChanges()
        {

        }
        public virtual bool ShouldDrag() => true;
    }
}
