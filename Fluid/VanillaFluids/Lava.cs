namespace TheOneLibrary.Fluid.VanillaFluids
{
	public class Lava : ModFluid
	{
		public override string Texture => TheOneLibrary.TexturePath + "Fluid/Lava";

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Lava");
			density = 2500;
		}
	}
}