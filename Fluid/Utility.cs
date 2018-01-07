using System.Linq;
using TheOneLibrary.Fluid;

namespace TheOneLibrary.Utility
{
	public static partial class Utility
	{
		public static ModFluid SetDefaults(int type)
		{
			ModFluid fluid = FluidLoader.fluids.First(x => x.type == type).NewInstance();
			fluid.SetDefaults();
			fluid.SetStaticDefaults();
			return fluid;
		}

		public static ModFluid SetDefaults(string name)
		{
			ModFluid fluid = FluidLoader.fluidNames[name].NewInstance();
			fluid.SetDefaults();
			fluid.SetStaticDefaults();
			return fluid;
		}
	}
}