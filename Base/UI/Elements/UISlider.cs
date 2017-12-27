using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace TheOneLibrary.UI.Elements
{
	public class UISlider : BaseElement
	{
		private static Color color = new Color(43, 56, 101, 200);

		private Texture2D bar = Main.colorBarTexture;
		private Texture2D slider = Main.colorSliderTexture;

		public int CurrentValue
		{
			get { return currentValue; }
			set
			{
				sliderPos = (GetDimensions().Width - 4 - slider.Width) / maxValue * value;
				currentValue = value;
			}
		}

		private int currentValue;
		public int minValue;
		public int maxValue;

		private bool dragging;
		private float sliderPos;

		public UISlider(int minValue = 0, int maxValue = 100)
		{
			this.minValue = minValue;
			this.maxValue = maxValue;
		}

		public override void MouseDown(UIMouseEvent evt)
		{
			dragging = true;
		}

		public override void MouseUp(UIMouseEvent evt)
		{
			CalculatedStyle dimensions = GetDimensions();

			dragging = false;

			sliderPos = MathHelper.Clamp(evt.MousePosition.X - dimensions.X, slider.Width / 2f + 2, dimensions.Width - slider.Width / 2f - 2);
			sliderPos -= slider.Width / 2f;

			currentValue = (int)((sliderPos - 2) * (maxValue / (dimensions.Width - 4 - slider.Width)));

			Recalculate();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (visible)
			{
				CalculatedStyle dimensions = GetDimensions();

				spriteBatch.Draw(bar, dimensions.Position(), new Rectangle(0, 0, 5, 16), color);
				spriteBatch.Draw(bar, dimensions.Position() + new Vector2(5, 0), new Rectangle(6, 0, 1, 16), color, 0f, Vector2.Zero, new Vector2(dimensions.Width - 9, 1), SpriteEffects.None, 0f);
				spriteBatch.Draw(bar, dimensions.Position() + new Vector2(dimensions.Width - 4, 0), new Rectangle(bar.Width - 4, 0, 4, 16), color);

				if (dragging)
				{
					sliderPos = MathHelper.Clamp(Main.mouseX - dimensions.X, slider.Width / 2f + 2, dimensions.Width - slider.Width / 2f - 2);
					sliderPos -= slider.Width / 2f;

					currentValue = (int)((sliderPos - 2) * (maxValue / (dimensions.Width - 4 - slider.Width)));

					Recalculate();
				}

				spriteBatch.Draw(slider, dimensions.Position() + new Vector2(sliderPos, dimensions.Height / 2 - slider.Height / 2f), Color.White);

				Rectangle sliderRect = new Rectangle((int)(dimensions.X + sliderPos), (int)(dimensions.Y + dimensions.Height / 2 - slider.Height / 2f), slider.Width, slider.Height);
				if (IsMouseHovering && sliderRect.Contains(Main.mouseX, Main.mouseY))
				{
					Main.LocalPlayer.showItemIcon = false;
					Main.ItemIconCacheUpdate(0);
					Main.instance.MouseTextHackZoom(currentValue.ToString());
					Main.mouseText = true;
				}
			}
		}
	}
}