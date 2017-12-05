namespace TheOneLibrary.Heat.Heat
{
	public interface IHeatReceiver : IHeatHandler
	{
		long ReceiveHeat(long maxReceive);
	}
}