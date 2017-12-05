using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace TheOneLibrary.Fluid
{
	public static class FluidLoader
	{
		public static IDictionary<string, ModFluid> fluidNames;
		public static IList<ModFluid> fluids;
		public static IList<Texture2D> fluidTexture = new List<Texture2D>();
		public static int NextFluidID = 0;

		static FluidLoader()
		{
			fluidNames = new Dictionary<string, ModFluid>();
			fluids = new List<ModFluid>();
		}

		public static ModFluid GetFluid(int type) => fluids[type] != null ? fluids[type] : null;

		public static ModFluid GetFluid(string name) => fluidNames.ContainsKey(name) ? fluidNames[name] : null;

		public static ModFluid GetNewInstance(string name) => fluidNames.ContainsKey(name) ? fluidNames[name].NewInstance() : null;

		public static ModFluid GetNewInstance(int type) => fluids[type] != null ? fluids[type].NewInstance() : null;

		public static int FluidType(string name)
		{
			ModFluid fluid = GetFluid(name);
			return fluid.type;
		}

		public static int FluidType<T>() where T : ModFluid => FluidType(typeof(T).Name);

		public static void Autoload()
		{
			foreach (Mod mod in ModLoader.LoadedMods)
			{
				if (mod != null && mod.Code != null)
				{
					foreach (Type type in mod.Code.GetTypes())
					{
						if (!type.IsAbstract && type.IsSubclassOf(typeof(ModFluid))) AutoloadFluid(type, mod);
					}
				}
			}
		}

		public static void Unload()
		{
			NextFluidID = 0;
			fluidNames.Clear();
			fluids.Clear();
			fluidTexture.Clear();
		}

		internal static void SetDefaults(ModFluid fluid)
		{
			fluid = GetFluid(fluid.Name).NewInstance();

			fluid.SetDefaults();
		}

		public static void SetupContent()
		{
			foreach (ModFluid fluid in fluidNames.Values)
			{
				SetDefaults(fluid);
				fluid.SetStaticDefaults();
				fluid.AutoStaticDefaults();
			}
		}

		public static int NextFluid()
		{
			int num = NextFluidID;
			NextFluidID++;
			return num;
		}

		public static void AddFluid(string name, ModFluid fluid)
		{
			fluid.type = NextFluid();
			fluid.Name = name;
			fluidNames[name] = fluid;
			fluids.Add(fluid);
		}

		private static void AutoloadFluid(Type type, Mod mod)
		{
			ModFluid fluid = (ModFluid)Activator.CreateInstance(type);
			string name = type.Name;

			fluid.DisplayName = mod.CreateTranslation(mod.Name + "." + name);
			fluid.mod = mod;
			AddFluid(name, fluid);
		}
	}
}