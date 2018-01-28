using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TheOneLibrary.Base.UI
{
    public abstract class BaseUI : UIState
    {
        public bool visible;
        public static Color panelColor = new Color(73, 94, 171) * 0.7f;

        public UIPanel panelMain = new UIPanel();

        public virtual void Load()
        {
        }

        public virtual void Load(object[] args)
        {
        }

        public virtual void Unload()
        {
        }

        public virtual void Toggle(bool load = true, bool unload = true, params object[] args)
        {
            visible = !visible;
            if (visible && load)
            {
                if (args.Length > 0) Load(args);
                else Load();
            }
            if (!visible && unload) Unload();
        }

        public Vector2 offset;
        public bool dragging;
        public virtual void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            if (evt.Target != panelMain) return;

            CalculatedStyle dimensions = panelMain.GetDimensions();
            offset = new Vector2(evt.MousePosition.X - dimensions.X, evt.MousePosition.Y - dimensions.Y);
            dragging = true;
        }

        public virtual void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            dragging = false;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = panelMain.GetDimensions();

            if (panelMain.ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
                Main.LocalPlayer.showItemIcon = false;
                Main.ItemIconCacheUpdate(0);
            }

            if (dragging)
            {
                panelMain.Left.Set(MathHelper.Clamp(Main.MouseScreen.X - offset.X, 0, Main.screenWidth - dimensions.Width), 0f);
                panelMain.Top.Set(MathHelper.Clamp(Main.MouseScreen.Y - offset.Y, 0, Main.screenHeight - dimensions.Height), 0f);
                panelMain.Recalculate();
            }
        }
    }
}