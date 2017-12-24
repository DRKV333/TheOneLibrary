using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheOneLibrary.Base.Items;
using TheOneLibrary.Utility;

namespace TheOneLibrary.Fluid
{
	public class Bucket : BaseItem
	{
		public override string Texture => TheOneLibrary.TexturePath + "Fluid/Bucket";

		public override bool CloneNewInstances => false;

		public override ModItem Clone(Item item)
		{
			Bucket clone = (Bucket)base.Clone(item);
			clone.fluid = fluid;
			return clone;
		}

		public ModFluid fluid;

		//public int liquidType = -1;
		//public int liquidAmount;
		public const int MaxAmount = 2040;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empty Bucket");
			Tooltip.SetDefault("Used to pickup fluids");
		}

		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.autoReuse = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string itemName = fluid == null ? "Empty Bucket" : $"{fluid.DisplayName.GetDefault()} Bucket";
			tooltips.FirstOrDefault(x => x.mod == "Terraria" && x.Name == "ItemName")?.ModifyText(itemName);
			item.SetNameOverride(itemName);

			if (fluid != null)
			{
				tooltips.Insert(1, new TooltipLine(mod, "BucketAmount", $"Volume: {fluid.volume}/{MaxAmount}"));
			}
		}

		//public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		//{

		//	return true;
		//}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (fluid != null)
			{
				spriteBatch.End();

				RasterizerState state = new RasterizerState { ScissorTestEnable = true };

				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, state);

				Rectangle prevRect = spriteBatch.GraphicsDevice.ScissorRectangle;
				float progress = fluid.volume / (float)MaxAmount;
				Main.NewText(position);
				Main.NewText(scale);
				spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)(position.X + 10 * scale), (int)(position.Y + 20 * scale - 10 * scale * progress), 4, (int)(10 * progress));

				spriteBatch.Draw(ModLoader.GetTexture(fluid.Texture), position + new Vector2(10, 10) * scale, new Rectangle(0, 0, 4, 10), Color.White, 0f, origin, scale, SpriteEffects.None, 0f);

				spriteBatch.GraphicsDevice.ScissorRectangle = prevRect;
				spriteBatch.End();
				spriteBatch.Begin();
			}
		}

		public override bool UseItem(Player player)
		{
			if (!Main.GamepadDisableCursorItemIcon)
			{
				player.showItemIcon = true;
				Main.ItemIconCacheUpdate(item.type);
			}

			Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
			if ((fluid == null || fluid.type == tile.liquidType()) && tile.liquid > 0)
			{
				Main.PlaySound(19, (int)player.position.X, (int)player.position.Y);

				if (fluid == null) fluid = Utility.Utility.SetDefaults(tile.liquidType());
				int drain = System.Math.Min(tile.liquid, MaxAmount - fluid.volume);
				fluid.volume += drain;

				tile.liquid -= (byte)drain;

				if (tile.liquid <= 0)
				{
					tile.lava(false);
					tile.honey(false);
				}

				WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, false);
				if (Main.netMode == 1) NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
				else Liquid.AddWater(Player.tileTargetX, Player.tileTargetY);
			}

			/*if (player.itemTime == 0 && player.itemAnimation > 0 && player.controlUseItem)
			{
				Water.pickup(bucket + SuperAbsorbantSponge)
				if (item.type == 205 && Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType() == 0)
				{
					int liqType = Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType();
					int liqAmount = 0;

					checks 3x3 area for liquid, only used for checking, might remove that
					for (int num235 = Player.tileTargetX - 1; num235 <= Player.tileTargetX + 1; num235++)
							{
								for (int num236 = Player.tileTargetY - 1; num236 <= Player.tileTargetY + 1; num236++)
								{
									if (Main.tile[num235, num236].liquidType() == liqType)
									{
										liqAmount += Main.tile[num235, num236].liquid;
									}
								}
							}
					if (tile.liquid > 0)
					{
						int liquidType = tile.liquidType();
						if (!tile.lava())
						{
							if (tile.honey())
							{
								item.stack--;
								this.PutItemInInventory(1128, this.selectedItem);
							}
							else
							{
								item.stack--;
								this.PutItemInInventory(206, this.selectedItem);
							}
						}
						else
						{
							item.stack--;
							this.PutItemInInventory(207, this.selectedItem);
						}

						for (int num238 = Player.tileTargetX - 1; num238 <= Player.tileTargetX + 1; num238++)
						{
							for (int num239 = Player.tileTargetY - 1; num239 <= Player.tileTargetY + 1; num239++)
							{
								if (num237 < 256 && Main.tile[num238, num239].liquidType() == liquidType)
								{
									int num240 = Main.tile[num238, num239].liquid;
									if (num240 + num237 > 255)
									{
										num240 = 255 - num237;
									}
									num237 += num240;
									Tile expr_A154 = Main.tile[num238, num239];
									expr_A154.liquid -= (byte)num240;
									Main.tile[num238, num239].liquidType(liquidType);
									if (Main.tile[num238, num239].liquid == 0)
									{
										Main.tile[num238, num239].lava(false);
										Main.tile[num238, num239].honey(false);
									}
									WorldGen.SquareTileFrame(num238, num239, false);
									if (Main.netMode == 1)
									{
										NetMessage.sendWater(num238, num239);
									}
									else
									{
										Liquid.AddWater(num238, num239);
									}
								}
							}
						}
					}
				}
				*/

			#region Placing
			/*else if (Main.tile[Player.tileTargetX, Player.tileTargetY].liquid < 200 && (!Main.tile[Player.tileTargetX, Player.tileTargetY].nactive() || !Main.tileSolid[Main.tile[Player.tileTargetX, Player.tileTargetY].type] || Main.tileSolidTop[Main.tile[Player.tileTargetX, Player.tileTargetY].type]))
			{
				if (item.type == 207)
				{
					if (Main.tile[Player.tileTargetX, Player.tileTargetY].liquid == 0 || Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType() == 1)
					{
						Main.PlaySound(19, (int)this.position.X, (int)this.position.Y, 1, 1f, 0f);
						Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType(1);
						Main.tile[Player.tileTargetX, Player.tileTargetY].liquid = 255;
						WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
						item.stack--;
						this.PutItemInInventory(205, this.selectedItem);
						this.itemTime = (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(this, item));
						if (Main.netMode == 1)
						{
							NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
						}
					}
				}
				else if (item.type == 206 || item.type == 3031)
				{
					if (Main.tile[Player.tileTargetX, Player.tileTargetY].liquid == 0 || Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType() == 0)
					{
						Main.PlaySound(19, (int)this.position.X, (int)this.position.Y, 1, 1f, 0f);
						Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType(0);
						Main.tile[Player.tileTargetX, Player.tileTargetY].liquid = 255;
						WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
						if (item.type != 3031)
						{
							item.stack--;
							this.PutItemInInventory(205, this.selectedItem);
						}
						this.itemTime = (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(this, item));
						if (Main.netMode == 1)
						{
							NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
						}
					}
				}
				else if (item.type == 1128 && (Main.tile[Player.tileTargetX, Player.tileTargetY].liquid == 0 || Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType() == 2))
				{
					Main.PlaySound(19, (int)this.position.X, (int)this.position.Y, 1, 1f, 0f);
					Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType(2);
					Main.tile[Player.tileTargetX, Player.tileTargetY].liquid = 255;
					WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
					item.stack--;
					this.PutItemInInventory(205, this.selectedItem);
					this.itemTime = (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(this, item));
					if (Main.netMode == 1)
					{
						NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
					}
				}
			}
		}*/
			#endregion

			return true;
		}

		public override TagCompound Save()
		{
			return fluid != null ? new TagCompound
			{
				["Type"] = fluid.Name,
				["Volume"] = fluid.volume
			} : null;
		}

		public override void Load(TagCompound tag)
		{
			if (tag != null)
			{
				ModFluid f = FluidLoader.GetFluid(tag.GetString("Type")).NewInstance();
				f.volume = tag.GetInt("Volume");
				fluid = f;
			}
		}
	}
}