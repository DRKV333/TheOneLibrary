using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.UI;

namespace TheOneLibrary.UI.Elements
{
	public class UIStepSlider : BaseElement
	{
		private static Color color = new Color(43, 56, 101, 200);

		private Texture2D bar = Main.colorBarTexture;
		private Texture2D slider = Main.colorSliderTexture;
		
		private bool dragging;
		public float SliderPos
		{
			get { return GetDimensions().Width / maxValue * CurrentValue; }
		}

		public int CurrentValue
		{
			get
			{
				if (member is FieldInfo) return (int)((FieldInfo)member).GetValue(obj);
				if (member is PropertyInfo) return (int)((PropertyInfo)member)?.GetValue(obj);
				return 0;
			}
			set { (member as FieldInfo)?.SetValue(obj, value); (member as PropertyInfo)?.SetValue(obj, value); }
		}

		public new Func<int, string> HoverText;

		public int minValue;
		public int maxValue;
		
		private MemberInfo member;
		private object obj;
		
		public UIStepSlider(MemberInfo value, int minValue = 0, int maxValue = 100, object obj = null, params int[] steps)
		{
			Height.Set(16f, 0f);

			member = value;
			this.obj = obj;

			this.minValue = minValue;
			this.maxValue = maxValue;

			for (int i = 0; i < steps.Length; i++)
			{
				UIButton button = new UIButton(Main.colorSliderTexture);
				button.Height.Pixels = 16f;
				button.Width.Pixels = 7f;
				button.VAlign = 0.5f;
				button.Id = steps[i].ToString();
				int i1 = i;
				button.OnClick += (a, b) => CurrentValue = steps[i1];
				Append(button);
			}
		}
		
		public override void RecalculateChildren()
		{
			CalculatedStyle dimensions = GetDimensions();
			UIElement[] elements = Elements.Where(x => x is UIButton).ToArray();
			for (int i = 0; i < elements.Length; i++)
			{
				elements[i].Left.Set(dimensions.Width / maxValue * int.Parse(elements[i].Id), 0f);
				elements[i].Recalculate();
			}
		}

		public override void MouseDown(UIMouseEvent evt)
		{
			if (evt.Target == this) dragging = true;
		}

		public override void MouseUp(UIMouseEvent evt)
		{
			dragging = false;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			spriteBatch.Draw(bar, dimensions.Position(), new Rectangle(0, 0, 5, 16), color);
			spriteBatch.Draw(bar, dimensions.Position() + new Vector2(5, 0), new Rectangle(6, 0, 1, 16), color, 0f, Vector2.Zero, new Vector2(dimensions.Width - 9, 1), SpriteEffects.None, 0f);
			spriteBatch.Draw(bar, dimensions.Position() + new Vector2(dimensions.Width - 4, 0), new Rectangle(bar.Width - 4, 0, 4, 16), color);

			if (dragging) CurrentValue = (int)MathHelper.Clamp((Main.mouseX - dimensions.X) * (maxValue / (dimensions.Width - 8f)), 0, maxValue);

			spriteBatch.Draw(slider, dimensions.Position() + new Vector2(SliderPos - slider.Width / 2f, dimensions.Height / 2f - slider.Height / 2f), Color.White);

			if (IsMouseHovering)
			{
				Main.LocalPlayer.showItemIcon = false;
				Main.ItemIconCacheUpdate(0);
				Main.mouseText = true;
				Utility.Utility.DrawMouseText(HoverText?.Invoke(CurrentValue) ?? CurrentValue.ToString());
			}
		}
	}
}