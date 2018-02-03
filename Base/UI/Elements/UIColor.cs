using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace TheOneLibrary.UI.Elements
{
	public class UIColor : UIElement
	{
		public Color color;

		public UIColor(Color color)
		{
			this.color = color;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height), null, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0f);
			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)dimensions.X + 2, (int)dimensions.Y + 2, (int)dimensions.Width - 4, (int)dimensions.Height - 4), null, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
		}
	}
}