using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TheOneLibrary.Fluid;

namespace TheOneLibrary.Storage
{
	#region Item

	public interface IContainer
	{
		IList<Item> GetItems();
	}

	public interface IContainerItem : IContainer
	{
		ModItem GetItem();
	}

	public interface IContainerTile : IContainer
	{
		ModTileEntity GetTileEntity();
	}

	public interface ISidedContainer : IContainerTile
	{
		IList<Item> GetInputSlots();

		IList<Item> GetOutputSlots();
	}

	#endregion

	#region Fluid

	public interface IFluidContainer
	{
		IList<ModFluid> GetFluids();
		
		void SetFluid(ModFluid value, int slot = 0);

		ModFluid GetFluid(int slot = 0);

		int GetFluidCapacity(int slot = 0);
	}

	public interface IFluidContainerItem : IFluidContainer
	{
		ModItem GetItem();
	}

	public interface ISidedFluidContainer : IFluidContainer
	{
		IList<ModFluid> GetInputTanks();

		IList<ModFluid> GetOutputTanks();
	}

	#endregion
}