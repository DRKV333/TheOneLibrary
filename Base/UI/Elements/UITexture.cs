using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using TheOneLibrary.UI.Elements;

namespace TheOneLibrary.Base.UI.Elements
{
	public class UITexture : BaseElement
	{
		public Texture2D texture;

		public UITexture(Texture2D texture)
		{
			this.texture = texture;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			spriteBatch.Draw(texture, new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height), new Rectangle(), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
		}
	}
}