//using System.ComponentModel;
//using Terraria.ModLoader;

//namespace TheOneLibrary
//{
//	public class TOLConfig : ModConfig
//	{
//		public override MultiplayerSyncMode Mode
//		{
//			get { return MultiplayerSyncMode.UniquePerPlayer; }
//		}

//		[DefaultValue("J")] [Label("Changes in which unit energy will be displayed")] public string EnergyUnit;

//		[DefaultValue("W")] [Label("Changes in which unit energy per second will be displayed")] public string EnergyUnitPerSecond;

//		[DefaultValue("J")] [Label("Changes in which unit heat will be displayed")] public string HeatUnit;

//		public override void PostAutoLoad()
//		{
//			TheOneLibrary.Config = this;
//		}
//	}
//}