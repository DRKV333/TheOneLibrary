namespace TheOneLibrary.Fluid.VanillaFluids
{
	public class Honey : ModFluid
	{
		public override string Texture => TheOneLibrary.TexturePath + "Fluid/Honey";

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Honey");
			density = 1420;
		}
	}
}