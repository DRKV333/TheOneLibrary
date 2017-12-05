using Terraria.UI;

namespace TheOneLibrary.Base.UI
{
	public struct GUI
	{
		public BaseUI key;
		public UserInterface value;

		public GUI(BaseUI key, UserInterface value)
		{
			this.key = key;
			this.value = value;
		}
	}
}