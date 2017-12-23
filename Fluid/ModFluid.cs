using Terraria.ModLoader;

namespace TheOneLibrary.Fluid
{
	public class ModFluid
	{
		public int type;
		public float density;

		public int volume;

		public ModTranslation DisplayName { get; internal set; }

		public string Name { get; internal set; }

		public Mod mod { get; internal set; }

		public virtual ModFluid Clone() => (ModFluid) MemberwiseClone();

		public virtual string Texture => (GetType().Namespace + "." + Name).Replace('.', '/');

		public virtual bool Autoload(ref string name) => mod.Properties.Autoload;

		public virtual void SetStaticDefaults()
		{
		}

		public void AutoStaticDefaults()
		{
			FluidLoader.fluidTexture.Add(ModLoader.GetTexture(Texture));
		}

		public virtual void SetDefaults()
		{
		}

		public virtual ModFluid NewInstance()
		{
			var copy = (ModFluid) MemberwiseClone();
			return copy;
		}
	}
}