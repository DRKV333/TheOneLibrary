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
	}

	public interface ISidedFluidContainer : IFluidContainer
	{
		IList<ModFluid> GetInputTanks();

		IList<ModFluid> GetOutputTanks();
	}

	#endregion
}