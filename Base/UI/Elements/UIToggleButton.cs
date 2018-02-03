using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
			
			spriteBatch.Draw(texture, new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height), null, Color.White * (toggled ? visibilityActive : visibilityInactive), 0f, Vector2.Zero, SpriteEffects.None, 0f);
		}
	}
}