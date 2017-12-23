namespace TheOneLibrary.Fluid.VanillaFluids
{
	public class Water : ModFluid
	{
		public override string Texture => TheOneLibrary.TexturePath + "Fluid/Water";

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Water");
			density = 1000;
		}
	}
}