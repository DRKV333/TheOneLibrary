using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Exceptions;
using TheOneLibrary.Fluid;

namespace TheOneLibrary.Recipe
{
	public class ItemRecipe
	{
		public List<Item> createItems = new List<Item>();
		public List<Item> requiredItem = new List<Item>();
		public List<ModFluid> requiredFluids = new List<ModFluid>();
		public List<int> requiredTiles = new List<int>();

		public bool anyWood;
		public bool anyIronBar;
		public bool anyFragment;
		public bool anySand;
		public bool anyPressurePlate;

		public List<int> acceptedGroups = new List<int>();

		public virtual bool RecipeAvailable() => true;

		public void SetResult(int type, int stack = 1)
		{
			Item item = new Item();
			item.SetDefaults(type);
			item.stack = stack;
			createItems.Add(item);
		}

		public void SetResult(ModItem item, int stack = 1, int slot = -1) => SetResult(item.item.type, stack);

		public void AddIngredient(int itemID, int stack = 1, int slot = -1)
		{
			Item item = new Item();
			item.SetDefaults(itemID);
			item.stack = stack;
			if (slot == -1) requiredItem.Add(item);
			else requiredItem[slot] = item;
		}

		public void AddIngredient(ModItem item, int stack = 1, int slot = -1) => AddIngredient(item.item.type, stack, slot);

		public void AddFluid(int fluidID, int volume = 1, int tanks = -1)
		{
			ModFluid fluid = FluidLoader.GetFluid(fluidID).NewInstance();
			fluid.volume = volume;
			if (tanks == -1) requiredFluids.Add(fluid);
			else requiredFluids[tanks] = fluid;
		}

		public void AddFluid(ModFluid fluid, int volume = 1, int tanks = -1) => AddFluid(fluid.type, volume, tanks);

		public void AddTile(int tileID)
		{
			if (tileID < 0 || tileID >= TileLoader.TileCount) throw new RecipeException("No tile has ID " + tileID);
			requiredTiles.Add(tileID);
		}

		public void AddTile(ModTile tile) => AddTile(tile.Type);

		public void AddRecipeGroup(string name, int stack = 1)
		{
			if (!RecipeGroup.recipeGroupIDs.ContainsKey(name)) throw new RecipeException("A recipe group with the name " + name + " does not exist.");
			int id = RecipeGroup.recipeGroupIDs[name];
			RecipeGroup rec = RecipeGroup.recipeGroups[id];
			AddIngredient(rec.ValidItems[rec.IconicItemIndex], stack);
			acceptedGroups.Add(id);
		}

		public virtual void OnCraft(Item item)
		{
		}

		public virtual int ConsumeItem(int type, int numRequired) => numRequired;

		public void AddRecipe()
		{
			if (!createItems.Any()) throw new RecipeException("A recipe without any result has been added.");

			TheOneLibrary.Instance.recipes.Add(this);
		}
	}
}