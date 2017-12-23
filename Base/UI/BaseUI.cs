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

		public void Toggle(bool load = true, bool unload = true, params object[] args)
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

		public void DragStart(UIMouseEvent evt, UIElement listeningElement)
		{
			offset = new Vector2(evt.MousePosition.X - panelMain.Left.Pixels, evt.MousePosition.Y - panelMain.Top.Pixels);
			dragging = true;
		}

		public void DragEnd(UIMouseEvent evt, UIElement listeningElement)
		{
			if (dragging)
			{
				Vector2 end = evt.MousePosition;
				dragging = false;

				panelMain.Left.Set(end.X - offset.X, 0f);
				panelMain.Top.Set(end.Y - offset.Y, 0f);
			}

			Recalculate();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Vector2 MousePosition = new Vector2(Main.mouseX, Main.mouseY);
			if (panelMain.ContainsPoint(MousePosition)) Main.LocalPlayer.mouseInterface = true;
			if (dragging)
			{
				panelMain.Left.Set(MousePosition.X - offset.X, 0f);
				panelMain.Top.Set(MousePosition.Y - offset.Y, 0f);
				Recalculate();
			}
		}
	}
}