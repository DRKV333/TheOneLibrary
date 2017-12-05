namespace TheOneLibrary.Energy.Energy
{
	public interface IEnergyHandler
	{
		long GetEnergy();

		long GetCapacity();

		EnergyStorage GetEnergyStorage();
	}
}