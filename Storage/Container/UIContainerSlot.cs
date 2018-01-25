using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using TheOneLibrary.Base;
using TheOneLibrary.Storage;
using TheOneLibrary.Utility;

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

			if (CanInteract?.Invoke(Item, Main.mouseItem) ?? true)
			{
				Item.newAndShiny = false;
				Player player = Main.LocalPlayer;

				if (ItemSlot.ShiftInUse)
				{
					Main.NewText("move to inventory");
					return;
				}

				Item temp = Item;
				Utils.Swap(ref temp, ref Main.mouseItem);
				Item = temp;

				if (Item.stack > 0) AchievementsHelper.NotifyItemPickup(player, Item);
				if (Item.type == 0 || Item.stack < 1) Item = new Item();
				if (Main.mouseItem.IsTheSameAs(Item))
				{
					Utils.Swap(ref Item.favorited, ref Main.mouseItem.favorited);
					if (Item.stack != Item.maxStack && Main.mouseItem.stack != Main.mouseItem.maxStack)
					{
						if (Main.mouseItem.stack + Item.stack <= Main.mouseItem.maxStack)
						{
							Item.stack += Main.mouseItem.stack;
							Main.mouseItem.stack = 0;
						}
						else
						{
							int delta = Main.mouseItem.maxStack - Item.stack;
							Item.stack += delta;
							Main.mouseItem.stack -= delta;
						}
					}
				}
				if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1) Main.mouseItem = new Item();
				if (Main.mouseItem.type > 0 || Item.type > 0)
				{
					Terraria.Recipe.FindRecipes();
					Main.PlaySound(7);
				}

				OnInteract?.Invoke();
			}

			SendUpdate();
		}

		public void RightMouseDownCont()
		{
			if (CanInteract?.Invoke(Item, Main.mouseItem) ?? true)
			{
				OnInteract?.Invoke();

				Player player = Main.LocalPlayer;
				Item.newAndShiny = false;

				if (player.itemAnimation > 0) return;

				bool specialClick = false;
				if (ItemLoader.CanRightClick(Item))
				{
					ItemLoader.RightClick(Item, player);
					specialClick = true;
					SendUpdate();
				}

				if (specialClick) return;

				if (Item.maxStack == 1 && Main.mouseRight && Main.mouseRightRelease)
				{
					if (Item.dye > 0)
					{
						object[] param = { Item, false };
						Item = typeof(ItemSlot).InvokeMethod<Item>("DyeSwap", param);

						if ((bool)param[1])
						{
							Main.EquipPageSelected = 0;
							AchievementsHelper.HandleOnEquip(player, Item, 12);
						}
					}
					else if (Main.projHook[Item.shoot])
					{
						object[] param = { Item, player.miscEquips, 4, false };
						Item = typeof(ItemSlot).InvokeMethod<Item>("EquipSwap", param);

						if ((bool)param[3])
						{
							Main.EquipPageSelected = 2;
							AchievementsHelper.HandleOnEquip(player, Item, 16);
						}
					}
					else if (Item.mountType != -1 && !MountID.Sets.Cart[Item.mountType])
					{
						object[] param = { Item, player.miscEquips, 3, false };
						Item = typeof(ItemSlot).InvokeMethod<Item>("EquipSwap", param);

						if ((bool)param[3])
						{
							Main.EquipPageSelected = 2;
							AchievementsHelper.HandleOnEquip(player, Item, 17);
						}
					}
					else if (Item.mountType != -1 && MountID.Sets.Cart[Item.mountType])
					{
						object[] param = { Item, player.miscEquips, 2, false };
						Item = typeof(ItemSlot).InvokeMethod<Item>("EquipSwap", param);

						if ((bool)param[3])
						{
							Main.EquipPageSelected = 2;
						}
					}
					else if (Item.buffType > 0 && Main.lightPet[Item.buffType])
					{
						object[] param = { Item, player.miscEquips, 1, false };
						Item = typeof(ItemSlot).InvokeMethod<Item>("EquipSwap", param);

						if ((bool)param[3])
						{
							Main.EquipPageSelected = 2;
						}
					}
					else if (Item.buffType > 0 && Main.vanityPet[Item.buffType])
					{
						object[] param = { Item, player.miscEquips, 0, false };
						Item = typeof(ItemSlot).InvokeMethod<Item>("EquipSwap", param);

						if ((bool)param[3])
						{
							Main.EquipPageSelected = 2;
						}
					}
					else
					{
						Item item1 = Item;
						object[] param = { Item, false };
						Item = typeof(ItemSlot).InvokeMethod<Item>("ArmorSwap", param);

						if ((bool)param[1])
						{
							Main.EquipPageSelected = 0;
							AchievementsHelper.HandleOnEquip(player, item1, item1.accessory ? 10 : 8);
						}
					}
					Terraria.Recipe.FindRecipes();
					
					return;
				}

				if (Main.stackSplit <= 1 && Main.mouseRight)
				{
					if (Item.maxStack > 1 && (Main.mouseItem.IsTheSameAs(Item) || Main.mouseItem.type == 0) && (Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0))
					{
						if (Main.mouseItem.type == 0)
						{
							Main.mouseItem = Item.Clone();
							Main.mouseItem.stack = 0;
							if (Item.favorited && Item.maxStack == 1) Main.mouseItem.favorited = true;
							Main.mouseItem.favorited = false;
						}
						Main.mouseItem.stack++;
						Item.stack--;
						if (Item.stack <= 0) Item = new Item();

						Terraria.Recipe.FindRecipes();

						Main.soundInstanceMenuTick.Stop();
						Main.soundInstanceMenuTick = Main.soundMenuTick.CreateInstance();
						Main.PlaySound(12);

						Main.stackSplit = Main.stackSplit == 0 ? 15 : Main.stackDelay;
					}

					SendUpdate();
				}
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (Main.mouseRight && Main.hasFocus && IsMouseHovering) RightMouseDownCont();
		}

		public void SendUpdate()
		{
			(Container as IContainerTile)?.GetTileEntity().SendUpdate();
			if (Container is IContainerItem) NetUtility.SyncItem((Container as IContainerItem).GetItem().item);
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