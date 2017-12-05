using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace TheOneLibrary.UI.Elements
{
	public class UIToggleButton : BaseElement
	{
		private Texture2D texture;

		public const float visibilityActive = 1f;
		public float visibilityInactive = 0.6f;

		public bool toggled = false;

		public UIToggleButton(Texture2D texture)
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
			CalculatedStyle dimensions = GetDimensions();

			float scale = Math.Min(dimensions.Width / texture.Width, dimensions.Height / texture.Height);
			spriteBatch.Draw(texture, new Vector2(dimensions.X, dimensions.Y), null, Color.White * (toggled ? visibilityActive : visibilityInactive), 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
		}
	}
}