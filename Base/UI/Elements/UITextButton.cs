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
		public UIPanel panel = new UIPanel();
		public UIText uiText;

		private float padding;

		public UITextButton(string text, float padding = 12)
		{
			this.padding = padding;

			panel.Width.Precent = 1;
			panel.Height.Precent = 1;
			panel.BackgroundColor = BaseUI.panelColor;
			panel.SetPadding(0);
			Append(panel);

			uiText = new UIText(text);
			uiText.Center();
			panel.Append(uiText);
		}

		public void SetText(string text)
		{
			uiText.SetText(text);
			Recalculate();
		}

		public void SetColor(Color? panelColor = null, Color? textColor = null)
		{
			panel.BackgroundColor = panelColor ?? BaseUI.panelColor;
			uiText.TextColor = textColor ?? Color.White;
		}

		public override void Recalculate()
		{
			base.Recalculate();

			CalculatedStyle dimensions = panel.GetDimensions();
			if (dimensions.Width > 0 && dimensions.Height > 0)
			{
				float textScale = Math.Min((dimensions.Width - padding) / Main.fontMouseText.MeasureString(uiText.Text).X, (dimensions.Height - padding) / Main.fontMouseText.MeasureString(uiText.Text).Y);
				uiText.SetText(uiText.Text, textScale, false);
			}
		}
	}
}