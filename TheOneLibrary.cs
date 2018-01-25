using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using TheOneLibrary.Base;
using TheOneLibrary.Fluid;
using TheOneLibrary.Layer;
using TheOneLibrary.Layer.Items;
using TheOneLibrary.Recipe;
using TheOneLibrary.Serializers;
using TheOneLibrary.Utility;

namespace TheOneLibrary
{
    public class TheOneLibrary : Mod
    {
        [Null] public static TheOneLibrary Instance;
        //[Null] public static TOLConfig Config;

        public const string TexturePath = "TheOneLibrary/Textures/";
        public const string PlaceholderTexture = TexturePath + "Placeholder";

        [Null] public static Texture2D borderTexture;
        [Null] public static Texture2D backgroundTexture;

        [Null] public static Texture2D corner;
        [Null] public static Texture2D side;

        public LayerDisplayUI LayerDisplayUI;
        public UserInterface ILayerDisplayUI;

        public List<ItemRecipe> recipes = new List<ItemRecipe>();

        public TheOneLibrary()
        {
            Properties = new ModProperties
            {
                Autoload = true,
                AutoloadBackgrounds = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }

        public override void Load()
        {
            Instance = this;

            TagSerializer.AddSerializer(new EnergySerializer());
            TagSerializer.AddSerializer(new HeatSerializer());
            TagSerializer.AddSerializer(new FluidSerializer());

            FluidLoader.Autoload();
            FluidLoader.SetupContent();

            if (!Main.dedServ)
            {
                borderTexture = TextureManager.Load("Images/UI/PanelBorder");
                backgroundTexture = TextureManager.Load("Images/UI/PanelBackground");

                corner = ModLoader.GetTexture(TexturePath + "BarCorner");
                side = ModLoader.GetTexture(TexturePath + "BarSide");

                LayerDisplayUI = new LayerDisplayUI();
                LayerDisplayUI.Activate();
                ILayerDisplayUI = new UserInterface();
                ILayerDisplayUI.SetState(LayerDisplayUI);
            }
        }

        public override void Unload()
        {
            recipes.Clear();

            FluidLoader.Unload();

            this.UnloadNullableTypes();

            GC.Collect();
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex + 1, new LegacyGameInterfaceLayer(
                    "OneLibrary: MouseText",
                    delegate
                    {
                        Utility.Utility.DrawMouseText();

                        return true;
                    }, InterfaceScaleType.UI));
            }

            if (InventoryIndex != -1)
            {
                if (!Main.playerInventory && !Main.ingameOptionsWindow && LayerManager.ActiveLayer != null && (Main.LocalPlayer.armor.Any(x => x.type == ItemType<Monocle>()) || Utility.Utility.HeldItem.type == ItemType<LayerTool>()))
                {
                    layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
                        "OneLibrary:Layer: LayerDisplay",
                        delegate
                        {
                            ILayerDisplayUI.Update(Main._drawInterfaceGameTime);
                            LayerDisplayUI.Draw(Main.spriteBatch);

                            return true;
                        }, InterfaceScaleType.UI));
                }
            }
        }

        public int index = -1;

        public override void PostUpdateInput()
        {
            if (Main.keyState.GetPressedKeys().Any(x => x == Keys.RightControl) && Utility.Utility.HeldItem.type == ItemType<LayerTool>())
            {
                if (Main.mouseRight && Main.mouseRightRelease)
                {
                    ((LayerTool)Utility.Utility.HeldItem.modItem).mode = ((LayerTool)Utility.Utility.HeldItem.modItem).mode.NextEnum();
                    Main.NewText(((LayerTool)Utility.Utility.HeldItem.modItem).mode);
                }
            }

            if (LayerManager.layers.Count > 0 && Main.LocalPlayer.armor.Any(x => x.type == ItemType<Monocle>()))
            {
                if (Main.keyState.GetPressedKeys().Any(x => x == Keys.RightControl))
                {
                    if (PlayerInput.ScrollWheelDelta > 0) index--;
                    else if (PlayerInput.ScrollWheelDelta < 0) index++;
                }

                if (index <= -1)
                {
                    LayerManager.ActiveLayer = null;
                    index = -1;
                }
                else
                {
                    index = Utility.Utility.Clamp(index, 0, LayerManager.layers.Count - 1);
                    LayerManager.ActiveLayer = LayerManager.layers.Values.ToList()[index];
                    LayerDisplayUI.ChangeLayer();
                }
            }
        }

        public static void RedundantFunc()
        {
            var something = Enumerable.Range(1, 10);
        }
    }

    public class TOLPlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            if (!Main.mapFullscreen)
            {
                int myX = Player.tileTargetX;
                int myY = Player.tileTargetY;
                if (player.position.X / 16f - Player.tileRangeX <= myX && (player.position.X + player.width) / 16f + Player.tileRangeX - 1f >= myX && player.position.Y / 16f - Player.tileRangeY <= myY && (player.position.Y + player.height) / 16f + Player.tileRangeY - 2f >= myY)
                {
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        if (Main.tile[myX, myY] == null) Main.tile[myX, myY] = new Tile();
                        if (Main.tile[myX, myY].active())
                        {
                            int type = Main.tile[myX, myY].type;
                            ModTile tile = TileLoader.GetTile(type);
                            (tile as BaseTile)?.LeftClick(myX, myY);
                        }
                    }

                    if (Main.mouseRight)
                    {
                        if (Main.tile[myX, myY] == null) Main.tile[myX, myY] = new Tile();
                        if (Main.tile[myX, myY].active())
                        {
                            int type = Main.tile[myX, myY].type;
                            ModTile tile = TileLoader.GetTile(type);
                            (tile as BaseTile)?.RightClickCont(myX, myY);
                        }
                    }

                    if (Main.mouseLeft)
                    {
                        if (Main.tile[myX, myY] == null) Main.tile[myX, myY] = new Tile();
                        if (Main.tile[myX, myY].active())
                        {
                            int type = Main.tile[myX, myY].type;
                            ModTile tile = TileLoader.GetTile(type);
                            (tile as BaseTile)?.LeftClickCont(myX, myY);
                        }
                    }
                }
            }
        }
    }
}