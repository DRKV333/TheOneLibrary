using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.OS;
using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.UI.Chat;
using TheOneLibrary.Utility;

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

		public override void RightClick(UIMouseEvent evt)
		{
			currentString = "";
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

		// arrow keys, delete, ctrl+back/del
		public static string GetInputText(string oldString)
		{
			if (!Main.hasFocus)
			{
				return oldString;
			}
			Main.inputTextEnter = false;
			Main.inputTextEscape = false;
			string text = oldString;
			string text2 = "";
			if (text == null)
			{
				text = "";
			}
			bool flag = false;
			if (Main.inputText.IsKeyDown(Keys.LeftControl) || Main.inputText.IsKeyDown(Keys.RightControl))
			{
				if (Main.inputText.IsKeyDown(Keys.X) && !Main.oldInputText.IsKeyDown(Keys.X))
				{
					Platform.Current.Clipboard = oldString;
					text = "";
				}
				else if (Main.inputText.IsKeyDown(Keys.C) && !Main.oldInputText.IsKeyDown(Keys.C) || Main.inputText.IsKeyDown(Keys.Insert) && !Main.oldInputText.IsKeyDown(Keys.Insert))
				{
					Platform.Current.Clipboard = oldString;
				}
				else if (Main.inputText.IsKeyDown(Keys.V) && !Main.oldInputText.IsKeyDown(Keys.V))
				{
					text2 += Platform.Current.Clipboard;
				}
			}
			else
			{
				if (Main.inputText.PressingShift())
				{
					if (Main.inputText.IsKeyDown(Keys.Delete) && !Main.oldInputText.IsKeyDown(Keys.Delete))
					{
						Platform.Current.Clipboard = oldString;
						text = "";
					}
					if (Main.inputText.IsKeyDown(Keys.Insert) && !Main.oldInputText.IsKeyDown(Keys.Insert))
					{
						string text3 = Platform.Current.Clipboard;
						for (int i = 0; i < text3.Length; i++)
						{
							if (text3[i] < ' ' || text3[i] == '\u007f')
							{
								text3 = text3.Replace(string.Concat(text3[i--]), "");
							}
						}
						text2 += text3;
					}
				}
				for (int j = 0; j < Main.keyCount; j++)
				{
					int num = Main.keyInt[j];
					string str = Main.keyString[j];
					if (num == (int)Keys.Enter) Main.inputTextEnter = true;
					else if (num == (int)Keys.Escape) Main.inputTextEscape = true;
					else if (num >= (int)Keys.Space && num != (int)Keys.F16) text2 += str;
				}
			}
			Main.keyCount = 0;
			text += text2;
			Main.oldInputText = Main.inputText;
			Main.inputText = Keyboard.GetState();
			Keys[] pressedKeys = Main.inputText.GetPressedKeys();
			Keys[] pressedKeysOld = Main.oldInputText.GetPressedKeys();
			if (Main.inputText.IsKeyDown(Keys.Back) && Main.oldInputText.IsKeyDown(Keys.Back))
			{
				if (backSpaceCount == 0)
				{
					backSpaceCount = 7;
					flag = true;
				}
				backSpaceCount--;
			}
			else
			{
				backSpaceCount = 15;
			}
			for (int k = 0; k < pressedKeys.Length; k++)
			{
				bool keysChanged = true;
				for (int l = 0; l < pressedKeysOld.Length; l++)
				{
					if (pressedKeys[k] == pressedKeysOld[l])
					{
						keysChanged = false;
					}
				}
				string a = string.Concat(pressedKeys[k]);
				if (a == "Back" && (keysChanged || flag) && text.Length > 0)
				{
					TextSnippet[] array = ChatManager.ParseMessage(text, Color.White).ToArray();
					text = array[array.Length - 1].DeleteWhole ? text.Substring(0, text.Length - array[array.Length - 1].TextOriginal.Length) : text.Substring(0, text.Length - 1);
				}
			}
			return text;
		}
		
		public static int backSpaceCount
		{
			get
			{
				return typeof(Main).GetFieldValue<int>("backSpaceCount");
			}
			set { typeof(Main).SetFieldValue("backSpaceCount", value); }
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			string displayString = currentString;

			if (focused)
			{
				PlayerInput.WritingText = true;
				Main.instance.HandleIME();
				
				string newString = GetInputText(currentString);
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
			if (Keys.Enter.IsKeyDown() || Keys.Escape.IsKeyDown()) Unfocus();
		}
	}
}