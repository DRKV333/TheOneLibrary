using Terraria;

namespace TheOneLibrary.Base.Items
{
	public class Wrench : BaseItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrench");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = 6;
		}
	}
}