using Terraria.ModLoader;

namespace TheOneLibrary.Layer.Layer
{
	public struct LayerInfo
	{
		public Mod Mod;
		public string Texture;
		public string Name;

		public bool Draw { get; set; }
		public bool DrawPreview { get; set; }
	}

	public struct ElementInfo
	{
		public bool Draw { get; set; }
	}
}