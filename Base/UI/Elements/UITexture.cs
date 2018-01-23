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
			Vector2 scale = new Vector2(dimensions.Width / texture.Width, dimensions.Height / texture.Height);
			spriteBatch.Draw(texture, dimensions.Position(), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
		}
	}
}