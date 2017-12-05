namespace TheOneLibrary.Heat.Heat
{
	public interface IHeatProvider : IHeatHandler
	{
		long ExtractHeat(long maxExtract);
	}
}