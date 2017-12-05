using Terraria.ModLoader.IO;
using TheOneLibrary.Heat.Heat;

namespace TheOneLibrary.Serializers
{
	public class HeatSerializer : TagSerializer<HeatStorage, TagCompound>
	{
		public override TagCompound Serialize(HeatStorage value)
		{
			TagCompound tag = new TagCompound();
			if (value.heat < 0) value.heat = 0;

			tag.Set("Heat", value.heat);
			tag.Set("Capacity", value.capacity);
			tag.Set("MaxIn", value.maxReceive);
			tag.Set("MaxOut", value.maxExtract);
			return tag;
		}

		public override HeatStorage Deserialize(TagCompound tag)
		{
			HeatStorage storage = new HeatStorage(tag.GetLong("Capacity"), tag.GetLong("MaxIn"), tag.GetLong("MaxOut"));
			storage.heat = tag.GetLong("Heat") > storage.capacity ? storage.capacity : tag.GetLong("Heat");

			return storage;
		}
	}
}