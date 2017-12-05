using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace TheOneLibrary.UI.Elements
{
	public class UITextInput : BaseElement
	{
		public bool focused;
		public string hintText;

		public string currentString = "";
		private int textBlinkerCount;
		private int textBlinkerState;
		private int textBlinkerPos;
		public int maxTextLenght = -1;
		public string defaultText = "Text";
		public bool backgroundPanel;

		public event Action OnTextChange;

		public UITextInput(string hintText = "")
		{
			this.hintText = hintText;
		}

		public override void Click(UIMouseEvent evt)
		{
			if (focused) Unfocus();
			else Focus();

			base.Click(evt);
		}

		public void Focus()
		{
			focused = true;
			Main.clrInput();

			textBlinkerPos = currentString.Length;

			Main.LocalPlayer.showItemIcon = false;
			Main.ItemIconCacheUpdate(0);

			Main.blockInput = true;
			Main.editSign = true;
			Main.chatRelease = false;
		}

		public void Unfocus()
		{
			focused = false;
			Main.blockInput = false;
			Main.editSign = false;
			Main.chatRelease = true;
		}

		public string GetText() => currentString;

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			string displayString = currentString;

			if (focused)
			{
				PlayerInput.WritingText = true;
				Main.instance.HandleIME();

				string newString = Main.GetInputText(currentString);
				if (!newString.Equals(currentString))
				{
					currentString = newString;
					OnTextChange?.Invoke();
				}
				else currentString = newString;


				if (++textBlinkerCount >= 20)
				{
					textBlinkerState = (textBlinkerState + 1) % 2;
					textBlinkerCount = 0;
				}
				if (textBlinkerState == 1) displayString = displayString + "|";
			}

			CalculatedStyle space = GetDimensions();

			Utils.DrawBorderString(spriteBatch, displayString, new Vector2(space.X, space.Y + YOffset(displayString)), Color.White);
			if (displayString.Length == 0 && !focused) Utils.DrawBorderString(spriteBatch, hintText, new Vector2(space.X, space.Y + YOffset(hintText)), Color.Gray);
		}

		public float YOffset(string text) => backgroundPanel ? Parent.GetDimensions().Height / 2f - Main.fontMouseText.MeasureString(text).Y / 2f : 0f;

		public override void Update(GameTime gameTime)
		{
			if (Main.keyState.IsKeyDown(Keys.Enter) || Main.keyState.IsKeyDown(Keys.Escape)) Unfocus();
		}
	}
}