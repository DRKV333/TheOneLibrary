using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.UI;
using TheOneLibrary.Base.UI;
using TheOneLibrary.Utility;

namespace TheOneLibrary.UI.Elements
{
	public class UITextButton : BaseElement
	{
		public Color PanelColor = BaseUI.panelColor;
		public Color TextColor = Color.White;
		public string text;
		public float padding;

		public UITextButton(string text, float padding = 8f)
		{
			this.text = text;
			this.padding = padding;
		}
		
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			spriteBatch.DrawPanel(dimensions, TheOneLibrary.backgroundTexture, PanelColor);
			spriteBatch.DrawPanel(dimensions, TheOneLibrary.borderTexture, Color.Black);

			float scale = Math.Min((dimensions.Width - padding * 2f) / text.Measure(Main.fontMouseText).X, (dimensions.Height - padding * 2f) / text.Measure(Main.fontMouseText).Y);
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, text, dimensions.Width / 2f, dimensions.Height / 2f, TextColor, Color.Black, text.Measure(Main.fontMouseText) * scale, scale);
		}
	}
}