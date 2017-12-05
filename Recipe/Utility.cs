using System.Collections.Generic;
using System.Linq;
using Terraria;
using TheOneLibrary.Recipe;

namespace TheOneLibrary.Utility
{
	public static partial class Utility
	{
		public static bool HasRecipes(this Item item) => Main.recipe.Any(x => x.createItem.type == item.type) || TheOneLibrary.Instance.recipes.Any(x => x.createItems.Contains(item));

		public static bool HasRecipes(int type)
		{
			Item item = new Item();
			item.SetDefaults(type);
			return HasRecipes(item);
		}

		public static bool HasUsages(this Item item) => Main.recipe.Any(x => x.requiredItem.Any(y => y.type == item.type)) || TheOneLibrary.Instance.recipes.Any(x => x.requiredItem.Contains(item));

		public static bool HasUsages(int type)
		{
			Item item = new Item();
			item.SetDefaults(type);
			return HasUsages(item);
		}

		public static List<ItemRecipe> GetRecipes(this Item item)
		{
			List<ItemRecipe> result = new List<ItemRecipe>();

			foreach (Terraria.Recipe recipe in Main.recipe.Where(x => x.createItem.type == item.type))
			{
				ItemRecipe itemRecipe = new ItemRecipe();
				foreach (Item ingredient in recipe.requiredItem) if (!ingredient.IsAir) itemRecipe.AddIngredient(ingredient.type, ingredient.stack);
				foreach (int tile in recipe.requiredTile) if (tile > 0) itemRecipe.AddTile(tile);
				itemRecipe.createItems.Add(recipe.createItem);
				itemRecipe.anyIronBar = recipe.anyIronBar;
				itemRecipe.anyFragment = recipe.anyFragment;
				itemRecipe.anyWood = recipe.anyWood;
				itemRecipe.anySand = recipe.anySand;
				itemRecipe.anyPressurePlate = recipe.anyPressurePlate;
				result.Add(itemRecipe);
			}

			result.AddRange(TheOneLibrary.Instance.recipes.Where(x => x.createItems.Contains(item)));

			return result;
		}

		public static List<ItemRecipe> GetRecipes(int type)
		{
			Item item = new Item();
			item.SetDefaults(type);
			return GetRecipes(item);
		}

		public static List<ItemRecipe> GetUsages(this Item item)
		{
			List<ItemRecipe> result = new List<ItemRecipe>();

			foreach (Terraria.Recipe recipe in Main.recipe.Where(x => x.requiredItem.Select(y => y.type).Contains(item.type)))
			{
				ItemRecipe itemRecipe = new ItemRecipe();
				foreach (Item ingredient in recipe.requiredItem) if (!ingredient.IsAir) itemRecipe.AddIngredient(ingredient.type, ingredient.stack);
				foreach (int tile in recipe.requiredTile) if (tile > 0) itemRecipe.AddTile(tile);
				itemRecipe.createItems.Add(recipe.createItem);
				itemRecipe.anyIronBar = recipe.anyIronBar;
				itemRecipe.anyFragment = recipe.anyFragment;
				itemRecipe.anyWood = recipe.anyWood;
				itemRecipe.anySand = recipe.anySand;
				itemRecipe.anyPressurePlate = recipe.anyPressurePlate;
				result.Add(itemRecipe);
			}

			result.AddRange(TheOneLibrary.Instance.recipes.Where(x => x.requiredItem.Contains(item)));

			return result;
		}

		public static List<ItemRecipe> GetUsages(int type)
		{
			Item item = new Item();
			item.SetDefaults(type);
			return GetUsages(item);
		}
	}
}