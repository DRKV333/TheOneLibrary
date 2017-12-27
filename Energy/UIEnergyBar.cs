using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using TheOneLibrary.Energy.Energy;
using TheOneLibrary.Utility;

namespace TheOneLibrary.UI.Elements
{
	public class UIEnergyBar : BaseElement
	{
		public EnergyStorage energy;
		public long oldEnergy;

		private Color bgColor = new Color(73, 94, 171) * 0.9f;
		private Color barColor = Color.Red;

		protected void DrawBar(SpriteBatch spriteBatch, Color color)
		{
			CalculatedStyle dimensions = GetDimensions();

			spriteBatch.Draw(TheOneLibrary.corner, dimensions.Position(), color);
			spriteBatch.Draw(TheOneLibrary.corner, dimensions.Position() + new Vector2(dimensions.Width - 12, 12), null, color, MathHelper.Pi * 0.5f, new Vector2(12, 12), Vector2.One, SpriteEffects.None, 0f);
			spriteBatch.Draw(TheOneLibrary.corner, dimensions.Position() + new Vector2(12, dimensions.Height - 12), null, color, MathHelper.Pi * 1.5f, new Vector2(12, 12), Vector2.One, SpriteEffects.None, 0f);
			spriteBatch.Draw(TheOneLibrary.corner, dimensions.Position() + new Vector2(dimensions.Width - 12, dimensions.Height - 12), null, color, MathHelper.Pi * 1f, new Vector2(12, 12), Vector2.One, SpriteEffects.None, 0f);

			spriteBatch.Draw(TheOneLibrary.side, new Rectangle((int)(dimensions.X + 12), (int)dimensions.Y, (int)(dimensions.Width - 24), 12), color);
			spriteBatch.Draw(TheOneLibrary.side, new Rectangle((int)dimensions.X, (int)(dimensions.Y + dimensions.Height - 12), (int)(dimensions.Height - 24), 12), null, color, MathHelper.Pi * 1.5f, Vector2.Zero, SpriteEffects.None, 0f);
			spriteBatch.Draw(TheOneLibrary.side, new Rectangle((int)(dimensions.X + dimensions.Width), (int)(dimensions.Y + 12), (int)(dimensions.Height - 24), 12), null, color, MathHelper.Pi * 0.5f, Vector2.Zero, SpriteEffects.None, 0f);
			spriteBatch.Draw(TheOneLibrary.side, new Rectangle((int)(dimensions.X + dimensions.Width - 12), (int)(dimensions.Y + dimensions.Height), (int)(dimensions.Width - 24), 12), null, color, MathHelper.Pi, Vector2.Zero, SpriteEffects.None, 0f);

			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)(dimensions.X + 12), (int)(dimensions.Y + 12), (int)(dimensions.Width - 24), (int)(dimensions.Height - 24)), color);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			long delta = energy.GetEnergy() - oldEnergy;
			oldEnergy = energy.GetEnergy();
			float progress = energy.GetEnergy() / (float)energy.GetCapacity();

			DrawBar(spriteBatch, bgColor);

			spriteBatch.End();

			RasterizerState state = new RasterizerState {ScissorTestEnable = true};

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, state);

			Rectangle prevRect = spriteBatch.GraphicsDevice.ScissorRectangle;
			spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)dimensions.X, (int)(dimensions.Y + dimensions.Height - dimensions.Height * progress), (int)dimensions.Width, (int)(dimensions.Height * progress));

			DrawBar(spriteBatch, barColor);

			spriteBatch.GraphicsDevice.ScissorRectangle = prevRect;
			spriteBatch.End();
			spriteBatch.Begin();

			if (IsMouseHovering)
			{
				Main.LocalPlayer.showItemIcon = false;
				Main.ItemIconCacheUpdate(0);
				Main.instance.MouseTextHackZoom($"{energy}\n{(delta > 0 ? "+" : "") + delta.AsPower(true)}");
				Main.mouseText = true;
			}
		}
	}
}