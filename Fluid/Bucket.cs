using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheOneLibrary.Base;
using TheOneLibrary.Base.Items;
using TheOneLibrary.Storage;

namespace TheOneLibrary.Fluid
{
    public class Bucket : BaseItem, IFluidContainerItem
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
            item.useTime = 15;
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.autoReuse = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (fluid != null) tooltips.Insert(1, new TooltipLine(mod, "BucketAmount", $"Volume: {fluid.volume}/{MaxAmount}"));
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (fluid != null)
            {
                spriteBatch.End();

                RasterizerState state = new RasterizerState { ScissorTestEnable = true };

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, state);

                Rectangle prevRect = spriteBatch.GraphicsDevice.ScissorRectangle;
                float progress = fluid.volume / (float)MaxAmount;

                spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)(position.X + 10 * scale), (int)(position.Y + 20 * scale - 10 * scale * progress), 4, (int)(10 * progress));

                spriteBatch.Draw(ModLoader.GetTexture(fluid.Texture), position + new Vector2(10, 10) * scale, new Rectangle(0, 0, 4, 10), Color.White, 0f, origin, scale, SpriteEffects.None, 0f);

                spriteBatch.GraphicsDevice.ScissorRectangle = prevRect;
                spriteBatch.End();
                spriteBatch.Begin();
            }

            item.SetNameOverride(fluid != null ? fluid.DisplayName.GetTranslation(Language.ActiveCulture) + " Bucket" : "Empty Bucket");
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (fluid != null)
                {
                    Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
                    if ((!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) && TileLoader.GetTile(tile.type)?.GetType().GetAttribute<BucketDisablePlacement>() == null)
                    {
                        if (tile.liquid == 0 || tile.liquidType() == fluid.type)
                        {
                            Main.PlaySound(19, (int)player.position.X, (int)player.position.Y);

                            if (tile.liquid == 0) tile.liquidType(fluid.type);

                            int volume = Math.Min(fluid.volume, 255 - tile.liquid);
                            tile.liquid += (byte)volume;
                            fluid.volume -= volume;
                            if (fluid.volume <= 0) fluid = null;

                            WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY);

                            if (Main.netMode == NetmodeID.MultiplayerClient) NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
                            NetUtility.SyncItem(item);
                        }
                    }
                }
            }
            else
            {
                if (!Main.GamepadDisableCursorItemIcon)
                {
                    player.showItemIcon = true;
                    Main.ItemIconCacheUpdate(item.type);
                }

                Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
                if ((fluid == null || fluid.type == tile.liquidType()) && tile.liquid > 0 && TileLoader.GetTile(tile.type)?.GetType().GetAttribute<BucketDisablePickup>() == null)
                {
                    Main.PlaySound(19, (int)player.position.X, (int)player.position.Y);

                    if (fluid == null) fluid = Utility.Utility.SetDefaults(tile.liquidType());

                    int drain = Math.Min(tile.liquid, MaxAmount - fluid.volume);
                    fluid.volume += drain;

                    tile.liquid -= (byte)drain;

                    if (tile.liquid <= 0)
                    {
                        tile.lava(false);
                        tile.honey(false);
                    }

                    WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, false);
                    if (Main.netMode == NetmodeID.MultiplayerClient) NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
                    else Liquid.AddWater(Player.tileTargetX, Player.tileTargetY);

                    NetUtility.SyncItem(item);
                }
            }

            return true;
        }

        public override TagCompound Save() => fluid != null ? new TagCompound { ["Type"] = fluid.Name, ["Volume"] = fluid.volume } : null;

        public override void Load(TagCompound tag)
        {
            if (tag.Count > 0)
            {
                ModFluid f = Utility.Utility.SetDefaults(tag.GetString("Type"));
                f.volume = tag.GetInt("Volume");
                fluid = f;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.EmptyBucket);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 3);
            recipe.anyIronBar = true;
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public IList<ModFluid> GetFluids() => new List<ModFluid> { fluid };

        public void SetFluid(ModFluid value, int slot = 0) => fluid = value;

        public ModFluid GetFluid(int slot = 0) => fluid;

        public int GetFluidCapacity(int slot = 0) => MaxAmount;

        public ModItem GetItem() => this;
    }
}