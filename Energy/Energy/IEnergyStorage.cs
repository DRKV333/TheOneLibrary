namespace TheOneLibrary.Energy.Energy
{
	public interface IEnergyStorage : IEnergyHandler
	{
		long ReceiveEnergy(long maxReceive);

		long ExtractEnergy(long maxExtract);
	}

	public interface IEnergyItem : IEnergyStorage
	{
	}
}