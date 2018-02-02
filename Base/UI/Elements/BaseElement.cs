using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.UI;

namespace TheOneLibrary.UI.Elements
{
	public class BaseElement : UIElement
	{
		public bool visible = true;
		public Func<string> HoverText;

		public virtual void LeftClickContinuous()
		{

		}

		public virtual void RightClickContinuous()
		{

		}

		public override void Update(GameTime gameTime)
		{
			if (Main.hasFocus && IsMouseHovering)
			{
				if (Main.mouseLeft) LeftClickContinuous();
				if (Main.mouseRight) RightClickContinuous();
			}
		}

		public static RasterizerState OverflowHiddenState = new RasterizerState
		{
			CullMode = CullMode.None,
			ScissorTestEnable = true
		};

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (visible)
			{
				bool overflowHidden = OverflowHidden;
				bool useImmediateMode = _useImmediateMode;
				RasterizerState rasterizerState = spriteBatch.GraphicsDevice.RasterizerState;
				Rectangle scissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
				SamplerState anisotropicClamp = SamplerState.AnisotropicClamp;
				if (useImmediateMode)
				{
					spriteBatch.End();
					spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, OverflowHiddenState, null, Main.UIScaleMatrix);
					DrawSelf(spriteBatch);
					spriteBatch.End();
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, OverflowHiddenState, null, Main.UIScaleMatrix);
				}
				else DrawSelf(spriteBatch);

				if (IsMouseHovering && HoverText != null) Utility.Utility.DrawMouseText(HoverText.Invoke());

				if (overflowHidden)
				{
					spriteBatch.End();
					Rectangle clippingRectangle = GetClippingRectangle(spriteBatch);
					Rectangle adjustedClippingRectangle = Rectangle.Intersect(clippingRectangle, spriteBatch.GraphicsDevice.ScissorRectangle);
					spriteBatch.GraphicsDevice.ScissorRectangle = adjustedClippingRectangle;
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, OverflowHiddenState, null, Main.UIScaleMatrix);
				}
				DrawChildren(spriteBatch);
				if (overflowHidden)
				{
					rasterizerState = spriteBatch.GraphicsDevice.RasterizerState;
					spriteBatch.End();
					spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, rasterizerState, null, Main.UIScaleMatrix);
				}
			}
		}
	}
}