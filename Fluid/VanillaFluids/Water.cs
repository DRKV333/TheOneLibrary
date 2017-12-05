﻿namespace TheOneLibrary.Fluid.VanillaFluids
{
	public class Water : ModFluid
	{
		public override string Texture => TheOneLibrary.PlaceholderTexture;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Water");
			density = 1000;
		}
	}
}