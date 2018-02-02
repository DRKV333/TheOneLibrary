using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace TheOneLibrary.UI.Elements
{
	public class UIButton : BaseElement
	{
		public Texture2D texture;

		public UIButton(Texture2D texture)
		{
			this.texture = texture;
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
			if (texture != null)
			{
				CalculatedStyle dimensions = GetDimensions();
				spriteBatch.Draw(texture, new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height), Color.White);

				base.DrawSelf(spriteBatch);
			}
		}
	}
}