using System;
using TheOneLibrary.Energy.Energy;

namespace TheOneLibrary.Utility
{
	public static partial class Utility
	{
		public static float GetProgress(IEnergyProvider storage) => storage.GetEnergyStorage().GetEnergy() / (float) storage.GetEnergyStorage().GetCapacity();

		public static float GetProgress(IEnergyReceiver storage) => storage.GetEnergyStorage().GetEnergy() / (float) storage.GetEnergyStorage().GetCapacity();

		public static long CalculateGeneration(long energy, EnergyStorage storage) => Math.Min(energy, storage.GetCapacity() - storage.GetEnergy());

		public static string AsPower(this long value, bool perSecond = false) => value.ToSI() + (perSecond ? TheOneLibrary.Config.EnergyUnitPerSecond : TheOneLibrary.Config.EnergyUnit + "/s");
	}
}