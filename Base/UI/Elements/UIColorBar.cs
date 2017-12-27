using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace TheOneLibrary.UI.Elements
{
	public class UIColorBar : BaseElement
	{
		private readonly Texture2D gradTexture;

		public UIColorBar()
		{
			gradTexture = CreateGrad(Main.graphics.GraphicsDevice, 6 * 255, 255);
		}

		public Texture2D CreateGrad(GraphicsDevice importedGraphicsDevice, int width, int height)
		{
			Color color = new Color(255, 0, 0);
			const int brightness = 255;

			Texture2D texture = new Texture2D(importedGraphicsDevice, width, height);

			Color[,] data = new Color[width, height];

			for (int x = 0; x < data.GetLength(0); x++) for (int y = 0; y < data.GetLength(1); y++) data[x, y] = Color.Transparent;

			for (int x = 0; x < 6 * 255; x++)
			{
				for (int y = 0; y < 255; y++)
				{
					if (color.R == brightness && color.G < brightness && color.B == 0) color.G += 1;
					else if (color.R > 0 && color.G == brightness && color.B == 0) color.R -= 1;
					else if (color.R == 0 && color.G == brightness && color.B < brightness) color.B += 1;
					else if (color.R == 0 && color.G > 0 && color.B == brightness) color.G -= 1;
					else if (color.R < brightness && color.G == 0 && color.B == brightness) color.R += 1;
					else if (color.R == brightness && color.G == 0 && color.B > 0) color.B -= 1;

					data[x, y] = color;
				}
			}

			IList<Color> data1 = new List<Color>();

			for (int x = 0; x < data.GetLength(0); x++) for (int y = 0; y < data.GetLength(1); y++) data1.Add(data[x, y]);
			texture.SetData(data1.ToArray());
			return texture;
		}

		public Color GetColor()
		{
			CalculatedStyle dimensions = GetDimensions();

			int x = (int)(Main.mouseX - dimensions.X);
			int y = (int)(Main.mouseY - dimensions.Y);

			Color[] data = new Color[gradTexture.Width * gradTexture.Height];
			gradTexture.GetData(data);

			Vector2 scale = new Vector2(dimensions.Width / gradTexture.Width, dimensions.Height / gradTexture.Height);

			return data[(int)(y / scale.Y) * gradTexture.Width + (int)(x / scale.X)];
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			spriteBatch.Draw(Main.magicPixel, dimensions.Position(), null, Color.Black, 0f, Vector2.Zero, new Vector2(dimensions.Width, dimensions.Height / 1000f), SpriteEffects.None, 0f);
			spriteBatch.Draw(gradTexture, dimensions.Position() + new Vector2(2, 2), null, Color.White, 0f, Vector2.Zero, new Vector2((dimensions.Width - 4f) / gradTexture.Width, (dimensions.Height - 4f) / gradTexture.Height), SpriteEffects.None, 0f);
		}
	}
}