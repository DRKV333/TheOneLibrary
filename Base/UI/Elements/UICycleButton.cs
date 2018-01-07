using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace TheOneLibrary.UI.Elements
{
	public class UICycleButton : BaseElement
	{
		public Texture2D[] textures;
		public int index;

		public string HoverText;

		public UICycleButton(params Texture2D[] texture)
		{
			textures = texture;
		}

		public override void Click(UIMouseEvent evt)
		{
			base.Click(evt);

			index++;
			if (index > textures.Length - 1) index = 0;
		}

		public override void RightClick(UIMouseEvent evt)
		{
			base.RightClick(evt);

			index--;
			if (index < 0) index = textures.Length - 1;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			spriteBatch.Draw(textures[index], new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height), Color.White);

			if (IsMouseHovering && !string.IsNullOrWhiteSpace(HoverText))
			{
				Main.LocalPlayer.showItemIcon = false;
				Main.ItemIconCacheUpdate(0);
				Main.instance.MouseTextHackZoom(HoverText);
				Main.mouseText = true;
			}
		}
	}
}