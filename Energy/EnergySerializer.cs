using Terraria.ModLoader.IO;
using TheOneLibrary.Energy.Energy;

namespace TheOneLibrary.Serializers
{
	public class EnergySerializer : TagSerializer<EnergyStorage, TagCompound>
	{
		public override TagCompound Serialize(EnergyStorage value)
		{
			TagCompound tag = new TagCompound();
			if (value.energy < 0) value.energy = 0;

			tag.Set("Energy", value.energy);
			tag.Set("Capacity", value.capacity);
			tag.Set("MaxIn", value.maxReceive);
			tag.Set("MaxOut", value.maxExtract);
			return tag;
		}

		public override EnergyStorage Deserialize(TagCompound tag)
		{
			EnergyStorage storage = new EnergyStorage(tag.GetLong("Capacity"), tag.GetLong("MaxIn"), tag.GetLong("MaxOut"));
			storage.energy = tag.GetLong("Energy") > storage.capacity ? storage.capacity : tag.GetLong("Energy");

			return storage;
		}
	}
}