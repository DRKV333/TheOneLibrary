namespace TheOneLibrary.Heat.Heat
{
	public interface IHeatStorage : IHeatHandler
	{
		long ReceiveHeat(long maxReceive);

		long ExtractHeat(long maxExtract);
	}
}