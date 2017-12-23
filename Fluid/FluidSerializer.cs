using Terraria.ModLoader.IO;

namespace TheOneLibrary.Fluid
{
	public class FluidSerializer : TagSerializer<ModFluid, TagCompound>
	{
		public override TagCompound Serialize(ModFluid value) => new TagCompound
		{
			["Type"] = value.Name,
			["Volume"] = value.volume
		};

		public override ModFluid Deserialize(TagCompound tag)
		{
			ModFluid fluid = FluidLoader.GetFluid(tag.GetString("Type")).NewInstance();
			fluid.volume = tag.GetInt("Volume");
			return fluid;
		}
	}
}