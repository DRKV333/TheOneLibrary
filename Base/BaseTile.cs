using Terraria.ModLoader;

namespace TheOneLibrary.Base
{
	public class BaseTile : ModTile
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = TheOneLibrary.PlaceholderTexture;
			return base.Autoload(ref name, ref texture);
		}
	}
}