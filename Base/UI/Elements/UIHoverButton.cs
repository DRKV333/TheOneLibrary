using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using TheOneLibrary.UI.Elements;

namespace TheOneLibrary.Base.UI.Elements
{
	public class UIHoverButton : BaseElement
	{
		public Texture2D[] textures;

		public UIHoverButton(params Texture2D[] texture)
		{
			if (texture.Length != 2) throw new Exception("UIHoverButton supports only 2 textures!");

			textures = texture;
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);

			Main.PlaySound(SoundID.MenuTick);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);

			Main.PlaySound(SoundID.MenuTick);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			spriteBatch.Draw(IsMouseHovering ? textures[1] : textures[0], new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height), Color.White);
		}
	}
}