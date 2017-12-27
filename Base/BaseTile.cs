using Terraria.ModLoader;

namespace TheOneLibrary.Base
{
	public class BaseTile : ModTile
	{
		public virtual string Texture => TheOneLibrary.PlaceholderTexture;

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = Texture;
			return base.Autoload(ref name, ref texture);
		}

		public virtual void LeftClick(int i, int j)
		{
		}
	}
}