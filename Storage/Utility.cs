using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.IO;
using TheOneLibrary.Storage;

namespace TheOneLibrary.Utility
{
	public static partial class Utility
	{
		public static TagCompound Save(this IList<Item> items) => new TagCompound {["Items"] = items.Select(ItemIO.Save).ToList()};

		public static IList<Item> Load(TagCompound tag) => tag["Items"] is List<TagCompound> ? tag.GetList<Item>("Items") : tag.GetCompound("Items").GetList<Item>("Items").ToList();

		public static void Write(this BinaryWriter writer, IList<Item> items) => TagIO.Write(items.Save(), writer);

		public static IList<Item> Read(this BinaryReader reader) => Load(TagIO.Read(reader));

		public static void DropItems(this IContainer container, Rectangle hitbox)
		{
			IList<Item> list = container.GetItems();
			for (var i = 0; i < list.Count; i++)
			{
				Item item = list[i];
				if (!item.IsAir) Item.NewItem(hitbox, item.type, item.stack, prefixGiven: item.prefix);
			}
		}
	}
}