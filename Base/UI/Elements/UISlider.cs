﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.UI;
using TheOneLibrary.Utility;

namespace TheOneLibrary.UI.Elements
{
	public class UISlider : BaseElement
	{
		private static Color color = new Color(43, 56, 101, 200);

		private Texture2D bar = Main.colorBarTexture;
		private Texture2D slider = Main.colorSliderTexture;

		public new Func<double, string> HoverText;

		private bool dragging;
		public float SliderPos
		{
			get { return (float)(GetDimensions().Width / maxValue * CurrentValue); }
		}

		public double CurrentValue
		{
			get { return member.GetValue<double>(obj); }
			set { (member as FieldInfo)?.SetValue(obj, value); (member as PropertyInfo)?.SetValue(obj, value); }
		}

		public double minValue;
		public double maxValue;

		private MemberInfo member;
		private object obj;

		public UISlider(MemberInfo value, double minValue = 0, double maxValue = 100, object obj = null)
		{
			Height.Set(16f, 0f);

			member = value;
			this.obj = obj;

			this.minValue = minValue;
			this.maxValue = maxValue;
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

			if (dragging) CurrentValue = (int)Utility.Utility.Clamp((Main.mouseX - dimensions.X) * (maxValue / (dimensions.Width - 8f)), 0, maxValue);

			spriteBatch.Draw(slider, dimensions.Position() + new Vector2(SliderPos - slider.Width / 2f, dimensions.Height / 2f - slider.Height / 2f), Color.White);

			if (IsMouseHovering) Utility.Utility.DrawMouseText(HoverText?.Invoke(CurrentValue) ?? CurrentValue.ToString());
		}
	}
}