using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using TheOneLibrary.Fluid;
using TheOneLibrary.Layer;
using TheOneLibrary.Layer.Items;
using TheOneLibrary.Recipe;
using TheOneLibrary.Serializers;

namespace TheOneLibrary
{
	public class TheOneLibrary : Mod
	{
		public static TheOneLibrary Instance;

		public const string TexturePath = "TheOneLibrary/Textures/";
		public const string PlaceholderTexture = TexturePath + "Placeholder";

		public static Texture2D borderTexture;
		public static Texture2D backgroundTexture;

		public static Texture2D corner;
		public static Texture2D side;

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
			borderTexture = TextureManager.Load("Images/UI/PanelBorder");
			backgroundTexture = TextureManager.Load("Images/UI/PanelBackground");

			corner = ModLoader.GetTexture(TexturePath + "BarCorner");
			side = ModLoader.GetTexture(TexturePath + "BarSide");

			TagSerializer.AddSerializer(new EnergySerializer());
			TagSerializer.AddSerializer(new HeatSerializer());

			FluidLoader.Autoload();
			FluidLoader.SetupContent();

			Main.OnPostDraw += Draw;

			if (!Main.dedServ)
			{
				LayerDisplayUI = new LayerDisplayUI();
				LayerDisplayUI.Activate();
				ILayerDisplayUI = new UserInterface();
				ILayerDisplayUI.SetState(LayerDisplayUI);
			}
		}

		public override void Unload()
		{
			recipes.Clear();

			borderTexture = null;
			backgroundTexture = null;

			corner = null;
			side = null;

			FluidLoader.Unload();

			Main.OnPostDraw -= Draw;

			Instance = null;
		}

		public void Draw(GameTime gameTime)
		{
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
			Utility.Utility.DrawMouseText();
			Main.spriteBatch.End();
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));

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
			var something = System.Linq.Enumerable.Range(1, 10);
		}
	}
}