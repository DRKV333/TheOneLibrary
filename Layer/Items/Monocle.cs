using Terraria.ModLoader;
using TheOneLibrary.Base.Items;

namespace TheOneLibrary.Layer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class Monocle : BaseItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Monocle");
			Tooltip.SetDefault("Shows custom layers");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.vanity = true;
		}
	}
}