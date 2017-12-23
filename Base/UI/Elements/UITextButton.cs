using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using TheOneLibrary.Base.UI;
using TheOneLibrary.Utility;

namespace TheOneLibrary.UI.Elements
{
	public class UITextButton : BaseElement
	{
		public string Text;

		private UIPanel panel = new UIPanel();
		private UIText uiText;

		private float padding;

		public UITextButton(string text, float padding = 12)
		{
			Text = text;
			this.padding = padding;

			panel.Width.Precent = 1;
			panel.Height.Precent = 1;
			panel.BackgroundColor = BaseUI.panelColor;
			panel.SetPadding(0);
			Append(panel);

			uiText = new UIText(Text);
			uiText.Center();
			panel.Append(uiText);
		}

		public void SetColor(Color? panelColor = null, Color? textColor = null)
		{
			panel.BackgroundColor = panelColor ?? BaseUI.panelColor;
			uiText.TextColor = textColor ?? Color.White;
		}

		public void RescaleText()
		{
			CalculatedStyle dimensions = panel.GetDimensions();
			if (dimensions.Width > 0 && dimensions.Height > 0)
			{
				float textScale = Math.Min((dimensions.Width - padding) / Main.fontMouseText.MeasureString(Text).X, (dimensions.Height - padding) / Main.fontMouseText.MeasureString(Text).Y);
				uiText.SetText(Text, textScale, false);
			}
		}
	}
}