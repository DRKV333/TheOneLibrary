using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;
using TheOneLibrary.Layer.Layer;

namespace TheOneLibrary.Utility
{
	public static partial class Utility
	{
		public static CustomDictionary<ILayerElement> ToDictionary<T>(this CustomDictionary<T> dict) where T : ILayerElement
		{
			return new CustomDictionary<ILayerElement>
			{
				internalDict = dict.internalDict.Keys.Zip(dict.internalDict.Values, (point16, element) => new KeyValuePair<Point16, ILayerElement>(point16, element)).ToDictionary(x => x.Key, x => x.Value)
			};
		}
	}
}