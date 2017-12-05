namespace TheOneLibrary.Heat.Heat
{
	public interface IHeatHandler
	{
		long GetHeat();

		long GetCapacity();

		HeatStorage GetHeatStorage();
	}
}