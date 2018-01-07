using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using TheOneLibrary.Base;
using TheOneLibrary.Storage;

namespace TheOneLibrary.UI.Elements
{
	public class UIContainerSlot : UIElement
	{
		public Texture2D backgroundTexture = Main.inventoryBackTexture;

		public IContainer Container;
		public int slot;

		/// <summary>
		///     Current item, mouse item.
		/// </summary>
		public event Func<Item, Item, bool> CanInteract;

		public event Action OnInteract;

		public int maxStack;

		public Item Item
		{
			get { return Container.GetItems()[slot]; }
			set { Container.GetItems()[slot] = value; }
		}

		public UIContainerSlot(IContainer container, int slot = 0)
		{
			Width.Pixels = 40;
			Height.Pixels = 40;

			this.slot = slot;
			Container = container;
		}

		public override void Click(UIMouseEvent evt)
		{
			base.Click(evt);

			if (CanInteract == null || CanInteract != null && CanInteract.Invoke(Item, Main.mouseItem))
			{
				Main.PlaySound(SoundID.MenuTick);
				OnInteract?.Invoke();

				if (Item.IsAir)
				{
					if (!Main.mouseItem.IsAir)
					{
						Item = Main.mouseItem.Clone();
						if (maxStack > 0) Item.maxStack = maxStack;
						int count = Math.Min(Main.mouseItem.stack, Item.maxStack);
						Item.stack = count;
						Main.mouseItem.stack -= count;
						if (Main.mouseItem.stack <= 0) Main.mouseItem.TurnToAir();
					}
				}
				else
				{
					if (Main.mouseItem.IsAir)
					{
						Main.mouseItem = Item.Clone();
						Item temp = new Item();
						temp.SetDefaults(Item.type);
						Main.mouseItem.maxStack = temp.maxStack;
						int count = Math.Min(Item.stack, Main.mouseItem.maxStack);
						Main.mouseItem.stack = count;
						Item.stack -= count;
						if (Item.stack <= 0) Item.TurnToAir();
					}
					else if (!Main.mouseItem.IsAir && Main.mouseItem.type == Item.type)
					{
						if (maxStack > 0) Item.maxStack = maxStack;
						int count = Math.Min(Main.mouseItem.stack, Item.maxStack - Item.stack);
						Main.mouseItem.stack -= count;
						Item.stack += count;
						if (Main.mouseItem.stack <= 0) Main.mouseItem.TurnToAir();
					}
				}
			}

			(Container as IContainerTile)?.GetTileEntity().SendUpdate();
			if (Container is IContainerItem) NetHelper.SyncEntity(MessageID.SyncItem, slot);
		}

		public override void RightClick(UIMouseEvent evt)
		{
			base.RightClick(evt);

			if (CanInteract == null || CanInteract != null && CanInteract.Invoke(Item, Main.mouseItem))
			{
				Main.PlaySound(SoundID.MenuTick);
				OnInteract?.Invoke();

				Item item = Item;
				ItemSlot.RightClick(ref item);
				Item = item;
			}

			(Container as IContainerTile)?.GetTileEntity().SendUpdate();
			if (Container is IContainerItem) NetHelper.SyncEntity(MessageID.SyncItem, slot);
		}

		public override int CompareTo(object obj) => slot.CompareTo(((UIContainerSlot)obj).slot);

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetInnerDimensions();

			float scale = Math.Min(dimensions.Width / backgroundTexture.Width, dimensions.Height / backgroundTexture.Height);
			spriteBatch.Draw(!Item.IsAir && Item.favorited ? Main.inventoryBack10Texture : backgroundTexture, dimensions.Position(), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

			if (!Item.IsAir)
			{
				Texture2D itemTexture = Main.itemTexture[Item.type];
				Rectangle rect = Main.itemAnimations[Item.type] != null ? Main.itemAnimations[Item.type].GetFrame(itemTexture) : itemTexture.Frame();
				Color newColor = Color.White;
				float pulseScale = 1f;
				ItemSlot.GetItemLight(ref newColor, ref pulseScale, Item);
				int height = rect.Height;
				int width = rect.Width;
				float drawScale = 1f;
				float availableWidth = 32;
				if (width > availableWidth || height > availableWidth)
				{
					if (width > height) drawScale = availableWidth / width;
					else drawScale = availableWidth / height;
				}
				drawScale *= scale;
				Vector2 vector = backgroundTexture.Size() * scale;
				Vector2 position2 = dimensions.Position() + vector / 2f - rect.Size() * drawScale / 2f;
				Vector2 origin = rect.Size() * (pulseScale / 2f - 0.5f);

				if (ItemLoader.PreDrawInInventory(Item, spriteBatch, position2, rect, Item.GetAlpha(newColor), Item.GetColor(Color.White), origin, drawScale * pulseScale))
				{
					spriteBatch.Draw(itemTexture, position2, rect, Item.GetAlpha(newColor), 0f, origin, drawScale * pulseScale, SpriteEffects.None, 0f);
					if (Item.color != Color.Transparent) spriteBatch.Draw(itemTexture, position2, rect, Item.GetColor(Color.White), 0f, origin, drawScale * pulseScale, SpriteEffects.None, 0f);
				}
				ItemLoader.PostDrawInInventory(Item, spriteBatch, position2, rect, Item.GetAlpha(newColor), Item.GetColor(Color.White), origin, drawScale * pulseScale);
				if (ItemID.Sets.TrapSigned[Item.type]) spriteBatch.Draw(Main.wireTexture, dimensions.Position() + new Vector2(40f, 40f) * scale, new Rectangle(4, 58, 8, 8), Color.White, 0f, new Vector2(4f), 1f, SpriteEffects.None, 0f);
				if (Item.stack > 1) ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, Item.stack.ToString(), dimensions.Position() + new Vector2(10f, 26f) * scale, Color.White, 0f, Vector2.Zero, new Vector2(scale), -1f, scale);

				if (IsMouseHovering)
				{
					Main.LocalPlayer.showItemIcon = false;
					Main.ItemIconCacheUpdate(0);
					Main.HoverItem = Item.Clone();
					Main.hoverItemName = Main.HoverItem.Name;
				}
			}
		}
	}
}