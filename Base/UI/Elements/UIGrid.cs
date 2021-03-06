﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using TheOneLibrary.Utility;

namespace TheOneLibrary.UI.Elements
{
	public class UIGrid : BaseElement
	{
		public delegate bool ElementSearchMethod(UIElement element);

		private class UIInnerList : BaseElement
		{
			public override bool ContainsPoint(Vector2 point) => true;

			protected override void DrawChildren(SpriteBatch spriteBatch)
			{
				Vector2 position = Parent.GetDimensions().Position();
				Vector2 dimensions = new Vector2(Parent.GetDimensions().Width, Parent.GetDimensions().Height);
				for (int i = 0; i < Elements.Count; i++)
				{
					UIElement current = Elements[i];
					Vector2 position2 = current.GetDimensions().Position();
					Vector2 dimensions2 = new Vector2(current.GetDimensions().Width, current.GetDimensions().Height);
					if (Collision.CheckAABBvAABBCollision(position, dimensions, position2, dimensions2)) current.Draw(spriteBatch);
				}
			}
		}

		public List<UIElement> items = new List<UIElement>();
		protected UIScrollbar scrollbar;
		internal UIElement innerList = new UIInnerList();
		private float innerListHeight;
		public float ListPadding = 5f;

		public int Count => items.Count;

		public int columns = 1;

		public UIGrid(int columns = 1)
		{
			this.columns = columns;
			innerList.Width.Set(0f, 1f);
			innerList.Height.Set(0f, 1f);
			Append(innerList);
		}

		public float GetTotalHeight()
		{
			return innerListHeight;
		}

		public void Goto(ElementSearchMethod searchMethod, bool center = false, bool bottom = false)
		{
			for (int i = 0; i < items.Count; i++)
			{
				if (searchMethod(items[i]))
				{
					scrollbar.ViewPosition = items[i].Top.Pixels;
					if (bottom) scrollbar.ViewPosition = items[i].Top.Pixels + items[i].GetOuterDimensions().Height;
					if (center) scrollbar.ViewPosition = items[i].Top.Pixels - GetInnerDimensions().Height / 2 + items[i].GetOuterDimensions().Height / 2;
					return;
				}
			}
		}

		public override void Update(GameTime gameTime)
		{
			for (int i = 0; i < items.Count; i++) items[i].Update(gameTime);

			base.Update(gameTime);
		}

		public virtual void Add(UIElement item)
		{
			item.Recalculate();
			items.Add(item);
			innerList.Append(item);
			UpdateOrder();
			innerList.Recalculate();
		}

		public virtual bool Remove(UIElement item)
		{
			innerList.RemoveChild(item);
			UpdateOrder();
			return items.Remove(item);
		}

		public virtual void Clear()
		{
			innerList.RemoveAllChildren();
			items.Clear();
		}

		public override void Recalculate()
		{
			base.Recalculate();

			innerList.Recalculate();
			UpdateScrollbar();
		}

		public override void ScrollWheel(UIScrollWheelEvent evt)
		{
			base.ScrollWheel(evt);
			if (scrollbar != null) scrollbar.ViewPosition -= evt.ScrollWheelValue;
		}

		public override void RecalculateChildren()
		{
			float top = 0f;
			float left = 0f;
			
			for (int i = 0; i < items.Count; i++)
			{
				UIElement item = items[i];
				CalculatedStyle dimensions = item.GetOuterDimensions();
				if (i % columns == columns - 1 /*&& i < items.Count - 1*/)
				{
					top += dimensions.Height + ListPadding;
					left = 0;
				}
				else left += dimensions.Width + ListPadding;
				item.Top.Set(top, 0f);
				item.Left.Set(left, 0f);
				item.Recalculate();
			}

			innerListHeight = items.Select((x, i) => i % columns == columns - 1 ? x.GetOuterDimensions().Height + ListPadding : 0f).Sum();
		}

		private void UpdateScrollbar()
		{
			scrollbar?.SetView(GetInnerDimensions().Height, innerListHeight);
		}

		public void SetScrollbar(UIScrollbar scrollbar)
		{
			this.scrollbar = scrollbar;
			UpdateScrollbar();
		}

		public void RemoveScrollbar()
		{
			scrollbar = null;
		}

		public void UpdateOrder()
		{
			items.Sort(SortMethod);
			UpdateScrollbar();
		}

		public int SortMethod(UIElement item1, UIElement item2) => item1.CompareTo(item2);

		public override List<SnapPoint> GetSnapPoints()
		{
			List<SnapPoint> list = new List<SnapPoint>();
			SnapPoint item;
			if (GetSnapPoint(out item)) list.Add(item);
			foreach (UIElement current in items) list.AddRange(current.GetSnapPoints());
			return list;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			RasterizerState state = new RasterizerState { ScissorTestEnable = true };
			Rectangle prevRect = spriteBatch.GraphicsDevice.ScissorRectangle;
			spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle.Intersect(GetClippingRectangle(spriteBatch), spriteBatch.GraphicsDevice.ScissorRectangle);
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, state, null, Main.UIScaleMatrix);

			DrawSelf(spriteBatch);
			RecalculateChildren();
			typeof(UIInnerList).InvokeMethod<object>("DrawChildren", new object[] { spriteBatch }, innerList);

			spriteBatch.End();
			spriteBatch.GraphicsDevice.ScissorRectangle = prevRect;
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, null, null, Main.UIScaleMatrix);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (scrollbar != null) innerList.Top.Set(-scrollbar.GetValue(), 0f);
			Recalculate();
		}
	}
}