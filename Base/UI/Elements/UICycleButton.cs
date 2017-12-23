using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace TheOneLibrary.UI.Elements
{
	public class UICycleButton : BaseElement
	{
		public Texture2D texture;
		public int width;
		public int height;
		public int frameX;
		public int frameY;

		public float opacity = 1f;
		public Color color = Color.White;

		public string Text;

		public UICycleButton(Texture2D texture, int width, int height)
		{
			this.texture = texture;
			this.width = width;
			this.height = height;
		}

		public void SetFrame(int frameX = 0, int frameY = 0)
		{
			this.frameX = frameX;
			this.frameY = frameY;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			spriteBatch.Draw(texture, new Rectangle((int) dimensions.X, (int) dimensions.Y, (int) dimensions.Width, (int) dimensions.Height), new Rectangle(width * frameX, height * frameY, width, height), color * opacity);

			if (IsMouseHovering && !string.IsNullOrWhiteSpace(Text))
			{
				Main.LocalPlayer.showItemIcon = false;
				Main.ItemIconCacheUpdate(0);
				Main.instance.MouseTextHackZoom(Text);
				Main.mouseText = true;
			}
		}
	}
}