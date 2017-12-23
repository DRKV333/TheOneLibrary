using System;

namespace TheOneLibrary.Base
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Field)]
	public class UIAttribute : Attribute
	{
		public string Name;

		public UIAttribute(string name)
		{
			Name = name;
		}
	}

	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class NullAttribute : Attribute
	{
	}
}