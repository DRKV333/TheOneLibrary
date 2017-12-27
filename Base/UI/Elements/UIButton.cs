using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace TheOneLibrary.UI.Elements
{
	public class UIButton : BaseElement
	{
		public Texture2D texture;

		public float opacityActive = 1f;
		public float opacityInactive = 0.6f;
		public Color color = Color.White;

		public string Text;

		public bool toggle;
		public bool toggleOnHover;

		public UIButton(Texture2D texture, bool toggleOnHover = false)
		{
			this.texture = texture;
			this.toggleOnHover = toggleOnHover;
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			toggle = true;

			base.MouseOver(evt);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			toggle = false;

			base.MouseOut(evt);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (visible && texture != null)
			{
				CalculatedStyle dimensions = GetDimensions();
				spriteBatch.Draw(texture, new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height), color * (toggleOnHover && toggle ? opacityActive : opacityInactive));

				if (IsMouseHovering && !string.IsNullOrWhiteSpace(Text))
				{
					Main.LocalPlayer.showItemIcon = false;
					Main.ItemIconCacheUpdate(0);
					Main.instance.MouseTextHackZoom(Text);
					Main.mouseText = true;
				}
			}
		}
	}
}