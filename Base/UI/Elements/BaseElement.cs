using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace TheOneLibrary.UI.Elements
{
	public class BaseElement : UIElement
	{
		public bool visible = true;

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (visible) base.DrawSelf(spriteBatch);
		}
	}
}