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

			spriteBatch.Draw(Main.magicPixel, dimensions.Position(), null, Color.Black, 0f, Vector2.Zero, new Vector2(dimensions.Width, dimensions.Height / 1000f), SpriteEffects.None, 0f);
			spriteBatch.Draw(Main.magicPixel, dimensions.Position() + new Vector2(2), null, color, 0f, Vector2.Zero, new Vector2(dimensions.Width - 4f, (dimensions.Height - 4f) / 1000f), SpriteEffects.None, 0f);
		}
	}
}