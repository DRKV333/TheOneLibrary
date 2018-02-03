using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.UI;

namespace TheOneLibrary.UI.Elements
{
	public class UICycleButton : BaseElement
	{
		public IEnumerable<Texture2D> textures;
		public int index;

		public UICycleButton(IEnumerable<Texture2D> texture)
		{
			textures = texture;
		}

		public UICycleButton(params Texture2D[] texture)
		{
			textures = texture;
		}

		public override void Click(UIMouseEvent evt)
		{
			base.Click(evt);
			index = index.NextEnum();
			//index++;
			//if (index > textures.Count() - 1) index = 0;
		}

		public override void RightClick(UIMouseEvent evt)
		{
			base.RightClick(evt);

			index = index.PreviousEnum();
			//index--;
			//if (index < 0) index = textures.Count() - 1;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			spriteBatch.Draw(textures.ElementAt(index), new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height), Color.White);
		}
	}
}